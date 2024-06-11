using Board;

namespace Defence
{
    public class BastetDefenceItem : DefenceItem
    {
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}