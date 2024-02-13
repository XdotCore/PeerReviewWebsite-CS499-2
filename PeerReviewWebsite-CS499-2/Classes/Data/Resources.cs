using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PeerReviewWebsite.Classes.Data {
    /// <summary>
    /// Loads embedded resources from the assembly
    /// </summary>
    public static class Resources {
        // Assembly information that never changes
        private static readonly Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private static readonly string assemblyName = thisAssembly.GetName().Name.Replace(" ", "");

        /// <summary>
        /// Gets the stream for the resource of the given name
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns><see langword="null"/> if an exception is caught, otherwise a <see cref="Stream"/> that contains the resource</returns>
        /// <remarks>You are responsible for disposing the stream, use <see langword="using"/></remarks>
        private static Stream GetResourceStream(string resourceName) {
            string fullName = $"{assemblyName}.{resourceName}";

            try {
                return thisAssembly.GetManifestResourceStream(fullName);
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the string for a text resource of the given name
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns><see langword="null"/> if the resource could not be retrieved, otherwise a <see cref="string"/> from the resource</returns>
        public static string GetResourceText(string resourceName) {
            // Get resource
            using Stream resource = GetResourceStream(resourceName);
            if (resource is null)
                return null;

            // Get string
            using StreamReader reader = new(resource);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Same as <see cref="GetResourceText(string)"/>, but looks for .sql in the SQL folder automatically
        /// </summary>
        /// <param name="sqlName">The name of the sql file</param>
        /// <returns><see langword="null"/> if the .sql file could not be retrieved, otherwise a <see cref="string"/> of the sql</returns>
        public static string GetSQL(string sqlName) => GetResourceText($"SQL.{sqlName}.sql");
    }
}
