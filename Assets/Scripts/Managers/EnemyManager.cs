using System.Collections.Generic;
using EnemyTypes;
using UnityEngine;

namespace Managers
{
    public class EnemyManager
    {
        private readonly List<Transform> _enemies = new ();

        public List<Transform> Enemies => _enemies;

        public List<BaseEnemy> Behaviours { get; } = new ();

        public void Register(BaseEnemy enemy)
        {
            if (!_enemies.Contains(enemy.transform))
            {
                _enemies.Add(enemy.transform);
            }

            if (!Behaviours.Contains(enemy))
            {
                Behaviours.Add(enemy);
            }
        }

        public void Unregister(BaseEnemy enemy)
        {
            _enemies.Remove(enemy.transform);
            Behaviours.Remove(enemy);
        }

        public void Clear()
        {
            _enemies.Clear();
            Behaviours.Clear();
        }
    }
}