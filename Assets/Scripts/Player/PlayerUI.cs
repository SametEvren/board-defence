using Enemies;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using System.Collections;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private GameObject defeatPanel;
        [SerializeField] private GameObject successPanel;
        private const float DelayBeforeShowPanels = 2f;

        private PlayerController _playerController;
        private EnemySpawnController _enemySpawnController;
        private GameStateController _gameStateController;
        
        [Inject]
        public void Construct(PlayerController playerController, EnemySpawnController enemySpawnController, GameStateController gameStateController)
        {
            _playerController = playerController;
            _enemySpawnController = enemySpawnController;
            _gameStateController = gameStateController;
        }
        
        private void Start()
        {
            _playerController.OnDamageTaken += UpdateHealthBar;
            _playerController.OnPlayerDestroyed += HandlePlayerDestroyed;
            _enemySpawnController.AllEnemiesDefeated += HandleAllEnemiesDefeated;
        }

        private void HandlePlayerDestroyed()
        {
            StartCoroutine(SetDefeatScreenWithDelay());
        }

        private void HandleAllEnemiesDefeated()
        {
            StartCoroutine(SetSuccessScreenWithDelay());
        }

        private IEnumerator SetSuccessScreenWithDelay()
        {
            yield return new WaitForSeconds(DelayBeforeShowPanels);
            successPanel.SetActive(true);
            _gameStateController.SetGameState(GameState.Victory);
        }

        private IEnumerator SetDefeatScreenWithDelay()
        {
            yield return new WaitForSeconds(DelayBeforeShowPanels);
            defeatPanel.SetActive(true);
            _gameStateController.SetGameState(GameState.Defeat);
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
            _playerController.OnPlayerDestroyed -= HandlePlayerDestroyed;
            _enemySpawnController.AllEnemiesDefeated -= HandleAllEnemiesDefeated;
        }
    }
}
