namespace Board
{
    public interface ISlotOccupier
    {
        //TODO: ???
        public SlotOccupantType OccupantType {get;}
    }

    public enum SlotOccupantType
    {
        None,
        Defence,
        Enemy
    }
}