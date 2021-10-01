using UnityEngine;

public class DestroyAfterSoundFinish : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    private void OnEnable()
    {
        float timeDelay = _source.clip.length;
        Destroy(gameObject, timeDelay);
    }
}
