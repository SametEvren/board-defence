
namespace Defence.Anubis
{
    public class AnubisDefenceItem : DefenceItem
    {
        private void Awake()
        {
            Attack = GetComponent<AnubisAttack>();
        }
    }
}