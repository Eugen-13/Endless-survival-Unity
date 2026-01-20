using System;

namespace Core.HealthBar
{
    public interface IHealth
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        event Action<float, float> OnHealthChanged;
        void TakeDamage(float amount);
        void Heal(float amount);
    }
}
