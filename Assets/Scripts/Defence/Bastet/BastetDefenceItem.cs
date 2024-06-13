
namespace Defence.Bastet
{
    public class BastetDefenceItem : DefenceItem
    {
        private void Awake()
        {
            Attack = GetComponent<BastetAttack>();
        }
    }
}