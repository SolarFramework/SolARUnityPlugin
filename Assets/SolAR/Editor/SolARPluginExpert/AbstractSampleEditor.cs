using System.Globalization;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using PropertyType = SolAR.XpcfRegistry.Configure.Property.TYPE;

namespace SolAR
{
    [CustomEditor(typeof(AbstractSample), true)]
    public class AbstractSampleEditor : Editor
    {
#pragma warning disable IDE1006 // Styles d'affectation de noms
        //new AbstractSample target => (AbstractSample)base.target;

        GUIStyle windowStyle => _windowStyle ?? (_windowStyle = new GUIStyle(GUI.skin.window) { richText = true, stretchHeight = false });
        GUIStyle _windowStyle;
#pragma warning restore IDE1006 // Styles d'affectation de noms

        readonly AnimBool animModules = new AnimBool();
        readonly AnimBool animConfiguration = new AnimBool(true);

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

            var path = serializedObject.FindProperty("conf.path");
            EditorGUILayout.PropertyField(path);

            var conf = serializedObject.FindProperty("conf.conf");
            OnConfGUI(conf);

            serializedObject.ApplyModifiedProperties();
        }

        void OnConfGUI(SerializedProperty conf)
        {
            var modules = conf.FindPropertyRelative("modules");
            OnModulesGUI(modules);

            var properties = conf.FindPropertyRelative("properties");
            OnConfigurationGUI(properties);
        }

        void OnConfigurationGUI(SerializedProperty configuration)
        {
            animConfiguration.target = EditorGUILayout.Foldout(animConfiguration.target, "Configuration");

            using (var scope = new EditorGUILayout.FadeGroupScope(animConfiguration.faded))
            {
                if (scope.visible)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (int i = 0; i < configuration.arraySize; ++i)
                        {
                            var componentConf = configuration.GetArrayElementAtIndex(i);
                            OnComponentConfGUI(componentConf);
                        }
                    }
                }
            }
        }

        void OnComponentConfGUI(SerializedProperty componentConf)
        {
            var component = componentConf.FindPropertyRelative("component");
            var name = componentConf.FindPropertyRelative("name");

            var label = string.Format("<b>{0}</b>", component.stringValue);
            if (!string.IsNullOrEmpty(name.stringValue))
                label = string.Format("{0} ({1})", label, name.stringValue);
            //var tooltip = string.Format("{0}: {1}", uuid.stringValue, description.stringValue);
            //var content = new GUIContent(label, tooltip);
            using (new GUILayout.VerticalScope(label, windowStyle, GUILayout.ExpandHeight(false)))
            {
                //var tooltip = string.Format("component: {0}\nuuid: {1}\nname: {2}", component.stringValue, uuid.stringValue, name.stringValue);
                //var content = new GUIContent(description.stringValue, tooltip);
                //EditorGUILayout.HelpBox(content);
                var properties = componentConf.FindPropertyRelative("properties");
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
            //var access = property.FindPropertyRelative("access");

            var propType = (PropertyType)type.enumValueIndex;
            var tooltip = propType.ToString();
            var content = new GUIContent(name.stringValue, tooltip);

            switch (propType)
            {
                default:
                /*
                EditorGUILayout.HelpBox(propType.ToString(), MessageType.Error);
                value.stringValue = EditorGUILayout.TextField(content, value.stringValue);
                break;
                */
                case PropertyType.@string:
                case PropertyType.wstring:
                    using (var scope = new EditorGUI.ChangeCheckScope())
                    {
                        var v = EditorGUILayout.TextField(content, value.stringValue);
                        if (scope.changed)
                        {
                            value.stringValue = v;
                        }
                    }
                    break;
                case PropertyType.structure:
                    EditorGUILayout.PrefixLabel(content);
                    var propertie = property.FindPropertyRelative("properties");
                    OnPropertiesGUI(propertie);
                    break;
                case PropertyType.@double:
                /*
                {
                    var sps = Enumerable
                        .Range(0, values.arraySize)
                        .Select(values.GetArrayElementAtIndex)
                        .DefaultIfEmpty(value);
                    foreach (var sp in sps)
                    {
                        using (var scope = new EditorGUI.ChangeCheckScope())
                        {
                            double.TryParse(sp.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                            v = EditorGUILayout.DelayedDoubleField(content, v);
                            if (scope.changed)
                            {
                                sp.stringValue = v.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
                break;
                */
                case PropertyType.@float:
                    switch (values.arraySize)
                    {
                        case 0:
                        case 1:
                            if (values.arraySize == 1) value = values.GetArrayElementAtIndex(0);
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                float.TryParse(value.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                v = EditorGUILayout.FloatField(content, v);
                                if (scope.changed)
                                {
                                    value.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                            break;
                        case 2:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var vec = new Vector2();
                                for (int i = 0; i < 2; ++i)
                                {
                                    float.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    vec[i] = v;
                                }
                                vec = EditorGUILayout.Vector2Field(content, vec);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 2; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = vec[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        case 3:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var vec = new Vector3();
                                for (int i = 0; i < 3; ++i)
                                {
                                    float.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    vec[i] = v;
                                }
                                vec = EditorGUILayout.Vector3Field(content, vec);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 3; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = vec[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        case 4:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var vec = new Vector4();
                                for (int i = 0; i < 4; ++i)
                                {
                                    float.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    vec[i] = v;
                                }
                                vec = EditorGUILayout.Vector4Field(content, vec);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 4; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = vec[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        default:
                            var sps = Enumerable
                                .Range(0, values.arraySize)
                                .Select(values.GetArrayElementAtIndex)
                                .DefaultIfEmpty(value);
                            foreach (var sp in sps)
                            {
                                using (var scope = new EditorGUI.ChangeCheckScope())
                                {
                                    float.TryParse(sp.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    v = EditorGUILayout.FloatField(content, v);
                                    if (scope.changed)
                                    {
                                        sp.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case PropertyType.@int:
                case PropertyType.@uint:
                case PropertyType.@long:
                case PropertyType.@ulong:
                    switch (values.arraySize)
                    {
                        case 0:
                        case 1:
                            if (values.arraySize == 1) value = values.GetArrayElementAtIndex(0);
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                int.TryParse(value.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out int v);
                                v = EditorGUILayout.IntField(content, v);
                                if (scope.changed)
                                {
                                    value.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                            break;
                        case 2:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var vec = new Vector2Int();
                                for (int i = 0; i < 2; ++i)
                                {
                                    int.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out int v);
                                    vec[i] = v;
                                }
                                vec = EditorGUILayout.Vector2IntField(content, vec);
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 2; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = vec[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        case 3:
                            using (var scope = new EditorGUI.ChangeCheckScope())
                            {
                                var vec = new Vector3Int();
                                for (int i = 0; i < 3; ++i)
                                {
                                    int.TryParse(values.GetArrayElementAtIndex(i).stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out int v);
                                    vec[i] = v;
                                }
                                if (content.text.ToLowerInvariant().Contains("color"))
                                {
                                    var c = new Color32((byte)vec.x, (byte)vec.y, (byte)vec.z, 0xFF);
                                    c = EditorGUILayout.ColorField(content, c);
                                    vec = new Vector3Int(c.r, c.g, c.b);
                                }
                                else
                                {
                                    vec = EditorGUILayout.Vector3IntField(content, vec);
                                }
                                if (scope.changed)
                                {
                                    for (int i = 0; i < 3; ++i)
                                    {
                                        values.GetArrayElementAtIndex(i).stringValue = vec[i].ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                        default:
                            Debug.LogError(values.arraySize);
                            var sps = Enumerable
                                .Range(0, values.arraySize)
                                .Select(values.GetArrayElementAtIndex)
                                .DefaultIfEmpty(value);
                            foreach (var sp in sps)
                            {
                                using (var scope = new EditorGUI.ChangeCheckScope())
                                {
                                    int.TryParse(sp.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    v = EditorGUILayout.IntField(content, v);
                                    if (scope.changed)
                                    {
                                        sp.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
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
                            OnModuleGUI(module);
                        }
                    }
                }
            }
        }

        void OnModuleGUI(SerializedProperty module)
        {
            var name = module.FindPropertyRelative("name");
            //var uuid = module.FindPropertyRelative("uuid");
            //var path = module.FindPropertyRelative("path");
            //var description = module.FindPropertyRelative("description");

            var label = string.Format("<b>{0}</b>", name.stringValue);
            //var tooltip = string.Format("{0}\nuuid: {1}\npath: {2}", description.stringValue, uuid.stringValue, path.stringValue);
            var content = new GUIContent(label);
            //GUILayout.Box(content);
            using (new GUILayout.VerticalScope(content, windowStyle, GUILayout.ExpandHeight(false)))
            {
                var components = module.FindPropertyRelative("components");
                OnComponentsGUI(components);
            }
        }

        void OnComponentsGUI(SerializedProperty components)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < components.arraySize; ++i)
                {
                    var component = components.GetArrayElementAtIndex(i);
                    OnComponentGUI(component);
                }
            }
        }

        void OnComponentGUI(SerializedProperty component)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var name = component.FindPropertyRelative("name");
                var uuid = component.FindPropertyRelative("uuid");
                var description = component.FindPropertyRelative("description");

                var label = name.stringValue;
                var tooltip = string.Format("{0}: {1}", uuid.stringValue, description.stringValue);
                var content = new GUIContent(label, tooltip);
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
                .Aggregate(new StringBuilder(), (sb, i) =>
                {
                    var name = i.FindPropertyRelative("name");
                    //var uuid = i.FindPropertyRelative("uuid");
                    var description = i.FindPropertyRelative("description");
                    return sb.AppendFormat("+{0}: {1}\n", name.stringValue, description.stringValue);
                },
                sb => sb.ToString());

            var interfacesContent = new GUIContent(interfaces.arraySize + " I", interfacesTooltip);
            GUILayout.Box(interfacesContent);
        }
    }
}
