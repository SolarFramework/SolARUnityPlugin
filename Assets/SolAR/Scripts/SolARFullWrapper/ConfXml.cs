using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace SolAR
{
    [Serializable]
    [XmlRoot("xpcf-registry")]
    public class ConfXml
    {
        [XmlElement("module")]
        public Module[] modules;
        [Serializable]
        public class Module
        {
            [XmlAttribute]
            public string uuid;
            [XmlAttribute]
            public string name;
            [XmlAttribute]
            public string path;
            [XmlAttribute]
            public string description;

            [XmlElement("component")]
            public Component[] components;
            [Serializable]
            public class Component
            {
                [XmlAttribute]
                public string uuid;
                [XmlAttribute]
                public string name;
                [XmlAttribute]
                public string description;

                [XmlElement("interface")]
                public Interface[] interfaces;
                [Serializable]
                public class Interface
                {
                    [XmlAttribute]
                    public string uuid;
                    [XmlAttribute]
                    public string name;
                    [XmlAttribute]
                    public string description;
                }
            }
        }

        public Configuration configuration;
        [Serializable]
        public class Configuration
        {
            [XmlElement("component")]
            public ComponentConf[] components;
            [Serializable]
            public class ComponentConf
            {
                [XmlIgnore]
                public string type;
                [XmlAnyElement("Comment")]
                public XmlComment Comment { get { return new XmlDocument().CreateComment(type); } set { } }

                //[XmlAnyElement]
                //public XmlElement[] elements = new XmlElement[2];

                //[XmlAnyAttribute]
                //public XmlAttribute[] attributes = new XmlAttribute[2];

                [XmlAttribute]
                public string uuid;
                [XmlAttribute]
                [DefaultValue("")]
                public string name;

                [XmlIgnore]
                public string description;

                [XmlElement("property")]
                public Property[] properties;
                [Serializable]
                public class Property
                {
                    [XmlAttribute]
                    public string name;
                    [XmlAttribute]
                    public string type;
                    public enum TYPE
                    {
                        String,
                        Float,
                        Integer,
                        UnsignedInteger,
                        Double,
                    }
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string value;
                    [XmlAttribute]
                    [DefaultValue("")]
                    public string description;

                    [XmlElement("value")]
                    public string[] values;
                    public bool ShouldSerializevalues() { return values != null && values.Length > 0; }
                }
            }
        }
    }
}
