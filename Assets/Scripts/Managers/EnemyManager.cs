using System.Collections.Generic;
using EnemyTypes;
using UnityEngine;

namespace Managers
{
    public class EnemyManager
    {
        private readonly List<Transform> _enemies = new();
        private readonly List<BaseEnemy> _behaviours = new();
        
        public List<Transform> Enemies => _enemies;
        public List<BaseEnemy>  Behaviours => _behaviours;

        public void Register(BaseEnemy enemy)
        {
            if (!_enemies.Contains(enemy.transform))
                _enemies.Add(enemy.transform);

            if (!_behaviours.Contains(enemy))
                _behaviours.Add(enemy);
        }

        public void Unregister(BaseEnemy enemy)
        {
            _enemies.Remove(enemy.transform);
            _behaviours.Remove(enemy);
        }

        public void Clear()
        {
            _enemies.Clear();
            _behaviours.Clear();
        }
    }
}