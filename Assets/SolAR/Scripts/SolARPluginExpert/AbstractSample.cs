using System;
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
            var serializer = new XmlSerializer(typeof(XpcfRegistry));
            using (var stream = File.OpenText(conf.path))
            {
                conf.conf = (XpcfRegistry)serializer.Deserialize(stream);
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
                    var serializer = new XmlSerializer(typeof(XpcfRegistry));
                    var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });
                    serializer.Serialize(xmlWriter, conf.conf, namespaces);
                }
                File.WriteAllText(conf.path, stringWriter.ToString());
            }
        }

        protected Texture2D inputTex;

        protected virtual void OnEnable()
        {
            foreach (var module in conf.conf.modules)
            {
                ComponentExtensions.modulesDict[module.name] = module.uuid;
            }
            foreach (var component in conf.conf.modules.SelectMany(m => m.components))
            {
                ComponentExtensions.componentsDict[component.name] = component.uuid;
            }
            foreach (var @interface in conf.conf.modules.SelectMany(m => m.components).SelectMany(c => c.interfaces))
            {
                ComponentExtensions.interfacesDict[@interface.name] = @interface.uuid;
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

        protected static long clock() => DateTimeOffset.Now.Ticks;
    }
}
#pragma warning restore IDE1006 // Styles d'affectation de noms
