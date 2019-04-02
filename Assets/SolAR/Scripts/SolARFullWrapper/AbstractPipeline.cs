using System;
using System.Collections.Generic;
using SolAR;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using UnityEngine;
using XPCF.Api;

public abstract class AbstractPipeline : IPipeline
{
    protected readonly IComponentManager xpcfComponentManager;

    protected readonly CompositeDisposable subscriptions = new CompositeDisposable();

    readonly List<IComponentIntrospect> _xpcfComponents = new List<IComponentIntrospect>();
    public IEnumerable<IComponentIntrospect> xpcfComponents
    {
        get { return _xpcfComponents; }
    }

    protected AbstractPipeline(IComponentManager xpcfComponentManager)
    {
        this.xpcfComponentManager = xpcfComponentManager;
    }

    public void Dispose()
    {
        subscriptions.Dispose();
    }

    protected T Create<T>(string type) where T : IComponentIntrospect, IDisposable
    {
        var component = xpcfComponentManager.Create(type).AddTo(subscriptions).BindTo<T>().AddTo(subscriptions);
        _xpcfComponents.Add(component);
        return component;
    }

    protected T Create<T>(string type, string name) where T : IComponentIntrospect, IDisposable
    {
        var component = xpcfComponentManager.Create(type, name).AddTo(subscriptions).BindTo<T>().AddTo(subscriptions);
        _xpcfComponents.Add(component);
        return component;
    }

    protected void LOG_ERROR(string message, params object[] objects) { Debug.LogErrorFormat(message, objects); }
    protected void LOG_INFO(string message, params object[] objects) { Debug.LogWarningFormat(message, objects); }
    protected void LOG_DEBUG(string message, params object[] objects) { Debug.LogFormat(message, objects); }

    public abstract Sizef GetMarkerSize();
    public abstract void SetCameraParameters(Matrix3x3f intrinsic, Vector5f distorsion);
    public abstract FrameworkReturnCode Proceed(Image inputImage, Transform3Df pose);
}
