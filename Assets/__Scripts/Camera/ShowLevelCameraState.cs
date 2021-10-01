
public class ShowLevelCameraState : CameraStateBase
{
    public ShowLevelCameraState(CameraStateMachine instance) : base(instance)
    {
        target = GameManager.S.ShowLevelCameraPivot.transform;
    }

    protected internal override void OnEnter()
    {
        base.OnEnter();
        instance.state = CameraState.ShowLevel;
    }

    protected internal override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected internal override void OnExit()
    {
        base.OnExit();
    }
}
