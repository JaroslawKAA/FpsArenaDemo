using DG.Tweening;
using UnityEngine;

namespace UnityTemplateProjects.Camera
{
    public class ChangeStateCameraState : CameraStateBase
    {
        private CameraState nextState;
        
        public ChangeStateCameraState(CameraStateMachine instance) : base(instance)
        {
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetNextState(CameraState nextState)
        {
            this.nextState = nextState;
        }

        protected internal override void OnEnter()
        {
            base.OnEnter();
            instance.state = CameraState.ChangeState;
            
            // Move position by DOTween
            instance.transform.DOMove(target.position, 1f)
                .SetEase(Ease.InOutQuint)
                .OnComplete(() =>
                {
                    // On complete change state
                    instance.ChangeState(nextState, false);
                });

            // Rotate by DOTween
            instance.transform.DORotateQuaternion(target.rotation, 1f)
                .SetEase(Ease.InOutQuint);
        }

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}