using Core.ObjectPool;
using DG.Tweening;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace Collectables
{
    public class ExperienceCrystal : PoolableObject
    {
        private int _expCount;
        private float _magnetSpeed;
        private Sequence _sequence;
        
        private float _pickUpRadius;
        private float _pickUpRadiusSqr;

        [Inject] private Player _player;

        public void Init(int expCount, float magnetSpeed)
        {
            _expCount = expCount;
            _magnetSpeed = magnetSpeed;
        }

        private void Start()
        {
            _pickUpRadius = _player.PickUpRadius;
            _pickUpRadiusSqr = _pickUpRadius * _pickUpRadius;
        }

        private void FixedUpdate()
        {
            if (!_player) return;

        
            float sqr = (transform.position - _player.transform.position).sqrMagnitude;

            if (sqr <= 0.1)
            {
                ReturnToPool();
                _player.AddExperience(_expCount);
            }

            if (!(sqr < _pickUpRadiusSqr)) return;
            
            Vector2 dir = (_player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(dir * (_magnetSpeed * Time.fixedDeltaTime));
        }

    }
}