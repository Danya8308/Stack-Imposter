using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase<TContext> : MonoBehaviour, IStoppable
{
    public event Func<bool> WhenCheckingLaunchCapability;

    public event Action OnLaunch;
    public event Action OnUpdate;
    public event Action OnStop;

    private IEnumerator _cycle;


    public TContext CurrentContext { get; private set; }

    public bool Active => _cycle != null;

    public float ProcessValue { get; private set; }

    public virtual float Duration => 0f;

    protected virtual IEnumerable<ActionComponent<TContext>> _components => null;

    private void Awake()
    {
        if (_components != null)
        {
            InitComponents();
        }
    }

    protected void OnDestroy()
    {
        if (_components != null)
        {
            ResetComponents();
        }
    }

    public bool Launch(TContext context)
    {
        if (context == null)
        {
            return false;
        }

        CurrentContext = context;

        if (Active == true || WhenCheckingLaunchCapability?.Invoke() == false)
        {
            return false;
        }

        BeforeLaunchEvent();

        _cycle = UpdateCycle();

        OnLaunch?.Invoke();
        LaunchEvent();

        StartCoroutine(_cycle);

        return true;
    }

    public void Stop()
    {
        if (Active == false)
        {
            return;
        }

        StopCoroutine(_cycle);
        _cycle = null;

        OnStop?.Invoke();
        StopEvent();

        ProcessValue = 0f;
        CurrentContext = default(TContext);
    }

    public void Restart(TContext context)
    {
        Stop();
        Launch(context);
    }

    #region VIRTUAL EVENTS

    protected virtual void BeforeLaunchEvent() { }

    protected virtual void LaunchEvent() { }

    protected virtual void UpdateEvent() { }

    protected virtual void StopEvent() { }

    #endregion

    private IEnumerator UpdateCycle()
    {
        var expiredTime = 0f;

        while (ProcessValue < 1f)
        {
            if (Duration <= 0f)
            {
                break;
            }

            expiredTime += Time.deltaTime;
            ProcessValue = expiredTime / Duration;

            if (ProcessValue > 1f)
            {
                ProcessValue = 1f;
            }

            OnUpdate?.Invoke();
            UpdateEvent();

            yield return new WaitForFixedUpdate();
        }

        Stop();
    }

    private void InitComponents()
    {
        foreach (var component in _components)
        {
            component.Accept(this);
        }
    }

    private void ResetComponents()
    {
        foreach (var component in _components)
        {
            component.Reset(this);
        }
    }
}