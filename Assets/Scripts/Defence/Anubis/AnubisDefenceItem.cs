using Board;

namespace Defence.Anubis
{
    public class AnubisDefenceItem : DefenceItem
    {
        private void Awake()
        {
            Attack = GetComponent<AnubisAttack>();
        }
        
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}