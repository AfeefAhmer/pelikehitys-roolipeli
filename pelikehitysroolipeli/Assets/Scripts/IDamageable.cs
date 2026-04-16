

using UnityEngine.Events;

namespace Assets.Scripts
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        event UnityAction OnDeath;
    }
}
