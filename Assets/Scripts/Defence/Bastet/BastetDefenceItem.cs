using Board;

namespace Defence.Bastet
{
    public class BastetDefenceItem : DefenceItem
    {
        private void Awake()
        {
            Attack = GetComponent<BastetAttack>();
        }
        
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}