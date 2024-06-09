using System;
using System.Collections.Generic;
using System.Linq;
using Board;
using Defence;
using UnityEngine;

namespace ItemPlacement
{
    public class ItemPlacementController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private List<DefenceItem> itemPrefabs;
        
        private DefenceItemType _currentType;
        private DefenceItem _currentItem;

        private bool CurrentlyPlacing => _currentItem != null && _currentType != DefenceItemType.None;

        private void Update()
        {
            if(!CurrentlyPlacing) return;

            var onValidTarget = MouseOnValidTarget(out var targetedSlot);
            _currentItem.gameObject.SetActive(onValidTarget);

            if (!onValidTarget) return;
           
            MoveItemToSlot(targetedSlot);
            if (Input.GetMouseButtonUp(0))
            {
                PlaceItemDown(targetedSlot);
            }
        }

        public void StartPlacing(DefenceItemType defenceItemType)
        {
            if(CurrentlyPlacing && _currentType == defenceItemType) return;

            if(CurrentlyPlacing) CancelPlacement();
            
            _currentType = defenceItemType;
            _currentItem = SpawnPlacementItem(defenceItemType);

            if (_currentItem == null)
            {
                Debug.LogError("Can't find or spawn the prefab for type:" + defenceItemType);
                CancelPlacement();
            }
        }
        
        private void PlaceItemDown(BoardSlot targetedSlot)
        {
            _currentItem.transform.SetParent(targetedSlot.transform);

            //TODO: Actual Placement
            targetedSlot.OccupySlot(_currentItem);
            
            _currentItem = null;
            _currentType = DefenceItemType.None;
        }

        private bool MouseOnValidTarget(out BoardSlot boardSlot)
        {
            var camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            var slotMask = 1 << LayerMask.NameToLayer("BoardSlot");

            Debug.DrawRay(camRay.origin, camRay.direction * 2000f, Color.red, 1f);
            if (Physics.Raycast(camRay, out var hitInfo, 2000f, slotMask))
            {
                if (hitInfo.transform.TryGetComponent(out boardSlot))
                {
                    return boardSlot.CanAllowPlacement;
                }
            }

            boardSlot = null;
            return false;
        }

        private void MoveItemToSlot(BoardSlot targetedSlot)
        {
            _currentItem.gameObject.SetActive(true);
            _currentItem.transform.position = targetedSlot.transform.position;
        }

        private DefenceItem SpawnPlacementItem(DefenceItemType defenceItemType)
        {
            var prefab = FetchItemFromPrefabs(defenceItemType);
            if (prefab == null) return null;
            
            //TODO: Get from pool
            var item = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            item.gameObject.SetActive(false);
            return item;
        }

        private DefenceItem FetchItemFromPrefabs(DefenceItemType defenceItemType)
        {
            return itemPrefabs.FirstOrDefault(d => d.ItemType == defenceItemType);
        }

        public void CancelPlacement()
        {
            _currentType = DefenceItemType.None;
            Destroy(_currentItem.gameObject);
            _currentItem = null;
        }
    }
}