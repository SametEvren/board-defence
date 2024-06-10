using Board;

namespace Defence
{
    public class BastetDefenceItem : DefenceItem
    {
        protected override void HandleChangeInArea(ISlotOccupier occupier)
        {
            base.HandleChangeInArea(occupier);
        }
    }
}