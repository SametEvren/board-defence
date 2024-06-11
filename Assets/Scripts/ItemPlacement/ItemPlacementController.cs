using System.Collections.Generic;
using System.Linq;
using Board;
using Defence;
using UnityEngine;
using Zenject;

namespace ItemPlacement
{
    public class ItemPlacementController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private List<DefenceItem> itemPrefabs;
        
        private DefenceItemType _currentType;
        private DefenceItem _currentItem;
        private Vector2Int _currentCoordinates;
        private List<BoardSlot> _potentialAffectedArea;
        private bool CurrentlyPlacing => _currentItem != null && _currentType != DefenceItemType.None;

        private BoardController _boardController;
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }

        private void Update()
        {
            if(!CurrentlyPlacing) return;

            var onValidTarget = MouseOnValidTarget(out var targetedSlot);
            
            var wasActive = _currentItem.isActiveAndEnabled;
            _currentItem.gameObject.SetActive(onValidTarget);

            if (wasActive != _currentItem.isActiveAndEnabled)
            {
                _potentialAffectedArea?.Clear();
                PlacementHighlighter.ResetHighLights();
            }

            switch (onValidTarget)
            {
                case false:
                    _currentCoordinates = Vector2Int.one * -1;
                    break;
                case true when targetedSlot.BoardCoordinates != _currentCoordinates:
                    MoveItemToSlot(targetedSlot);
                    break;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(onValidTarget)
                    PlaceItemDown(targetedSlot);
                else
                    CancelPlacement();
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

            targetedSlot.OccupySlot(_currentItem);
            _currentItem.SetAffectedSlots(_potentialAffectedArea);
            
            _boardController.UpdateInventory(_currentType);
            _currentItem = null;
            _currentType = DefenceItemType.None;
            _currentCoordinates = Vector2Int.one * -1;
            _potentialAffectedArea.Clear();

            PlacementHighlighter.ResetHighLights();
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
            _currentCoordinates = targetedSlot.BoardCoordinates;
            _potentialAffectedArea = new List<BoardSlot>(PlacementHighlighter.HighlightPlacementRange(_boardController.BoardSlots, targetedSlot.BoardCoordinates, _currentItem.Data));
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
            Debug.Log("Cancelled Placement");
            _currentType = DefenceItemType.None;
            _currentCoordinates = Vector2Int.one * -1;
            Destroy(_currentItem.gameObject);
            _currentItem = null;
            _potentialAffectedArea.Clear();
            PlacementHighlighter.ResetHighLights();
        }
    }
}