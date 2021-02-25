using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XPCF.Api;
using XPCF.Core;

namespace SolAR.Expert
{
    public partial class RuntimeEditor : MonoBehaviour
    {
        static UUID configurableUUID;

        public IComponentManager xpcfManager { get { return xpcf_api.getComponentManagerInstance(); } }

        public PipelineManager pipelineManager;

        readonly List<IComponentIntrospect> xpcfComponents = new List<IComponentIntrospect>();
        GUIContent[] guiComponents;
        int idComponent = -1;
        IComponentIntrospect xpcfComponent;

        IConfigurable xpcfConfigurable;

        protected void Awake()
        {
            configurableUUID = configurableUUID ?? new UUID("98DBA14F-6EF9-462E-A387-34756B4CBA80");
        }

        bool isOpen;

        protected void OnGUI()
        {
            using (GUIScope.ChangeCheck)
            {
                var command = isOpen ? "Close" : "Configure";
                isOpen = GUILayout.Toggle(isOpen, command, GUI.skin.button);
                if(GUI.changed)
                {
                    if (isOpen)
                    {
                        xpcfComponents.AddRange(pipelineManager.xpcfComponents);
                        guiComponents = xpcfComponents.Select(c => new GUIContent(c.GetType().Name)).ToArray();
                    }
                    else
                    {
                        xpcfComponents.Clear();
                        guiComponents = null;
                    }
                }
            }
            if (!isOpen) return;

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
                    GUILayout.Label("Select an IConfigurable component");
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
}
