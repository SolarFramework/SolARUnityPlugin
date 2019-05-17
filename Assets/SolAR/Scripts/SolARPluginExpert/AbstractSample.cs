using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UniRx;
using UnityEngine;

#pragma warning disable IDE1006 // Styles d'affectation de noms
namespace SolAR
{
    public abstract class AbstractSample : MonoBehaviour
    {
    protected readonly CompositeDisposable subscriptions = new CompositeDisposable();

        [HideInInspector]
        public Configuration conf;
        //public ConfigurationSO confSO;

        [ContextMenu("Load")]
        void Load()
        {
            var serializer = new XmlSerializer(typeof(ConfXml));
            using (var stream = File.Open(conf.path, FileMode.Open))
            {
                conf.conf = (ConfXml)serializer.Deserialize(stream);
            }
        }

        [ContextMenu("Save")]
        void Save()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true, }; //TODO: check Unicode BOM
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var serializer = new XmlSerializer(typeof(ConfXml));
                    var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });
                    serializer.Serialize(xmlWriter, conf.conf, namespaces);
                }
                File.WriteAllText(conf.path, stringWriter.ToString());
            }
        }

        protected Texture2D inputTex;

        protected virtual void OnEnable()
        {
            foreach (var kvp in conf.conf.modules.ToDictionary(m => m.name, m => m.uuid))
            {
                ComponentExtensions.modulesDict[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in conf.conf.modules.SelectMany(m => m.components).ToDictionary(c => c.name, c => c.uuid))
            {
                ComponentExtensions.componentsDict[kvp.Key] = kvp.Value;
            }
            var comparer = new KeyBasedEqualityComparer<ConfXml.Module.Component.Interface, string>(i => i.uuid);
            foreach (var kvp in conf.conf.modules.SelectMany(m => m.components).SelectMany(c => c.interfaces).Distinct(comparer).ToDictionary(i => i.name, i => i.uuid))
            {
                ComponentExtensions.interfacesDict[kvp.Key] = kvp.Value;
            }
        }

        protected virtual void OnDisable()
        {
            subscriptions.Clear();
        }

        protected void printf(string format, params object[] objs) { Debug.LogFormat(format, objs); }

        protected void LOG_ERROR(string message, params object[] objects) { Debug.LogErrorFormat(this, message, objects); }
        protected void LOG_INFO(string message, params object[] objects) { Debug.LogWarningFormat(this, message, objects); }
        protected void LOG_DEBUG(string message, params object[] objects) { Debug.LogFormat(this, message, objects); }

        protected void LOG_ADD_LOG_TO_CONSOLE() { }

        protected const long CLOCKS_PER_SEC = TimeSpan.TicksPerSecond;

        protected static long clock() { return DateTimeOffset.Now.Ticks; }
    }
}
#pragma warning restore IDE1006 // Styles d'affectation de noms
