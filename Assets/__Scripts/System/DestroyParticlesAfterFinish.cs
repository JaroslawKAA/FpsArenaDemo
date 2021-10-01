using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class DestroyParticlesAfterFinish : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 10);
    }
}
