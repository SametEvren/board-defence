using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
                });
        }
    }
}