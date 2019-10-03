using System.Globalization;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace SolAR
{
    [CustomEditor(typeof(AbstractSample), true)]
    public class AbstractSampleEditor : Editor
    {
#pragma warning disable IDE1006 // Styles d'affectation de noms
        new AbstractSample target { get { return (AbstractSample)base.target; } }
#pragma warning restore IDE1006 // Styles d'affectation de noms

        GUIStyle _windowStyle;
        GUIStyle windowStyle { get { return _windowStyle ?? (_windowStyle = new GUIStyle(GUI.skin.window) { richText = true, stretchHeight = false }); } }

        readonly AnimBool animModules = new AnimBool();
        readonly AnimBool animConfiguration = new AnimBool();

        protected void OnEnable()
        {
            animModules.valueChanged.AddListener(new UnityAction(Repaint));
            animConfiguration.valueChanged.AddListener(new UnityAction(Repaint));
        }

        protected void OnDisable()
        {
            animModules.valueChanged.RemoveAllListeners();
            animConfiguration.valueChanged.RemoveAllListeners();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Don't make child fields be indented
            //var indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;

            //EditorGUI.PropertyField(position, property, new GUIContent("Conf"), true);

            var path = serializedObject.FindProperty("conf.path");
            EditorGUILayout.PropertyField(path);
            
            var conf = serializedObject.FindProperty("conf.conf");
            OnConfGUI(conf);

            //ConfDrawer.ArrayGUI(modules, ref position);


            // Set indent back to what it was
            //EditorGUI.indentLevel = indent;

            serializedObject.ApplyModifiedProperties();
        }

        void OnConfGUI(SerializedProperty conf)
        {
            var modules = conf.FindPropertyRelative("modules");
            var configuration = conf.FindPropertyRelative("properties");

            OnModulesGUI(modules);
            OnConfigurationGUI(configuration);
        }

        void OnModulesGUI(SerializedProperty modules)
        {
            animModules.target = EditorGUILayout.Foldout(animModules.target, "Modules/Components");

            using (var scope = new EditorGUILayout.FadeGroupScope(animModules.faded))
            {
                if (scope.visible)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (int i = 0; i < modules.arraySize; ++i)
                        {
                            var module = modules.GetArrayElementAtIndex(i);
                            //EditorGUILayout.PropertyField(module);
                            OnModuleGUI(module);
                        }
                    }
                }
            }
        }

        void OnModuleGUI(SerializedProperty module)
        {
            //var uuid = module.FindPropertyRelative("uuid");
            var name = module.FindPropertyRelative("name");
            
            //var path = module.FindPropertyRelative("path");
            var description = module.FindPropertyRelative("description");

            //var tooltip = string.Format("{0}", description.stringValue, path.stringValue);
            var content = new GUIContent(name.stringValue, description.stringValue);
            GUILayout.Box(content);

            var components = module.FindPropertyRelative("components");
            OnComponentsGUI(components);
        }

        void OnComponentsGUI(SerializedProperty components)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < components.arraySize; ++i)
                {
                    var component = components.GetArrayElementAtIndex(i);
                    //EditorGUILayout.PropertyField(component);
                    OnComponentGUI(component);
                }
            }
        }

        void OnComponentGUI(SerializedProperty component)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                //var uuid = component.FindPropertyRelative("uuid");
                var name = component.FindPropertyRelative("name");
                var description = component.FindPropertyRelative("description");

                //var tooltip = string.Format("{0}: {1}", uuid.stringValue, description.stringValue);
                var content = new GUIContent(name.stringValue, description.stringValue);
                GUILayout.Box(content, GUILayout.ExpandWidth(true));

                var interfaces = component.FindPropertyRelative("interfaces");
                OnInterfacesGUI(interfaces);
            }
        }

        void OnInterfacesGUI(SerializedProperty interfaces)
        {
            var interfacesTooltip = Enumerable
                .Range(0, interfaces.arraySize)
                .Select(interfaces.GetArrayElementAtIndex)
                .Aggregate(new StringBuilder(), (s, i) =>
                {
                    //var uuid = i.FindPropertyRelative("uuid");
                    var name = i.FindPropertyRelative("name");
                    var description = i.FindPropertyRelative("description");
                    s.AppendFormat("+{0}: {1}\n", name.stringValue, description.stringValue);
                    return s;
                },
                s => s.ToString());

            var interfacesContent = new GUIContent(interfaces.arraySize + " I", interfacesTooltip);
            GUILayout.Box(interfacesContent);
        }

        void OnConfigurationGUI(SerializedProperty configuration)
        {
            animConfiguration.target = EditorGUILayout.Foldout(animConfiguration.target, "Configuration");

            using (var scope = new EditorGUILayout.FadeGroupScope(animConfiguration.faded))
            {
                if (scope.visible)
                {
                    var componentsConf = configuration.FindPropertyRelative("configure");
                    OnComponentsConfGUI(componentsConf);
                }
            }
        }

        void OnComponentsConfGUI(SerializedProperty componentsConf)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < componentsConf.arraySize; ++i)
                {
                    var componentConf = componentsConf.GetArrayElementAtIndex(i);
                    //EditorGUILayout.PropertyField(component);
                    OnComponentConfGUI(componentConf);
                }
            }
        }

        void OnComponentConfGUI(SerializedProperty componentConf)
        {
            var name = componentConf.FindPropertyRelative("component");
            var properties = componentConf.FindPropertyRelative("properties");

            var type = componentConf.FindPropertyRelative("type");
            if (string.IsNullOrEmpty(type.stringValue))
            {
                var component = target.conf.conf.modules
                    .SelectMany(m => m.components)
                    .FirstOrDefault(c => c.name == name.stringValue);
                type.stringValue = component?.name ?? "<color=red><b>???</b></color>";
            }

            var label = string.Format("<b>{0}</b>", name.stringValue);
            //var tooltip = string.Format("{0}: {1}", uuid.stringValue, description.stringValue);
            var content = new GUIContent(label);
            using (new GUILayout.VerticalScope(content, windowStyle, GUILayout.ExpandHeight(false)))
            {
                OnPropertiesGUI(properties);
            }
        }

        void OnPropertiesGUI(SerializedProperty properties)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < properties.arraySize; ++i)
                {
                    var property = properties.GetArrayElementAtIndex(i);
                    //EditorGUILayout.PropertyField(component);
                    OnPropertyGUI(property);
                }
            }
        }

        void OnPropertyGUI(SerializedProperty property)
        {
            var name = property.FindPropertyRelative("name");
            var type = property.FindPropertyRelative("type");
            var value = property.FindPropertyRelative("value");
            var values = property.FindPropertyRelative("values");

            //var tooltip = string.Format("{0}: {1}", uuid.stringValue, description.stringValue);
            var content = new GUIContent(name.stringValue, type.stringValue);

            switch (type.stringValue.ToLowerInvariant())
            {
                default:
                    EditorGUILayout.HelpBox(type.stringValue, MessageType.Error);
                    value.stringValue = EditorGUILayout.TextField(content, value.stringValue);
                    break;
                case "string":
                    value.stringValue = EditorGUILayout.TextField(content, value.stringValue);
                    break;
                case "double":
                    {
                        var enumerable = Enumerable
                            .Range(0, values.arraySize)
                            .Select(values.GetArrayElementAtIndex)
                            .DefaultIfEmpty(value);
                        foreach (var v in enumerable)
                        {
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                double f;
                                double.TryParse(v.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out f);
                                f = EditorGUILayout.DelayedDoubleField(content, f);
                                if (scope.changed)
                                {
                                    v.stringValue = f.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                        }
                    }
                    break;
                case "float":
                    switch (values.arraySize)
                    {
                        case 0:
                        case 1:
                            if (values.arraySize == 1) value = values.GetArrayElementAtIndex(0);
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                float f;
                                float.TryParse(value.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out f);
                                f = EditorGUILayout.FloatField(content, f);
                                if (scope.changed)
                                {
                                    value.stringValue = f.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                            break;
                        case 2:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var v = new Vector2();
                                for (int i = 0; i < 2; ++i)
                                {
                                    float f;
                                    float.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out f);
                                    v[i] = f;
                                }
                                v = EditorGUILayout.Vector2Field(content, v);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 2; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = v[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        case 3:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var v = new Vector3();
                                for (int i = 0; i < 3; ++i)
                                {
                                    float f;
                                    float.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out f);
                                    v[i] = f;
                                }
                                v = EditorGUILayout.Vector3Field(content, v);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 3; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = v[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        default:
                            var enumerable = Enumerable
                                .Range(0, values.arraySize)
                                .Select(values.GetArrayElementAtIndex)
                                .DefaultIfEmpty(value);
                            foreach (var v in enumerable)
                            {
                                using (var scope = new EditorGUI.ChangeCheckScope())
                                {
                                    float f;
                                    float.TryParse(v.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out f);
                                    f = EditorGUILayout.FloatField(content, f);
                                    if (scope.changed)
                                    {
                                        v.stringValue = f.ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case "integer":
                case "unsignedinteger":
                    switch (values.arraySize)
                    {
                        case 0:
                        case 1:
                            if (values.arraySize == 1) value = values.GetArrayElementAtIndex(0);
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                int f;
                                int.TryParse(value.stringValue, out f);
                                f = EditorGUILayout.IntField(content, f);
                                if (scope.changed)
                                {
                                    value.stringValue = f.ToString();
                                }
                            }
                            break;
                        case 2:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var v = new Vector2Int();
                                for (int i = 0; i < 2; ++i)
                                {
                                    int f;
                                    int.TryParse(values.GetArrayElementAtIndex(i).stringValue, out f);
                                    v[i] = f;
                                }
                                v = EditorGUILayout.Vector2IntField(content, v);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 2; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = v[i].ToString();
                                    }
                                }
                            }
                            break;
                        case 3:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var v = new Vector3Int();
                                for (int i = 0; i < 3; ++i)
                                {
                                    int f;
                                    int.TryParse(values.GetArrayElementAtIndex(i).stringValue, out f);
                                    v[i] = f;
                                }
                                if (content.text.ToLowerInvariant().Contains("color"))
                                {
                                    var c = new Color32((byte)v.x, (byte)v.y, (byte)v.z, 0xFF);
                                    c = EditorGUILayout.ColorField(content, c);
                                    v = new Vector3Int(c.r, c.g, c.b);
                                }
                                else
                                {
                                    v = EditorGUILayout.Vector3IntField(content, v);
                                }
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 3; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = v[i].ToString();
                                    }
                                }
                            }
                            break;
                        default:
                            Debug.LogError(values.arraySize);
                            var enumerable = Enumerable
                                .Range(0, values.arraySize)
                                .Select(values.GetArrayElementAtIndex)
                                .DefaultIfEmpty(value);
                            foreach (var v in enumerable)
                            {
                                using (var scope = new EditorGUI.ChangeCheckScope())
                                {
                                    int f;
                                    int.TryParse(v.stringValue, out f);
                                    f = EditorGUILayout.IntField(content, f);
                                    if (scope.changed)
                                    {
                                        v.stringValue = f.ToString();
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
