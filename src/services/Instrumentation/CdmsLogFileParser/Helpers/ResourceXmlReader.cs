using System.Reflection;
using System.Xml;

namespace CdmsLogFileParser.Helpers
{
    /// <summary>
    /// Represents an XmlTextReader containing the contents of an embedded resource
    /// </summary>
    public class ResourceXmlReader : XmlTextReader
    {
        private static ResourceReader _resource = new ResourceReader();

        /// <summary>
        /// Represents an XmlTextReader containing the contents of an embedded resource
        /// </summary>
        /// <param name="xsdResourceName">The fully qualified name of the resource</param>
        public ResourceXmlReader(string xsdResourceName)
            : base(_resource.GetResource(xsdResourceName))
        {
        }

        /// <summary>
        /// Represents an XmlTextReader containing the contents of an embedded resource
        /// </summary>
        /// <param name="xsdResourceName">The fully qualified name of the resource</param>
        /// <param name="assembly">assembly which contains the specified resource name</param>
        public ResourceXmlReader(string xsdResourceName, Assembly assembly)
            : base(_resource.GetResource(xsdResourceName, assembly))
        {
        }
    }
}
