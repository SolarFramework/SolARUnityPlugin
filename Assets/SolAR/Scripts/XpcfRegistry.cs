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
            [DefaultValue("")]
            public string name;
            [XmlAttribute]
            public string uuid;
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
                [DefaultValue("")]
                public string name;
                [XmlAttribute]
                public string uuid;
                [XmlAttribute]
                [DefaultValue("")]
                public string description;

                [XmlElement("interface")]
                public List<Interface> interfaces = new List<Interface>();
                [Serializable]
                public class Interface
                {
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string name;
                    [XmlAttribute]
                    public string uuid;
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
        */
        [XmlArrayItem("alias")]
        public List<Alias> aliases = new List<Alias>();
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
            public List<Bind1> bindings = new List<Bind1>();
            [Serializable]
            public class Bind1
            {
                [XmlAttribute]
                [DefaultValue("")]
                public string name;
                [XmlAttribute("interface")]
                public string @interface;
                [XmlAttribute]
                public string to;
                [XmlAttribute]
                [DefaultValue(Range.@default)]
                public Range range = Range.@default;
                public enum Range { @default, all }

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
            [Serializable]
            public class Inject
            {
                [XmlAttribute]
                public string to;

                [XmlElement("bind")]
                public List<Bind2> binds = new List<Bind2>();
                [Serializable]
                public class Bind2
                {
                    [XmlAttribute("interface")]
                    public string @interface;
                    [XmlAttribute]
                    public string to;
                    [XmlAttribute]
                    public string properties;
                }
            }
        }

        [Obsolete("Use properties")]
        [XmlArray("configuration")]
        [XmlArrayItem("component")]
        public List<Configure> componentsConfig = new List<Configure>();

        [XmlArray("properties")]
        [XmlArrayItem("configure")]
        public List<Configure> properties = new List<Configure>();
        [Serializable]
        public class Configure
        {
            [XmlAttribute]
            public string component;

            [XmlAttribute]
            [DefaultValue("")]
            public string name;

            [XmlElement("property")]
            public List<Property2> properties = new List<Property2>();
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

                [XmlElement("value")]
                public string[] values;
                //public bool ShouldSerializevalues() => values?.Length > 0;
#if TRUE
            }
            [Serializable]
            public class Property2 : Property
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
