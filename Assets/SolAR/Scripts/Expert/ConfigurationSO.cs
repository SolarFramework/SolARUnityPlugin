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

        public XpcfRegistry conf;

#if UNITY_EDITOR
        protected void Reset()
        {
            path = UnityEditor.AssetDatabase.GetAssetPath(this);
            path = Path.ChangeExtension(path, ".xml");
        }
#endif

        public void Load()
        {
            var serializer = new XmlSerializer(typeof(XpcfRegistry));
            using (var stream = File.OpenText(path))
            {
                conf = (XpcfRegistry)serializer.Deserialize(stream);
            }
        }
    }

    [Serializable]
    //[Obsolete]
    public class Configuration
    {
        public string path;

        public XpcfRegistry conf;

        [ContextMenu("Load")]
        public void Load()
        {
            var serializer = new XmlSerializer(typeof(XpcfRegistry));
            using (var stream = File.OpenText(path))
            {
                conf = (XpcfRegistry)serializer.Deserialize(stream);
            }
        }

        [ContextMenu("Save")]
        public void Save()
        {
            var serializer = new XmlSerializer(typeof(XpcfRegistry));
            using (var stream = File.CreateText(path))
            {
                serializer.Serialize(stream, conf);
            }
        }
    }
}
