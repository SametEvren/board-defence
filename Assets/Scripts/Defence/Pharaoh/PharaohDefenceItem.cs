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
    }
}