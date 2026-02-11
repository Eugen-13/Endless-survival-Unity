using System;

namespace Core.HealthBar
{
    public interface IHealth
    {
        event Action<float, float> OnHealthChanged;

        float MaxHealth { get; }

        float CurrentHealth { get; }

        void TakeDamage(float amount);

        void Heal(float amount);
    }
}
