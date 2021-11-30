using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace SolAR
{
    [Serializable]
    [XmlRoot("xpcf-registry")]
    public class XpcfRegistry //: XpcfConfiguration
    {
        [XmlAttribute]
        public bool autoAlias = false;
        [XmlElement("module")]
        public List<Module> modules = new List<Module>();
        [Serializable]
        public class Module
        {
            [XmlAttribute]
            public string uuid;
            [XmlAttribute]
            [DefaultValue("")]
            public string name;
            [XmlAttribute]
            [DefaultValue("")]
            public string path;
            [XmlAttribute]
            [DefaultValue("")]
            public string description;

            [XmlElement("component")]
            public List<Component> components = new List<Component>();
            [Serializable]
            public class Component
            {
                [XmlAttribute]
                public string uuid;
                [XmlAttribute]
                [DefaultValue("")]
                public string name;
                [XmlAttribute]
                [DefaultValue("")]
                public string description;

                [XmlElement("interface")]
                public List<Interface> interfaces = new List<Interface>();
                [Serializable]
                public class Interface
                {
                    [XmlAttribute]
                    public string uuid;
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string name;
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string description;
                }
            }
        }
        /*
    }

    [Serializable]
    [XmlRoot("xpcf-configuration")]
    public class XpcfConfiguration
    {
        // */
        [XmlArrayItem("alias")]
        public List<Alias> aliases = new List<Alias>();
        public bool ShouldSerializealiases() => aliases?.Count > 0;
        [Serializable]
        public class Alias
        {
            [XmlAttribute]
            [DefaultValue("")]
            public string name;
            [XmlAttribute]
            public Type type;
            public enum Type { component, @interface }
            [XmlAttribute]
            public string uuid;
        }

        public Factory factory;
        [Serializable]
        public class Factory
        {
            [XmlArrayItem("bind")]
            public List<FactoryBind> bindings = new List<FactoryBind>();
            public bool ShouldSerializebindings() => bindings?.Count > 0;
            [Serializable]
            public class FactoryBind
            {
                [XmlAttribute("interface")]
                public string @interface;
                [XmlAttribute]
                public string to;
                [XmlAttribute]
                [DefaultValue("default")]
                public string range = "default";
                [XmlAttribute]
                [DefaultValue("")]
                public string name;
                [XmlAttribute]
                [DefaultValue("transient")]
                public string scope =  "transient"; // Singleton
                [XmlAttribute]
                [DefaultValue("")]
                public string properties;

                [XmlElement("component")]
                public List<ComponentBind> component = new List<ComponentBind>();
                [Serializable]
                public class ComponentBind
                {
                    [XmlAttribute]
                    public string to;
                }
            }

            [XmlArrayItem("inject")]
            public List<Inject> injects = new List<Inject>();
            public bool ShouldSerializeinjects() => injects?.Count > 0;
            [Serializable]
            public class Inject
            {
                [XmlAttribute]
                public string to;

                [XmlElement("bind")]
                public List<InjectBind> binds = new List<InjectBind>();
                [Serializable]
                public class InjectBind
                {
                    [XmlAttribute("interface")]
                    public string @interface;
                    [XmlAttribute]
                    public string to;
                    [XmlAttribute]
                    [DefaultValue("default")]
                    public string range = "default";
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string name;
                    [XmlAttribute]
                    [DefaultValue("transient")]
                    public string scope = "transient"; // Singleton
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string properties;
                }
            }
        }

        [Obsolete("Use properties")]
        [XmlArray("configuration")]
        [XmlArrayItem("component")]
        public List<Configure> componentsConfig = new List<Configure>();
        [Obsolete]
        public bool ShouldSerializecomponentsConfig() => componentsConfig?.Count > 0;

        [XmlArray("properties")]
        [XmlArrayItem("configure")]
        public List<Configure> properties = new List<Configure>();
        public bool ShouldSerializeproperties() => properties?.Count > 0;
        [Serializable]
        public class Configure
        {
            [XmlAttribute]
            public string component;

            [XmlAttribute]
            [DefaultValue("")]
            public string name;

            [XmlElement("property")]
            public List<PropertyRecursive> properties = new List<PropertyRecursive>();
            [Serializable]
            public class Property
            {
                [XmlAttribute]
                public string name;
                [XmlAttribute]
                public TYPE type;
                public enum TYPE { @int, @uint, @long, @ulong, @string, wstring, @float, @double, structure }
                [XmlAttribute]
                [DefaultValue("")]
                public string value;
                [XmlAttribute]
                [DefaultValue(Access.@default)]
                public Access access;
                public enum Access { @default, ro }
                [XmlAttribute]
                [DefaultValue("")]
                public string description;

                [XmlElement("value")]
                public string[] values;
#if TRUE
            }
            [Serializable]
            public class PropertyRecursive : Property
            {
#endif
                [XmlElement("property")]
                public List<Property> properties = new List<Property>();
            }

            /*
            // Comment
            [XmlAnyElement("Comment")]
            public XmlComment Comment { get => new XmlDocument().CreateComment(uuid); set { } }
            [XmlIgnore]
            public string uuid; // Used to speedup UI
            */
        }
    }
}
