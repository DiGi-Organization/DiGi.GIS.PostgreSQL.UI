using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for UserSignInWindow.xaml
    /// <para>Asks for the email and password of the user a task writes through the GIS Web API as (DiGi.GIS.PostgreSQL.UI#48). The window only collects them: the task signs in with them on its own thread (<see cref="Query.UserTokenStringAsync"/>), because a request must not block the user interface thread.</para>
    /// <para>Like <see cref="PostgreSQLUserCreateOptionsWindow"/>, it carries no options object: the password reaches it as text, and an options instance would keep that text alive on the task for the life of the process. The task reads the values, signs in and lets the window go within one run.</para>
    /// </summary>
    public partial class UserSignInWindow : Window
    {
        private string? email;

        private string? password;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSignInWindow"/> class.
        /// </summary>
        public UserSignInWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the email to sign in with. It carries the value of the control only once the dialog has been closed with OK; until then, and after a cancellation, it is null.
        /// </summary>
        public string? Email
        {
            get
            {
                return email;
            }
        }

        /// <summary>
        /// Gets the password to sign in with. It exists only between the dialog being closed with OK and the sign-in request, and is never written anywhere.
        /// </summary>
        public string? Password
        {
            get
            {
                return password;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            string? email_Value = string.IsNullOrWhiteSpace(TextBoxControl_Email.Value) ? null : TextBoxControl_Email.Value!.Trim();
            if (email_Value is null)
            {
                MessageBox.Show("An email has to be given.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Not trimmed: a password is sent exactly as it was typed.
            string password_Value = PasswordBox_Password.Password;
            if (string.IsNullOrEmpty(password_Value))
            {
                MessageBox.Show("A password has to be given.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            email = email_Value;
            password = password_Value;

            DialogResult = true;
            Close();
        }
    }
}
