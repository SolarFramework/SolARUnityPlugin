using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace SolAR
{
    [CreateAssetMenu(fileName = "conf_", menuName = "SolAR/conf")]
    [Serializable]
    [Obsolete]
    public class ConfigurationSO : ScriptableObject
    {
        public string path;

        public ConfXml conf;

        protected void Reset()
        {
            path = UnityEditor.AssetDatabase.GetAssetPath(this);
            path = Path.ChangeExtension(path, ".xml");
        }

        public void Load()
        {
            var serializer = new XmlSerializer(typeof(ConfXml));
            using (var stream = File.OpenRead(path))
            {
                conf = (ConfXml)serializer.Deserialize(stream);
            }
        }
    }

    [Serializable]
    //[Obsolete]
    public class Configuration
    {
        public string path;

        public ConfXml conf;

        [ContextMenu("Load")]
        public void Load()
        {
            var serializer = new XmlSerializer(typeof(ConfXml));
            using (var stream = File.Open(path, FileMode.Open))
            {
                conf = (ConfXml)serializer.Deserialize(stream);
            }
        }

        [ContextMenu("Save")]
        public void Save()
        {
            var serializer = new XmlSerializer(typeof(ConfXml));
            using (var stream = File.Open(path, FileMode.Create))
            {
                serializer.Serialize(stream, conf);
            }
        }
    }
}
