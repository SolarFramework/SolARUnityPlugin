using System;
using System.Collections.Generic;
using System.Linq;
using SolAR;
using SolAR.Api.Input.Devices;
using SolAR.Datastructure;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using XPCF.Api;
using XPCF.Core;

public class SolARTest : AbstractSample
{
    //public Configuration conf;
    public string uuid = "5B7396F4-A804-4F3C-A0EB-FB1D56042BB4";
    UUID UUID { get { return new UUID(uuid); } }
    IComponentManager xpcfComponentManager;
    IComponentIntrospect xpcfComponent;
    ICamera iCamera;
    Image image;

    protected void Start()
    {
        image = SharedPtr.Alloc<Image>().AddTo(subscriptions);
    }

    protected override void OnDisable()
    {
        //base.OnDisable();
    }

    protected void OnDestroy()
    {
        base.OnDisable();
    }

    void DictGui(string name, Dictionary<string, string> dictionary)
    {
        using (new GUILayout.HorizontalScope(name, GUI.skin.window))
        {
            var id = GUILayout.SelectionGrid(-1, dictionary.Keys.ToArray(), 6);
            if (id != -1)
            {
                uuid = dictionary.ElementAt(id).Value;
            }
        }
    }

    bool isOpenUUID;
    protected void OnGUI()
    {
        if (isOpenUUID = GUILayout.Toggle(isOpenUUID, "UUID"))
        {
            DictGui("Modules", ComponentExtensions.modulesDict);
            DictGui("Interfaces", ComponentExtensions.interfacesDict);
            DictGui("Components", ComponentExtensions.componentsDict);
        }

        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("getManager"))
            {
                xpcfComponentManager = xpcf_api.getComponentManagerInstance().AddTo(subscriptions);
            }
            GUILayout.Toggle(xpcfComponentManager != null, "OK");
            if (GUILayout.Button("load"))
            {
                var path = conf.path;
                Debug.Log(path);
                Debug.Log(xpcfComponentManager.load(path));
            }
            if (GUILayout.Button("clear"))
            {
                xpcfComponentManager.clear();
            }
        }
        using (new GUILayout.HorizontalScope("Metadata", GUI.skin.window))
        {
            if (GUILayout.Button("getModulesMD"))
            {
                using (var modules = xpcfComponentManager.getModulesMetadata())
                {
                    Debug.Log(modules);
                    Debug.Log(modules.size());
                    using (var e = modules.getEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            using (var m = e.current())
                            {
                                Debug.LogFormat("{0}: {1} : {2}", m.name(), m.getPath(), m.description());
                            }
                        }
                    }
                }
            }
            if (GUILayout.Button("getInterfacesMD"))
            {
                using (var interfaces = xpcfComponentManager.getInterfacesMetadata())
                {
                    Debug.Log(interfaces);
                    Debug.Log(interfaces.size());
                    using (var e = interfaces.getEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            using (var i = e.current())
                            {
                                //Debug.Log(i.getUUID());
                                Debug.LogFormat("{0}: {1}", i.name(), i.description());
                            }
                        }
                    }
                }
            }
            if (GUILayout.Button("findComponentMD(UUID)"))
            {
                using (var x = xpcfComponentManager.findComponentMetadata(UUID)) Debug.Log(x);
            }
            if (GUILayout.Button("findInterfaceMD(UUID)"))
            {
                using (var x = xpcfComponentManager.findInterfaceMetadata(UUID)) Debug.Log(x);
            }
            if (GUILayout.Button("findModuleMD(UUID)"))
            {
                using (var x = xpcfComponentManager.findModuleMetadata(UUID)) Debug.Log(x);
            }
            if (GUILayout.Button("getModuleUUID(UUID)"))
            {
                using (var x = xpcfComponentManager.getModuleUUID(UUID)) Debug.Log(x);
            }
        }
        uuid = GUILayout.TextField(uuid);
        if (GUILayout.Button("createComponent(UUID)"))
        {
            xpcfComponent = xpcfComponentManager.createComponent(UUID).AddTo(subscriptions);
        }
        GUILayout.Toggle(xpcfComponent != null, "OK");
        using (new GUILayout.HorizontalScope("IComponentIntrospect", GUI.skin.window))
        {
            if (GUILayout.Button("getNbInterfaces"))
            {
                Debug.Log(xpcfComponent.getNbInterfaces());
            }
            if (GUILayout.Button("getInterfaces"))
            {
                using (var interfaces = xpcfComponent.getInterfaces())
                {
                    Debug.Log(interfaces.size());
                    using (var e = interfaces.getEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            using (var i = e.current())
                            {
                                Debug.Log(xpcfComponent.getDescription(i));
                            }
                        }
                    }
                }
            }
            if (GUILayout.Button("implements(UUID)"))
            {
                Debug.Log(xpcfComponent.implements(UUID));
            }
            if (GUILayout.Button("getDescription(UUID)"))
            {
                Debug.Log(xpcfComponent.getDescription(UUID));
            }
        }
        using (new GUILayout.HorizontalScope("bindTo", GUI.skin.window))
        {
            if (GUILayout.Button("bindTo<ICamera>"))
            {
                iCamera = xpcfComponent.BindTo<ICamera>().AddTo(subscriptions);
            }
            //if (GUILayout.Button("queryInterface TODO"))
            //{
            //    xpcfComponent = xpcfComponent.queryInterface(UUID);
            //}
        }
        GUILayout.Toggle(iCamera != null, "OK");
        using (new GUILayout.HorizontalScope("ICamera", GUI.skin.window))
        {
            if (GUILayout.Button("start"))
            {
                Debug.Log(iCamera.start());
            }
            if (GUILayout.Button("getDistorsionParameters"))
            {
                using (var x = iCamera.getDistorsionParameters()) { Debug.Log(x); }
            }
            if (GUILayout.Button("getIntrinsicsParameters"))
            {
                using (var x = iCamera.getIntrinsicsParameters()) { Debug.Log(x); }
            }
            if (GUILayout.Button("getResolution"))
            {
                using (var size = iCamera.getResolution())
                {
                    Debug.LogFormat("{0} x {1}", size.width, size.height);
                }
            }
            if (GUILayout.Button("getNextImage"))
            {
                Debug.Log(iCamera.getNextImage(image));
            }
        }
        GUILayout.Toggle(image != null, "OK");
        using (new GUILayout.HorizontalScope("Image", GUI.skin.window))
        {
            if (GUILayout.Button("getWidth")) Debug.Log(image.getWidth());
            if (GUILayout.Button("getHeight")) Debug.Log(image.getHeight());
            if (GUILayout.Button("getNbChannels")) Debug.Log(image.getNbChannels());
            if (GUILayout.Button("getSize")) { using (var size = image.getSize()) Debug.LogFormat("{0} x {1}", size.width, size.height); }
            if (GUILayout.Button("getNbBitsPerComponent")) Debug.Log(image.getNbBitsPerComponent());
            if (GUILayout.Button("getBufferSize")) Debug.Log(image.getBufferSize());
            if (GUILayout.Button("getDataType")) Debug.Log(image.getDataType());
            if (GUILayout.Button("getImageLayout")) Debug.Log(image.getImageLayout());
            if (GUILayout.Button("getPixelOrder")) Debug.Log(image.getPixelOrder());
        }
        using (new GUILayout.HorizontalScope("Texture", GUI.skin.window))
        {
            if (GUILayout.Button("new"))
            {
                if (tex != null) Destroy(tex);
                var w = (int)image.getWidth();
                var h = (int)image.getHeight();
                Assert.AreEqual(3, image.getNbChannels());
                Assert.AreEqual(8, image.getNbBitsPerComponent());
                Assert.AreEqual(Image.DataType.TYPE_8U, image.getDataType());
                Assert.AreEqual(Image.ImageLayout.LAYOUT_BGR, image.getImageLayout());
                Assert.AreEqual(Image.PixelOrder.INTERLEAVED, image.getPixelOrder());
                tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            }
            if (GUILayout.Button("LoadRawTextureData")) tex.LoadRawTextureData(image.data(), (int)image.getBufferSize());
            if (GUILayout.Button("Apply")) tex.Apply();
        }
        if (tex != null) GUILayout.Label(tex);
    }
    Texture2D tex;
}
