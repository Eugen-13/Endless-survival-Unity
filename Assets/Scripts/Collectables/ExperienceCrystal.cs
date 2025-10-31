using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class ExperienceCrystal : PoolableObject
{
    [SerializeField] private int _expCount;
    [SerializeField] private float _magnetSpeed = 10f;
    private Sequence _sequence;

    private Transform _player;
    private float _pickUpRadius;
    private float _pickUpRadiusSqr;

    public void SetExpCount(int expCount) => _expCount = expCount;

    private void Start()
    {
        _player = Player.Instance.transform;
        _pickUpRadius = Player.Instance.PickUpRadius;
        _pickUpRadiusSqr = _pickUpRadius * _pickUpRadius;
    }

    private void FixedUpdate()
    {
        if (!_player) return;

        
        float sqr = (transform.position - _player.transform.position).sqrMagnitude;

        if (sqr <= 0.1)
        {
            ReturnToPool();
            Player.Instance.AddExperience(_expCount);
        }

        if (sqr < _pickUpRadiusSqr)
        {
            Vector2 dir = (_player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(dir * _magnetSpeed * Time.fixedDeltaTime);
        }
    }

}