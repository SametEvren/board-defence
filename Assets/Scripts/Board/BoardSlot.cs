using System;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BoardSlot : MonoBehaviour
    {
        public event Action<ISlotOccupier> OnOccupationChanged;
        
        [SerializeField] private Material unplaceableSlotMaterial;
        [SerializeField] private Material placeableSlotMaterial;
        [SerializeField] private Material highlightMaterial;

        private MeshRenderer _meshRenderer;
        private ISlotOccupier _currentOccupant;
        private bool _isPlaceable;
        private bool IsOccupied => _currentOccupant != null;
        public bool CanAllowPlacement => !IsOccupied && _isPlaceable;

        [SerializeField] private Vector2Int _coordinates;
        public Vector2Int BoardCoordinates { get => _coordinates; private set => _coordinates = value; }
        
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OccupySlot(ISlotOccupier newOccupant)
        {
            _currentOccupant = newOccupant;
            _currentOccupant.OnRemovedFromSlot += RemoveOccupant;
            OnOccupationChanged?.Invoke(_currentOccupant);
        }

        private void RemoveOccupant()
        {
            _currentOccupant.OnRemovedFromSlot -= RemoveOccupant;
            _currentOccupant = null;
            OnOccupationChanged?.Invoke(_currentOccupant);
        }

        public void SetHighlight(bool isHighlighting)
        {
            if (isHighlighting)
                _meshRenderer.material = highlightMaterial;
            else
                RenderPlaceableStatus();
        }

        public void InitializeSlot(Vector2Int coordinates, bool isPlaceable)
        {
            BoardCoordinates = coordinates;
            _isPlaceable = isPlaceable;
            RenderPlaceableStatus();
        }

        public void RenderPlaceableStatus()
        {
            _meshRenderer.material = _isPlaceable ? placeableSlotMaterial : unplaceableSlotMaterial;
        }
    }
}