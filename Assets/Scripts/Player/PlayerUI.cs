using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private GameObject defeatPanel;
        [SerializeField] private GameObject successPanel;

        private PlayerController _playerController;
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        private void Start()
        {
            _playerController.OnDamageTaken += UpdateHealthBar;
            _playerController.OnPlayerDestroyed += SetDefeatScreen;
        }

        private void SetSuccessScreen()
        {
            successPanel.SetActive(true);
        }
        
        private void SetDefeatScreen()
        {
            defeatPanel.SetActive(true);
        }

        void UpdateHealthBar(float playerHealth, float fullHealth)
        {
            playerHealthBar.fillAmount = playerHealth / fullHealth;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene("Scenes/Game");
        }

        public void NextLevel()
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
            SceneManager.LoadScene("Scenes/Game");
        }

        private void OnDestroy()
        {
            _playerController.OnDamageTaken -= UpdateHealthBar;
            _playerController.OnPlayerDestroyed -= SetDefeatScreen;
        }
    }
}