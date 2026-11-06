using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Computes the SHA-256 of a file, streamed block by block rather than read into memory (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38).
        /// <para>A release of an open building dataset reaches hundreds of gigabytes, so the digest is computed while the file is read, and the read observes the cancellation token - a cancelled run stops between blocks instead of only after the whole file.</para>
        /// <para>The digest is the lowercase hexadecimal form. A path that does not name an existing file answers null rather than throwing, and a read failure is logged and answers null, so a caller cannot silently store an empty or malformed digest as provenance.</para>
        /// </summary>
        /// <param name="path">The file to hash.</param>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
        /// <returns>A task whose result is the lowercase hexadecimal SHA-256 of the file, or null when the file does not exist or could not be read.</returns>
        public static async Task<string?> FileSHA256Async(string? path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                await using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
                using IncrementalHash incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

                byte[] buffer = new byte[81920];
                int bytesRead;
                while ((bytesRead = await fileStream.ReadAsync(buffer.AsMemory(), cancellationToken)) > 0)
                {
                    incrementalHash.AppendData(buffer, 0, bytesRead);
                }

                return System.Convert.ToHexStringLower(incrementalHash.GetHashAndReset());
            }
            catch (OperationCanceledException)
            {
                // Cancellation is the caller's decision, not a hashing failure - the base background task
                // already treats an exception of its own token as a cancellation rather than a fault.
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Method}: {Path} could not be hashed", nameof(FileSHA256Async), path);
                return null;
            }
        }
    }
}
