using System;
using Board;
using ItemPlacement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIScripts
{
    [RequireComponent(typeof(Button))]
    public class ItemPlacementButton : MonoBehaviour
    {
        [SerializeField] private DefenceItemType itemType;
        [SerializeField] private ItemPlacementController itemPlacementController;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void HandleButtonClicked()
        {
            itemPlacementController.StartPlacing(itemType);
        }
    }
}