using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BoardSlot : MonoBehaviour
    {
        [SerializeField] private Material unplaceableSlotMaterial;
        [SerializeField] private Material placeableSlotMaterial;

        private MeshRenderer _meshRenderer;
        private ISlotOccupier _currentOccupant;
        private bool _isPlaceable;
        public bool CanAllowPlacement => !IsOccupied && _isPlaceable;
        public bool IsOccupied => _currentOccupant != null;
        public void SetPlaceable(bool placable) => _isPlaceable = placable;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OccupySlot(ISlotOccupier newOccupant)
        {
            _currentOccupant = newOccupant;
        }

        public void RenderAsPlaceable()
        {
            SetPlaceable(true);
            _meshRenderer.material = _isPlaceable ? placeableSlotMaterial : unplaceableSlotMaterial;
        }
    }
}