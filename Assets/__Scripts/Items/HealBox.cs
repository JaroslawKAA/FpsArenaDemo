using Player;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealBox : MonoBehaviour
{
    [SerializeField] private int healStrength = 25;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (character.HealthCurrent < character.HealthMax)
            {
                character.Heal(healStrength);
                Destroy(gameObject);
            }
        }
    }
}
