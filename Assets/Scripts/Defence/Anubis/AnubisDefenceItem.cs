using Board;

namespace Defence.Anubis
{
    public class AnubisDefenceItem : DefenceItem
    {
        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}