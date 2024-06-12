using System;
using Board;
using UnityEngine;

namespace Defence.Pharaoh
{
    [RequireComponent(typeof(PharaohAttack))]
    public class PharaohDefenceItem : DefenceItem
    {
        private void Awake()
        {
            Attack = GetComponent<PharaohAttack>();
        }

        protected override void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            base.HandleChangeInArea(occupier, added);
        }
    }
}