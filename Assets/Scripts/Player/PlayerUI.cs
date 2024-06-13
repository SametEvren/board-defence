using Enemies;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using System.Collections;
using DG.Tweening;
using ModestTree;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private GameObject defeatPanel;
        [SerializeField] private GameObject successPanel;
        [SerializeField] private CanvasGroup damageIndicator;
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

        private void OnValidate()
        {
            Assert.IsNotNull(playerHealthBar);
            Assert.IsNotNull(defeatPanel);
            Assert.IsNotNull(successPanel);
            Assert.IsNotNull(damageIndicator);
        }

        private void Start()
        {
            _playerController.OnDamageTaken += UpdateHealthBar;
            _playerController.OnDamageTaken += RenderDamageIndicator;
            _playerController.OnPlayerDestroyed += HandlePlayerDestroyed;
            _enemySpawnController.AllEnemiesDefeated += HandleAllEnemiesDefeated;
        }

        private void RenderDamageIndicator(float _, float __)
        {
            DOTween.Sequence()
                .Append(damageIndicator.DOFade(1f, 0.2f))
                .Append(damageIndicator.DOFade(0f, 0.3f));
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
