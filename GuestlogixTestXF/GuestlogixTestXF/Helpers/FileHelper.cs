using GuestlogixTestXF.Core;
using System.IO;
using System.Reflection;

namespace GuestlogixTestXF
{
    public class FileHelper : IFileHelper
    {
        /// <summary>
        /// Method to read a resource file as a stream
        /// </summary>
        public Stream ReadResourceFileAsStream(string name)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            return assembly.GetManifestResourceStream(name);
        }
    }
}
