using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using PropertyType = SolAR.XpcfRegistry.Configure.Property.TYPE;

namespace SolAR
{
    [CustomEditor(typeof(SolARPipeline), true)]
    public class SolARPipelineEditor : Editor
    {
#pragma warning disable IDE1006 // Styles d'affectation de noms
        new SolARPipeline target => (SolARPipeline)base.target;

        GUIStyle _windowStyle;
        GUIStyle windowStyle => _windowStyle ?? (_windowStyle = new GUIStyle(GUI.skin.window) { richText = true, stretchHeight = false });
#pragma warning restore IDE1006 // Styles d'affectation de noms

        readonly AnimBool animConfiguration = new AnimBool(true);
        SerializedObject pipelineSO;

        protected void OnEnable()
        {
            pipelineSO = new SerializedObject(target);
            LoadPipelines();
            SelectPipeline(target.m_selectedPipeline);
            animConfiguration.valueChanged.AddListener(new UnityAction(Repaint));
        }

        protected void OnDisable()
        {
            animConfiguration.valueChanged.RemoveAllListeners();
        }

        void OnConfGUI(SerializedProperty conf, ref bool modified)
        {
            var properties = conf.FindPropertyRelative("properties");
            OnConfigurationGUI(properties, ref modified);
        }

        void OnConfigurationGUI(SerializedProperty configuration, ref bool modified)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                animConfiguration.target = EditorGUILayout.Foldout(animConfiguration.target, "Configuration");

                using (var scope = new EditorGUILayout.FadeGroupScope(animConfiguration.faded))
                {
                    if (scope.visible)
                    {
                        //using (new EditorGUI.IndentLevelScope())
                        {
                            for (int i = 0; i < configuration.arraySize; ++i)
                            {
                                var componentConf = configuration.GetArrayElementAtIndex(i);
                                OnComponentConfGUI(componentConf, ref modified);
                            }
                        }
                    }
                }
            }
        }

        void OnComponentConfGUI(SerializedProperty componentConf, ref bool modified)
        {
            var component = componentConf.FindPropertyRelative("component");
            var name = componentConf.FindPropertyRelative("name");

            /*
            var description = componentConf.FindPropertyRelative("description");
            if (string.IsNullOrEmpty(uuid?.stringValue))
            {
                var componentVal = component.stringValue;
                var componentRef = target.conf.modules
                    .SelectMany(m => m.components)
                    .FirstOrDefault(c => c.name == componentVal);
                uuid.stringValue = componentRef?.uuid ?? "<color=red><b>Component not declared !</b></color>";
                description.stringValue = componentRef?.description;
            }
            */

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
                OnPropertiesGUI(properties, ref modified);
            }
        }

        void OnPropertiesGUI(SerializedProperty properties, ref bool modified)
        {
            //using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < properties.arraySize; ++i)
                {
                    var property = properties.GetArrayElementAtIndex(i);
                    OnPropertyGUI(property, ref modified);
                }
            }
        }

        void OnPropertyGUI(SerializedProperty property, ref bool modified)
        {
            var name = property.FindPropertyRelative("name");
            var type = property.FindPropertyRelative("type");
            var value = property.FindPropertyRelative("value");
            var values = property.FindPropertyRelative("values");
            //var access = property.FindPropertyRelative("access");

            var propType = (PropertyType)type.enumValueIndex;
            //var typeName = propType.ToString();
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
                        var v = EditorGUILayout.DelayedTextField(content, value.stringValue);
                        if (scope.changed)
                        {
                            value.stringValue = v;
                            modified = true;
                        }
                    }
                    break;
                case PropertyType.structure:
                    EditorGUILayout.PrefixLabel(content);
                    var propertie = property.FindPropertyRelative("properties");
                    OnPropertiesGUI(propertie, ref modified);
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
                                modified = true;
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
                                v = EditorGUILayout.DelayedFloatField(content, v);
                                if (scope.changed)
                                {
                                    value.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                    modified = true;
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
                                    modified = true;
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
                                    modified = true;
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
                                    modified = true;
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
                                    v = EditorGUILayout.DelayedFloatField(content, v);
                                    if (scope.changed)
                                    {
                                        sp.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                        modified = true;
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
                                v = EditorGUILayout.DelayedIntField(content, v);
                                if (scope.changed)
                                {
                                    value.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                    modified = true;
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
                                    modified = true;
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
                                    modified = true;
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
                                    int.TryParse(sp.stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
                                    v = EditorGUILayout.DelayedIntField(content, v);
                                    if (scope.changed)
                                    {
                                        sp.stringValue = v.ToString(CultureInfo.InvariantCulture);
                                        modified = true;
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        void SelectPipeline(int num_pipeline)
        {
            //pipelineSO.Update();
            pipelineSO.FindProperty("m_selectedPipeline").intValue = num_pipeline;

            var pipelinesPath = pipelineSO.FindProperty("m_pipelinesPath");
            if (pipelinesPath.arraySize <= 0)
                return;
           
            if (num_pipeline >= pipelinesPath.arraySize)
                num_pipeline = pipelinesPath.arraySize - 1;

            string selectedPipelinePath = Application.dataPath + pipelinesPath.GetArrayElementAtIndex(num_pipeline).stringValue;
          
            using (var stream = File.OpenText(selectedPipelinePath))
            {
                var serializer = new XmlSerializer(typeof(XpcfRegistry));
                pipelineSO.ApplyModifiedProperties();
                target.conf = (XpcfRegistry)serializer.Deserialize(stream);
                pipelineSO.Update();
                pipelineSO.FindProperty("m_configurationPath").stringValue = pipelinesPath.GetArrayElementAtIndex(num_pipeline).stringValue;
            }
            SaveConfig();
            //pipelineSO.ApplyModifiedProperties();
        }

        void LoadPipelines()
        {
            pipelineSO.Update();
            string[] files = Directory.GetFiles(Application.dataPath + pipelineSO.FindProperty("m_pipelineFolder").stringValue, "*.xml");
            var namesList = new List<string>();
            //var uuidList = new List<string>();
            var pathList = new List<string>();

            var pipelinesNameProperty = pipelineSO.FindProperty("m_pipelinesName");
            pipelinesNameProperty.ClearArray();
            var pipelinesPathProperty = pipelineSO.FindProperty("m_pipelinesPath");
            pipelinesPathProperty.ClearArray();

            foreach (var file in files)
            {
                int index = file.IndexOf(Application.dataPath);
                if (index != 0) continue;

                string file_temp = file.Replace("\\", "/");

                XElement root = null;
                try
                {
                    root = XElement.Load(file_temp);
                }
                catch (System.Xml.XmlException e)
                {
                    Debug.LogFormat(this, "Configuration file {0} has an error:{1}", file_temp, e.Message);
                    continue;
                }
                //if (root == null) continue;

                foreach (XElement component_interface in root.Descendants("interface"))
                {
                    if (component_interface.Attributes("name").First().Value == "IPoseEstimationPipeline")
                    {
                        XElement component = component_interface.Ancestors("component").First();
                        string pipelineName = component.Attribute("name").Value;
                        if (!string.IsNullOrEmpty(pipelineName))
                        {
                            pipelinesNameProperty.InsertArrayElementAtIndex(pipelinesNameProperty.arraySize);
                            pipelinesNameProperty.GetArrayElementAtIndex(pipelinesNameProperty.arraySize-1).stringValue = pipelineName;
                            //namesList.Add(pipelineName);
                            //string pipelineUuid = component.Attribute("uuid").Value;
                            //uuidList.Add(pipelineUuid);
                            string relative_file_temp = file_temp.Substring(Application.dataPath.Length);
                            pipelinesPathProperty.InsertArrayElementAtIndex(pipelinesPathProperty.arraySize);
                            pipelinesPathProperty.GetArrayElementAtIndex(pipelinesPathProperty.arraySize-1).stringValue = relative_file_temp;

                            //uuidList.Add(pipelineUuid);
                            //pathList.Add(relative_file_temp);
                        }
                    }
                }
            }
            //target.m_pipelinesName = namesList.ToArray();
            //target.m_pipelinesPath = pathList.ToArray();
            //target.m_pipelinesUUID = uuidList.ToArray();

            if (pipelinesNameProperty.arraySize == 0)
            {
                pipelineSO.FindProperty("m_selectedPipeline").intValue = -1;
            }
            else if (pipelineSO.FindProperty("m_selectedPipeline").intValue >= pipelinesNameProperty.arraySize || pipelineSO.FindProperty("m_selectedPipeline").intValue == -1)
            {
                pipelineSO.FindProperty("m_selectedPipeline").intValue = -1;
            }
            pipelineSO.ApplyModifiedProperties();
        }

        void SaveConfig()
        {
            pipelineSO.ApplyModifiedProperties();
            target.conf.autoAlias = true;

            using (var stringWriter = File.CreateText(Application.dataPath + pipelineSO.FindProperty("m_configurationPath").stringValue))
            {
                var serializer = new XmlSerializer(typeof(XpcfRegistry));
                serializer.Serialize(stringWriter, target.conf);
            }
            pipelineSO.Update();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            pipelineSO.Update();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            WebcamGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            CanvasGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            using (new GUILayout.VerticalScope("<b>Pipelines</b>", windowStyle, GUILayout.ExpandHeight(false)))
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Select Pipelines Folder"))
                    {
                        while (true)
                        {
                            string folder = EditorUtility.OpenFolderPanel("Select a new pipelines folder", pipelineSO.FindProperty("m_pipelineFolder").stringValue, "");
                            if (string.IsNullOrEmpty(folder)) break;
                            int indexApplicationDataPath = folder.IndexOf(Application.dataPath);
                            int indexStreamingAssetsPath = folder.IndexOf(Application.streamingAssetsPath);
                            if (indexApplicationDataPath == 0 && indexStreamingAssetsPath == -1)
                            {
                                pipelineSO.FindProperty("m_pipelineFolder").stringValue = folder.Substring(Application.dataPath.Length);
                                LoadPipelines();
                                break;
                            }

                            EditorUtility.DisplayDialog("Pipelines folder selection error", "The folder for your pipelines must be under the Asset folder of your Unity project, but not in the StreamingAssets directory.", "OK");
                        }
                    }
                }

                if ((pipelineSO.FindProperty("m_pipelinesName").arraySize == 0) || (pipelineSO.FindProperty("m_selectedPipeline").intValue < 0))
                {
                    pipelineSO.FindProperty("m_configurationPath").stringValue = "";
                    target.conf = null;
                    pipelineSO.ApplyModifiedProperties();
                    return;
                }

                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    var style = new GUIStyle(EditorStyles.popup)
                    {
                        fontSize = 12,
                        fixedHeight = 15.0f,
                    };

                    pipelineSO.FindProperty("m_selectedPipeline").intValue = EditorGUILayout.Popup(pipelineSO.FindProperty("m_selectedPipeline").intValue, target.m_pipelinesName, style);

                    if (scope.changed)
                    {
                        SelectPipeline(pipelineSO.FindProperty("m_selectedPipeline").intValue);
                    }
                }

                var conf = pipelineSO.FindProperty("conf");

                bool modified = false;
                if (conf != null)
                    OnConfGUI(conf, ref modified);

                pipelineSO.ApplyModifiedProperties();
                if (modified) { SaveConfig(); }
            }
            pipelineSO.ApplyModifiedProperties();
        }

        void WebcamGUI()
        {
            //pipelineSO.Update();
            bool isUnityWebCam = EditorGUILayout.Toggle("Use Unity Webcam", pipelineSO.FindProperty("isUnityWebcam").boolValue);
            pipelineSO.FindProperty("isUnityWebcam").boolValue = isUnityWebCam;
            //target.isUnityWebcam = EditorGUILayout.Toggle("Use Unity Webcam", target.isUnityWebcam);
            if (isUnityWebCam)
            //if (target.isUnityWebcam)
            {
                var webCams = WebCamTexture.devices;
                var webCamNames = Array.ConvertAll(webCams, webCam =>
                {
                    string webCamName = webCam.name;
                    if (webCam.isFrontFacing)
                        webCamName += " (front)";
                    return webCamName;
                });
                var label = new GUIContent("Video Camera");
                pipelineSO.FindProperty("webcamIndex").intValue = EditorGUILayout.Popup(label, target.webcamIndex, webCamNames);
                //target.webcamIndex = EditorGUILayout.Popup(label, target.webcamIndex, webCamNames);

                pipelineSO.FindProperty("focalX").floatValue = EditorGUILayout.DelayedFloatField("focalX ", target.focalX);
                pipelineSO.FindProperty("focalY").floatValue = EditorGUILayout.DelayedFloatField("focalY ", target.focalY);
                pipelineSO.FindProperty("centerX").floatValue = EditorGUILayout.DelayedFloatField("centerX ", target.centerX);
                pipelineSO.FindProperty("centerY").floatValue = EditorGUILayout.DelayedFloatField("centerY ", target.centerY);
                pipelineSO.FindProperty("width").intValue = EditorGUILayout.DelayedIntField("width ", target.width);
                pipelineSO.FindProperty("height").intValue = EditorGUILayout.DelayedIntField("height ", target.height);

                /*
                target.focalX = EditorGUILayout.FloatField("focalX ", target.focalX);
                target.focalY = EditorGUILayout.FloatField("focalY ", target.focalY);
                target.centerX = EditorGUILayout.FloatField("centerX ", target.centerX);
                target.centerY = EditorGUILayout.FloatField("centerY ", target.centerY);
                target.width = EditorGUILayout.IntField("width ", target.width);
                target.height = EditorGUILayout.IntField("height ", target.height);
                */
            }
            //pipelineSO.ApplyModifiedProperties();
        }

        void CanvasGUI()
        {
            bool hasCustomCanvas = EditorGUILayout.Toggle("Custom_Canvas", pipelineSO.FindProperty("hasCustomCanvas").boolValue);
            pipelineSO.FindProperty("hasCustomCanvas").boolValue = hasCustomCanvas;
            if (target.hasCustomCanvas)
            {
                pipelineSO.ApplyModifiedProperties();
                target.canvas = (Canvas)EditorGUILayout.ObjectField("Static Canvas UI", target.canvas, typeof(Canvas), true);
                target.material = (Material)EditorGUILayout.ObjectField("Static Canvas Material", target.material, typeof(Material), true);
                pipelineSO.Update();
            }           
        }
    }
}
