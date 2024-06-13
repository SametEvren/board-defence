using System;
using Board;
using Game;
using ItemPlacement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using System.Collections;
using UnityEngine.Assertions;

namespace UIScripts
{
    [RequireComponent(typeof(EventTrigger))]
    public class ItemPlacementButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private DefenceItemType itemType;
        [SerializeField] private ItemPlacementController itemPlacementController;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private float cooldownDuration = 5f;
        
        private bool _isOnCooldown;
        private ItemPlacementController _itemPlacementController;
        private GameStateController _gameStateController;

        [Inject]
        public void Construct(ItemPlacementController placementController, GameStateController gameStateController)
        {
            _itemPlacementController = placementController;
            _gameStateController = gameStateController;
        }

        private void OnValidate()
        {
            Assert.IsNotNull(cooldownImage);
        }

        private void Start()
        {
            cooldownImage.fillAmount = 1;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleButtonClicked();
        }

        private void HandleButtonClicked()
        {
            if (_isOnCooldown) return;

            if (_itemPlacementController.CheckAvailable(itemType) && _gameStateController.CurrentState == GameState.Playing)
            {
                itemPlacementController.StartPlacing(itemType, this);
            }
        }

        public void StartCooldown()
        {
            if (!_isOnCooldown)
            {
                StartCoroutine(CooldownRoutine());
            }
        }

        private IEnumerator CooldownRoutine()
        {
            var startFillAmount = 0.47f;
            _isOnCooldown = true;
            cooldownImage.fillAmount = startFillAmount; 
            var elapsed = 0f;

            while (elapsed < cooldownDuration)
            {
                elapsed += Time.deltaTime;
                cooldownImage.fillAmount = Mathf.Lerp(startFillAmount, 1f,
                    elapsed / cooldownDuration);
                yield return null;
            }

            cooldownImage.fillAmount = 1;
            _isOnCooldown = false;
        }


    }
}
