using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BoardSlot : MonoBehaviour
    {
        public event Action<ISlotOccupier, bool> OnOccupationChanged;
        
        [SerializeField] private Material unplaceableSlotMaterial;
        [SerializeField] private Material placeableSlotMaterial;
        [SerializeField] private Material highlightMaterial;

        private MeshRenderer _meshRenderer;
        private List<ISlotOccupier> _currentOccupants = new List<ISlotOccupier>();
        private bool _isPlaceable;
        private bool IsOccupied => _currentOccupants != null && _currentOccupants.Count != 0;
        public  List<ISlotOccupier> CurrentOccupants => _currentOccupants;
        public bool CanAllowPlacement => !IsOccupied && _isPlaceable;

        [SerializeField] private Vector2Int _coordinates;
        public Vector2Int BoardCoordinates { get => _coordinates; private set => _coordinates = value; }
        
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OccupySlot(ISlotOccupier newOccupant)
        {
            _currentOccupants.Add(newOccupant);
            newOccupant.OnRemovedFromSlot += RemoveOccupant;
            OnOccupationChanged?.Invoke(newOccupant, true);
        }

        private void RemoveOccupant(ISlotOccupier oldOccupant)
        {
            oldOccupant.OnRemovedFromSlot -= RemoveOccupant;
            _currentOccupants.Remove(oldOccupant);
            OnOccupationChanged?.Invoke(oldOccupant, false);
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
            _currentOccupants.Clear();
            RenderPlaceableStatus();
        }

        public void RenderPlaceableStatus()
        {
            _meshRenderer.material = _isPlaceable ? placeableSlotMaterial : unplaceableSlotMaterial;
        }
    }
}
