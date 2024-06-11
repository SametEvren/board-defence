using UnityEngine;
using UnityEngine.UI;

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

        public void ControlPause()
        {
            if (_gamePaused)
            {
                Continue();
            }
            else
            {
                Pause();
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
            Time.timeScale = _previousTimeScale;
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
            Time.timeScale = 1f;
            UpdateSpeedControl(normalSpeedSprite);
        }

        private void FastForward()
        {
            _gameSpeedUp = true;
            Time.timeScale = 2f;
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