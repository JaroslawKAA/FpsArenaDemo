using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private int healthMax = 100;
        [SerializeField] private int healthCurrent = 100;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private GameObject damageVFX;
        [SerializeField] private Transform vFXPivot;
        [SerializeField] private int damage = 10;
        [SerializeField] private AudioClip takeDamageSound;

        private AudioSource _source;

        private void Awake()
        {
            Assert.IsNotNull(vFXPivot);

            _source = GetComponent<AudioSource>();
        }

        public int HealthCurrent
        {
            private set
            {
                int tempValue = value;
                int tempCurrent = healthCurrent;

                healthCurrent = value;
                healthCurrent = Mathf.Clamp(healthCurrent, 0, healthMax);

                if (tempCurrent > 0 && tempValue <= 0)
                {
                    OnDeath?.Invoke();
                }

                if (tempCurrent > tempValue)
                {
                    OnDamage?.Invoke();
                }

                if (healthBar != null && Alive && healthBar.activeSelf == false)
                    healthBar.SetActive(true);

                OnHealthChange?.Invoke();
            }
            get => healthCurrent;
        }

        public int HealthMax => healthMax;

        public bool Alive => HealthCurrent > 0;

        public GameObject DamageVFX => damageVFX;

        public int Damage => damage;

        public Transform VFXPivot => vFXPivot;

        public void ApplyDamage(int damage)
        {
            HealthCurrent -= damage;
        }

        public void Heal(int value)
        {
            HealthCurrent += value;
        }

        public void PlayDamageEffect()
        {
            if (DamageVFX != null)
                Instantiate(DamageVFX, VFXPivot.position, Quaternion.identity);
        }

        public void PlayDamageEffect(Vector3 spawnPoint)
        {
            if (DamageVFX != null)
                Instantiate(DamageVFX, spawnPoint, Quaternion.identity);
        }

        public void PlayDamageSound()
        {
            _source.clip = takeDamageSound;
            _source.Play();
        }

        #region Events

        public event Action OnHealthChange;

        public event Action OnDeath;

        public event Action OnDamage;

        #endregion
    }
}