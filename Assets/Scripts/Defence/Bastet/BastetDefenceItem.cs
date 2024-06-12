using Board;

namespace Defence.Bastet
{
    public class BastetDefenceItem : DefenceItem
    {
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}