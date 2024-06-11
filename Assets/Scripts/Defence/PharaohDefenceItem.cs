using Board;

namespace Defence
{
    public class PharaohDefenceItem : DefenceItem
    {
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}