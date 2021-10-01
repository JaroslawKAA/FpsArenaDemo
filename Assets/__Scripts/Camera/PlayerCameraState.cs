using UnityEngine;

public class PlayerCameraState : CameraStateBase
{
    private float cameraVerticalAngle;

    public PlayerCameraState(CameraStateMachine instance) : base(instance)
    {
        target = GameManager.S.Player.CameraPositionTarget;
    }

    protected internal override void OnEnter()
    {
        base.OnEnter();
        instance.state = CameraState.FollowPlayer;
        instance.transform.parent = GameManager.S.Player.CameraPositionTarget;
    }

    protected internal override void OnUpdate()
    {
        base.OnUpdate();

        HandleYLook();
        instance.transform.localPosition = GetCameraPosition();
    }

    /// <summary>
    /// Raycast in order to stop camera on collider.
    /// </summary>
    /// <returns>Local position relatively to target object.</returns>
    private Vector3 GetCameraPosition()
    {
        Vector3 cameraPosition;

        Debug.DrawRay(target.position + target.rotation * Vector3.right * instance.offsetX, -target.forward * instance.distanceFromPlayer, Color.green);
        if (Physics.Raycast(
            target.position + target.rotation * Vector3.right * instance.offsetX,
            -target.forward,
            out RaycastHit hit,
            instance.distanceFromPlayer,
            instance.physicMask))
        {
            cameraPosition = target.InverseTransformPoint(hit.point);
        }
        else
            cameraPosition = new Vector3(instance.offsetX, 0, -instance.distanceFromPlayer);
        
        return cameraPosition;
    }

    private void HandleYLook()
    {
        float lookY = Input.GetAxisRaw("Mouse Y");

        cameraVerticalAngle -= lookY * instance.mouseSensitivity * Time.deltaTime;
        cameraVerticalAngle = Mathf.Clamp(
            cameraVerticalAngle,
            -instance.verticalRotationLimit,
            instance.verticalRotationLimit);

        target.rotation = Quaternion.Euler(
            new Vector3(cameraVerticalAngle,
            target.rotation.eulerAngles.y,
            target.rotation.eulerAngles.z));
    }

    public override Transform GetTarget()
    {
        Vector3 globalPosition = base.target.TransformPoint(GetCameraPosition());
        
        GameObject target = new GameObject { transform = { position = globalPosition, rotation = base.target.rotation}};
        return target.transform;
    }
}