using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Image gameLogo;
        [SerializeField] private Button playButton;
        [SerializeField] private Image playButtonBorder;
        
        private void Start()
        {
            SetPropertiesBeforeAnimation();
            ActivateAnimation();
        }

        private void SetPropertiesBeforeAnimation()
        {
            playButton.interactable = false;

            playButtonBorder.color = Color.clear;
            playButtonBorder.transform.localScale = Vector3.one * 20;
            playButton.image.color = Color.clear;
            gameLogo.color = Color.clear;
        }

        private void ActivateAnimation()
        {
            var gameLogoSequence = DOTween.Sequence();

            var soundQueDelay = 3f;
            var tweenDuration = 0.5f;
            var tweenDelay = 0.5f;
            
            gameLogoSequence.Append(gameLogo.DOColor(Color.white, tweenDuration * 2).SetDelay(tweenDelay))
                .Insert(soundQueDelay + tweenDelay ,playButton.image.DOColor(Color.white, tweenDuration * 2))
                .Insert(soundQueDelay,playButtonBorder.transform.DOScale(1f, tweenDuration).SetDelay(tweenDelay))
                .Insert(soundQueDelay,playButtonBorder.DOColor(Color.white,tweenDuration).SetDelay(tweenDelay))
                .Insert(soundQueDelay,playButtonBorder.transform.DORotate(Vector3.forward * 1080, tweenDuration, 
                    RotateMode.FastBeyond360).SetDelay(tweenDelay))
                .OnComplete(() =>
                {
                    playButton.interactable = true;
                    RotateBorderRandomly();
                });
        }

        private void RotateBorderRandomly()
        {
            var randValue = Random.Range(0,2);
            
            var tweenDuration = 0.5f;
            
            playButtonBorder.transform.DORotate( (randValue == 1 ? Vector3.forward : Vector3.back) * Random.Range(0f, 360f), tweenDuration,
                RotateMode.FastBeyond360).SetDelay(Random.Range(0.2f, 1.2f))
                .OnComplete(RotateBorderRandomly);
        }

        public void StartTheGame()
        {
            SceneManager.LoadScene("Game");
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}