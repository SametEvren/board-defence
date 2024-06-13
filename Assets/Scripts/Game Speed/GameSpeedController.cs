using System;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game_Speed
{
    public class GameSpeedController : MonoBehaviour
    {
        [SerializeField] private Image pauseControlImage;
        [SerializeField] private Image speedControlImage;

        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite playSprite;
        [SerializeField] private Sprite fastForwardSprite;
        [SerializeField] private Sprite normalSpeedSprite;

        private float _previousTimeScale;

        private bool _gamePaused;
        private bool _gameSpeedUp;
        
        private GameStateController _gameStateController;
        
        [Inject]
        public void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
            _gameStateController.OnGameStateChanged += HandleGameStateChanged;
        }

        private void Start()
        {
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            _gameStateController.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    Continue();
                    break;
                case GameState.Paused:
                    Pause();
                    break;
                case GameState.Victory:
                case GameState.Defeat:
                    Pause();
                    pauseControlImage.gameObject.SetActive(false);
                    speedControlImage.gameObject.SetActive(false);
                    break;
            }
        }

        public void ControlPause()
        {
            if (_gamePaused)
            {
                _gameStateController.SetGameState(GameState.Playing);
            }
            else
            {
                _gameStateController.SetGameState(GameState.Paused);
            }
        }

        public void ControlSpeed()
        {
            if (_gameSpeedUp)
            {
                NormalSpeed();
            }
            else
            {
                FastForward();
            }
        }
        
        private void Continue()
        {
            _gamePaused = false;
            Time.timeScale = _previousTimeScale > 0 ? _previousTimeScale : 1f;
            UpdatePauseControl(pauseSprite);
        }
        
        private void Pause()
        {
            _gamePaused = true;
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            UpdatePauseControl(playSprite);
        }
        
        private void NormalSpeed()
        {
            _gameSpeedUp = false;
            if (_gameStateController.CurrentState == GameState.Playing)
            {
                Time.timeScale = 1f;
            }
            UpdateSpeedControl(normalSpeedSprite);
        }

        private void FastForward()
        {
            _gameSpeedUp = true;
            if (_gameStateController.CurrentState == GameState.Playing)
            {
                Time.timeScale = 2f;
            }
            UpdateSpeedControl(fastForwardSprite);
        }

        private void UpdatePauseControl(Sprite newSprite)
        {
            if (pauseControlImage != null)
            {
                pauseControlImage.sprite = newSprite;
            }
        }

        private void UpdateSpeedControl(Sprite newSprite)
        {
            if (speedControlImage != null)
            {
                speedControlImage.sprite = newSprite;
            }
        }
    }
}
