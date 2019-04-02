using System.Collections.Generic;
using System.Linq;
using SolAR;
using UnityEngine;
using XPCF.Api;
using XPCF.Core;

public partial class RuntimeEditor : MonoBehaviour
{
    static UUID configurableUUID;

    public IComponentManager xpcfManager { get { return xpcf_api.getComponentManagerInstance(); } }

    public PipelineManager pipelineManager;

    readonly List<IComponentIntrospect> xpcfComponents = new List<IComponentIntrospect>();
    GUIContent[] guiComponents;
    int idComponent = -1;
    IComponentIntrospect xpcfComponent;

    //UUID[] xpcfInterfaces;
    //GUIContent[] guiInterfaces;
    //int idInterface = -1;

    IConfigurable xpcfConfigurable;

    protected void Awake()
    {
        configurableUUID = configurableUUID ?? new UUID("98DBA14F-6EF9-462E-A387-34756B4CBA80");
    }

    protected void OnEnable()
    {
        xpcfComponents.AddRange(pipelineManager.xpcfComponents);
        guiComponents = xpcfComponents.Select(c => new GUIContent(c.GetType().Name)).ToArray();
    }

    protected void OnDisable()
    {
        xpcfComponents.Clear();
    }

    protected void OnGUI()
    {
        using (new GUILayout.HorizontalScope(GUI.skin.box, GUILayout.ExpandWidth(true)))
        {
            if (guiComponents != null)
            {
                using (GUIScope.ChangeCheck)
                {
                    idComponent = GUILayout.SelectionGrid(idComponent, guiComponents, 1, GUILayout.Width(200));
                    if (GUI.changed)
                    {
                        xpcfComponent = xpcfComponents[idComponent];

                        /*
                        foreach (var uuid in xpcfComponent.getInterfaces())
                        {
                            Debug.Log(xpcfComponent.getDescription(uuid));
                            //var metadata = xpcfManager.findInterfaceMetadata(uuid);
                            //Debug.Log(metadata.name());
                            //Debug.Log(metadata.description());
                        }
                        */
                        /*
                        xpcfInterfaces = xpcfComponent.getInterfaces().ToArray();
                        guiInterfaces = xpcfInterfaces
                            .Select(xpcfComponent.getDescription)
                            .Select(s => new GUIContent("", s))
                            .ToArray();
                            */

                        xpcfConfigurable = xpcfComponent.implements(configurableUUID) ? xpcfComponent.BindTo<IConfigurable>() : null;
                    }
                }
            }
            /*
            if (guiInterfaces != null)
            {
                using (GUITools.ChangeCheckScope)
                {
                    idInterface = GUILayout.SelectionGrid(idInterface, guiInterfaces, 1);
                    if (GUI.changed)
                    {
                        var uuid = xpcfInterfaces[idInterface];

                        //var xpcfInstance = xpcfComponent.bindTo("");
                        guiInterfaces = xpcfComponent.getInterfaces()
                            .Select(xpcfComponent.getDescription)
                            .Select(s => new GUIContent("", s))
                            .ToArray();
                    }
                }
            }
            */
            if (xpcfConfigurable == null)
            {
                GUILayout.Label("This component is not IConfigurable");
            }
            else
            {
                using (new GUILayout.VerticalScope(GUI.skin.box))
                {
                    foreach (var p in xpcfConfigurable.getProperties())
                    {
                        var access = p.getAccessSpecifier();
                        var type = p.getType();
                        object value = access.CanRead() ? p.Get() : type.Default();

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(p.getName(), GUILayout.Width(200));
                            using (GUIScope.ChangeCheck)
                            {
                                value = type.OnGUI(value);
                                if (access.CanWrite() && GUI.changed)
                                {
                                    p.Set(value);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
