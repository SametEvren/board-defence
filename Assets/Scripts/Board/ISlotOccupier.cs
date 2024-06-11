﻿using System;

namespace Board
{
    public interface ISlotOccupier
    {
        public SlotOccupantType OccupantType {get;}
        public event Action<ISlotOccupier> OnRemovedFromSlot;
    }

    public enum SlotOccupantType
    {
        None,
        Defence,
        Enemy
    }
}