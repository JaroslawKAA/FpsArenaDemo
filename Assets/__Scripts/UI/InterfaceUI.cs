using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityTemplateProjects
{
    public class InterfaceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI projectileCounter;
        [SerializeField] private TextMeshProUGUI _waveNumber;
        private WeaponController playerWeaponController;

        private void Awake()
        {
            Assert.IsNotNull(projectileCounter);
            Assert.IsNotNull(_waveNumber);
        }

        private void Start()
        {
            playerWeaponController = GameManager.S.Player.GetComponent<WeaponController>();
            playerWeaponController.OnProjectileCountChange += UpdateProjectilesCounter;
            projectileCounter.color = Color.yellow;

            UpdateProjectilesCounter();
            GameManager.S.OnGameOver += () => { gameObject.SetActive(false); };
            GameManager.S.OnNextWave += UpdateWaveNumber;
            GameManager.S.OnGameStart += UpdateWaveNumber;
        }

        private void UpdateProjectilesCounter()
        {
            projectileCounter.text =
                $"{playerWeaponController.ProjectilesCount}/{playerWeaponController.ProjectilesCountMax}";

            projectileCounter.color = playerWeaponController.ProjectilesCount > 0
                ? Color.yellow
                : Color.red;
        }

        private void UpdateWaveNumber()
        {
            _waveNumber.text = $"Wave: {Spawner.S.Wave}";
        }
    }
}