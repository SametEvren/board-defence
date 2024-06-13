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

        private void OnDestroy()
        {
            _playerController.OnDamageTaken -= UpdateHealthBar;
            _playerController.OnPlayerDestroyed -= SetDefeatScreen;
        }
    }
}