using System;
using System.IO;
using System.Reflection;

namespace CdmsLogFileParser.Helpers
{
    /// <summary>
    /// Represents an stream containing the contents of an
    /// embedded resource
    /// </summary>
    public class ResourceReader
    {
        private Assembly _assembly;

        /// <summary>
        /// Represents a System.IO.Stream that contains the contents
        /// of an embedded resource
        /// </summary>
        public ResourceReader()
        {
            // Get this type's assembly so we can load embedded resources
            // from the assembly manifest
            _assembly = this.GetType().Assembly;
        }

        /// <summary>
        /// Retrieve a System.IO.Stream object containing contents of 
        /// embedded resource
        /// </summary>
        /// <param name="xsdResourceName">The fully qualified name of the embedded resource</param>
        /// <returns>Stream containing embedded resource contents</returns>
        public Stream GetResource(string xsdResourceName)
        {
            if (xsdResourceName == null) throw new ArgumentNullException("xsdResourceName");

            // Load the embedded resource specified by xsdResourceName
            // place the contents in a xml text reader
            var stream = _assembly.GetManifestResourceStream(xsdResourceName);

            if (null == stream)
            {
                throw new ApplicationException(string.Format(@"ResourceStream not found for '{0}'", xsdResourceName));
            }
            return stream;
        }

        public Stream GetResource(string xsdResourceName, Assembly assembly)
        {
            if (xsdResourceName == null) throw new ArgumentNullException("xsdResourceName");

            // Load the embedded resource specified by xsdResourceName
            // place the contents in a xml text reader
            var stream = assembly.GetManifestResourceStream(xsdResourceName);

            if (null == stream)
            {
                throw new ApplicationException(string.Format(@"ResourceStream not found for '{0}' in assembly '{1}'", xsdResourceName, assembly.FullName));
            }
            return stream;
        }

        public string GetResourceString(string xsdResourceName, Assembly assembly)
        {
            if (xsdResourceName == null) throw new ArgumentNullException("xsdResourceName");

            // Load the embedded resource specified by xsdResourceName
            // place the contents in a Stream reader
            var stream = assembly.GetManifestResourceStream(xsdResourceName);

            if (null == stream)
            {
                throw new ApplicationException(string.Format(@"ResourceStream not found for '{0}' in assembly '{1}'", xsdResourceName, assembly.FullName));
            }

            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
