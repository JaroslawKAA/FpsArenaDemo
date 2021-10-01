using UnityEngine;

public abstract class CameraStateBase
{
    protected CameraStateMachine instance;
    protected Transform target;

    protected CameraStateBase(CameraStateMachine instance)
    {
        this.instance = instance;
    }

    protected internal virtual void OnEnter()
    {
    }
    
    protected internal virtual void OnUpdate()
    {
    }

    protected internal virtual void OnExit()
    {
    }

    public virtual Transform GetTarget()
    {
        return target;
    }
}
