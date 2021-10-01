using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Character character;
    [SerializeField] private bool disableOnDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(healthBar);
        Assert.IsNotNull(character);

        character.OnHealthChange += UpdateBar;
        
        if (disableOnDeath)
        {
            character.OnDeath += DisableObject;
        }

        UpdateBar();
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

    private void UpdateBar()
    {
        Vector3 scale = healthBar.localScale;

        scale.x = (float) character.HealthCurrent / (float) character.HealthMax;

        healthBar.localScale = scale;
    }
}