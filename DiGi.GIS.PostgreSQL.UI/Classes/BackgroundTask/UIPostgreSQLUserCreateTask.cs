using DiGi.Core.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.User.Classes;
using DiGi.User.Enums;
using DiGi.User.PostgreSQL.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Creates one user in the user database, asking for it through <see cref="PostgreSQLUserCreateOptionsWindow"/> each time the task is started.
    /// <para>The password is stored only as a PBKDF2 derived key with its salt and iteration count. The text the operator typed reaches the derivation and nothing else - not the row, not the log, and not a field on this task.</para>
    /// <para>The user database is not the database the rest of this application works against: users live where <c>User_PostgreSQL_Main.conf</c> points, which is what the Web API reads when it authenticates a login. A user written anywhere else could not log in.</para>
    /// <para>There is no base task to hand the run to. <see cref="UserPostgreSQLConverter"/> already carries every database step, and creating a user is three of them.</para>
    /// <para>Every refusal - a taken email, a database that cannot be reached - is thrown as a <see cref="BackgroundTaskFailureException"/> rather than returned, so the task row carries the reason on hover instead of a bare Failed with the reason buried in the log file. A cancelled dialog is not a refusal: the run ends as canceled through <see cref="CancelableBackgroundTask"/>, and the tray logs no error (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#61).</para>
    /// </summary>
    public class UIPostgreSQLUserCreateTask : CancelableBackgroundTask, IGISPostgreSQLUIObject
    {
        private readonly UserPostgreSQLConverter userPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLUserCreateTask"/> class.
        /// </summary>
        /// <param name="userPostgreSQLConverter">The converter addressing the user database the new user is written to.</param>
        public UIPostgreSQLUserCreateTask(UserPostgreSQLConverter userPostgreSQLConverter)
        {
            this.userPostgreSQLConverter = userPostgreSQLConverter ?? throw new ArgumentNullException(nameof(userPostgreSQLConverter));
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(CancellationToken token)
        {
            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the new user cannot be asked for");
                throw new BackgroundTaskFailureException("No WPF application is running - the new user cannot be asked for");
            }

            string? email = null;
            string? firstName = null;
            string? lastName = null;
            string? password = null;
            UserLevel? userLevel = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLUserCreateOptionsWindow postgreSQLUserCreateOptionsWindow = new();

                if (postgreSQLUserCreateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                email = postgreSQLUserCreateOptionsWindow.Email;
                firstName = postgreSQLUserCreateOptionsWindow.FirstName;
                lastName = postgreSQLUserCreateOptionsWindow.LastName;
                password = postgreSQLUserCreateOptionsWindow.Password;
                userLevel = postgreSQLUserCreateOptionsWindow.UserLevel;
            });

            // A cancelled dialog leaves these unassigned and ends the run here as canceled, having written nothing -
            // not as a failure, which the tray would log as an error (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#61).
            if (email is null || password is null || userLevel is null)
            {
                Serilog.Modify.Log("Creating a user was cancelled - nothing was written");
                CancelRun();
                return false;
            }

            // The whole storage chain, before the duplicate check below reads any of it: CreateTableAsync creates
            // the database when it is absent, then the table, then the credential columns on a table that predates
            // them. All three are idempotent.
            //
            // It has to come first because the duplicate check is a read, and a read finds neither of the first two
            // for itself - an absent database fails the connection with 3D000, and a database without the table
            // fails the statement with 42P01. Neither is something a read should repair.
            // A Stop is honoured only before the first write: the table set-up and the duplicate check take the
            // run's token, while the insert, the credential and the undo below take none, so a stop never leaves a
            // user row half written or a failed write not undone.
            if (!await userPostgreSQLConverter.CreateTableAsync(cancellationToken: token))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The user database could not be created or reached - no user was created");
                throw new BackgroundTaskFailureException("The user database could not be created or reached - no user was created");
            }

            // Refused rather than upserted. InsertAsync writes ON CONFLICT (email) DO UPDATE, so without this
            // check a taken email would silently rewrite that user's name and level instead of failing.
            if (await userPostgreSQLConverter.GetUserByEmailAsync(email, cancellationToken: token) is not null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "A user with the email {Email} already exists - no user was created", email);
                throw new BackgroundTaskFailureException($"A user with the email {email} already exists - no user was created");
            }

            UserCredential? userCredential = DiGi.User.PostgreSQL.Create.UserCredential(email, password);
            if (userCredential is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "A password credential could not be derived for {Email} - no user was created", email);
                throw new BackgroundTaskFailureException($"A password credential could not be derived for {email} - no user was created");
            }

            // InsertAsync creates the users table when it is not there yet and adds the credential columns to one
            // that predates them, so the first run against a database is also what migrates it.
            List<string> ids = await userPostgreSQLConverter.InsertAsync([new DiGi.User.Classes.User(email) { FirstName = firstName, LastName = lastName, Level = (int)userLevel.Value }], cancellationToken: CancellationToken.None);
            if (ids.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The user {Email} could not be written", email);
                throw new BackgroundTaskFailureException($"The user {email} could not be written");
            }

            // Undoes the insert above when the credential cannot be written. Without it the run leaves a row that
            // cannot log in and that the duplicate check above would refuse to write over, so the email would be
            // spent until somebody deleted the row by hand.
            async Task RemoveAsync()
            {
                try
                {
                    // Only a row that carries no credential at all. InsertAsync upserts and deliberately leaves an
                    // existing user's credential columns alone, so a row that has one belonged to somebody before
                    // this run started and must not be removed by it.
                    if (await userPostgreSQLConverter.GetUserCredentialAsync(email, cancellationToken: CancellationToken.None) is null)
                    {
                        await userPostgreSQLConverter.DeleteUserByEmailAsync(email, cancellationToken: CancellationToken.None);
                    }
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(exception, "The user {Email} was written without a password credential and could not be removed - it has to be deleted by hand before that email can be used again", email);
                }
            }

            // Second, because SetUserCredentialAsync is an UPDATE against a row that has to exist already. It does
            // not catch NpgsqlException, so a connection lost here arrives as a throw rather than as false.
            bool credentialWritten;

            try
            {
                credentialWritten = await userPostgreSQLConverter.SetUserCredentialAsync(userCredential, cancellationToken: CancellationToken.None);
            }
            catch
            {
                await RemoveAsync();
                throw;
            }

            if (!credentialWritten)
            {
                await RemoveAsync();

                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The password credential of {Email} could not be written - no user was created", email);
                throw new BackgroundTaskFailureException($"The password credential of {email} could not be written - no user was created");
            }

            Serilog.Modify.Log("User {Email} created with level {UserLevel}", email, userLevel.Value);

            return true;
        }
    }
}
