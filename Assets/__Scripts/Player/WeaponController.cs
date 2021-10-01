using System;
using System.Collections;
using __Scripts;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private int projectilesCountMax = 30;
    [SerializeField] private int projectilesCount = 30;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectile;

    [Header("Sounds")] [SerializeField] private AudioSource weaponSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip emptySound;
    [SerializeField] private AudioClip reloadSound;

    private bool _reloading;
    
    public int ProjectilesCount
    {
        get => projectilesCount;
        private set
        {
            projectilesCount = value;
            OnProjectileCountChange?.Invoke();
        }
    }

    public int ProjectilesCountMax => projectilesCountMax;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(projectileSpawnPoint);
        Assert.IsNotNull(projectile);
        Assert.IsNotNull(weaponSource);
    }

    public void Shoot()
    {
        if (_reloading)
            return;
        
        if (projectilesCount > 0)
        {
            // Set rotation of projectile to aimer hit point
            Quaternion lookOnAimer =
                Quaternion.LookRotation(Raycaster.S.AimerHit.point - projectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(projectile,
                projectileSpawnPoint.position,
                lookOnAimer);
            Projectile p = projectileInstance.GetComponent<Projectile>();
            p.AddForce();
            PlaySound(shootSound);

            ProjectilesCount--;
        }
        else
        {
            PlaySound(emptySound);
        }
    }

    public void Reload(bool fast = false)
    {
        if (fast)
        {
            ProjectilesCount = projectilesCountMax;
        }
        else
        {
            PlaySound(reloadSound);
            _reloading = true;
            StartCoroutine(Utils.Delay(reloadSound.length, () =>
            {
                ProjectilesCount = projectilesCountMax;
                _reloading = false;
            }));
        }
    }
    
    private void PlaySound(AudioClip clip)
    {
        weaponSource.clip = clip;
        if (weaponSource.clip != null)
            weaponSource.Play();
    }

    #region Events

    public event Action OnProjectileCountChange;

    #endregion
}