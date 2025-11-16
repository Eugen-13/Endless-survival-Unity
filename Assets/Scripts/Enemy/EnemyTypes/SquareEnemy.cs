using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SquareEnemy : BaseEnemy
{
    [Header("Lunge Settings")]
    [SerializeField] private float _lungeDistance = 6f;
    [SerializeField] private float _lungeDuration = 0.3f;
    [SerializeField] private float _backDuration = 0.25f;
    [SerializeField] private float _turnDuringLunge = 360f;
    [SerializeField] private float _preLungeDelay = 2f;

    private Vector3 _startPosition;
    private Collider2D _enemyCollider;

    protected override void Awake()
    {
        base.Awake();
        _agent.radius = 1.5f;
        _repelRadius = 3;
        _repelForce = 4;

        _enemyCollider = GetComponent<Collider2D>();
    }


    public override void InitaliceStats(EnemyStats enemyStats)
    {
        base.InitaliceStats(enemyStats);
        this.AttackRange = _lungeDistance;
        this.Speed = enemyStats.Speed / 2;
        this.Health = enemyStats.Health * 2;
        _damage = enemyStats.Damage * 2;
    }

    protected override IEnumerator AttackRoutine()
    {
        if (_isAttacking)
            yield break;

        _isAttacking = true;
        _agent.isStopped = true;
        _startPosition = transform.position;

        Vector3 dirToPlayer = (_player.position - _startPosition).normalized;
        Vector3 lungeTarget = _startPosition + dirToPlayer * Vector3.Distance(_startPosition, _player.position);

        _agent.updatePosition = false;

        Coroutine checkCollision = null;

        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        _attackSequence = DOTween.Sequence();
        _attackSequence.Join(_spriteRenderer.DOColor(Color.red, 0.1f));
        _attackSequence.AppendInterval(_preLungeDelay);
        _attackSequence.AppendCallback(() => StartCoroutine(CheckPlayerCollisionDuringLunge()));
        _attackSequence.Join(transform.DOMove(lungeTarget, _lungeDuration).SetEase(Ease.OutQuad));
        _attackSequence.Join(transform.DORotate(new Vector3(0, 0, _turnDuringLunge), _lungeDuration).SetEase(Ease.OutCubic));
        _attackSequence.Append(transform.DOMove(_startPosition, _backDuration).SetEase(Ease.InOutSine));
        _attackSequence.Join(transform.DORotate(Vector3.zero, _backDuration).SetEase(Ease.InOutSine));
        _attackSequence.Append(_spriteRenderer.DOColor(Color.white, 0.2f));
        _attackSequence.OnComplete(() =>
        {
            _agent.Warp(transform.position);
            _agent.updatePosition = true;
            _agent.isStopped = false;
            _lastAttackTime = Time.time;
            _isAttacking = false;

            if (checkCollision != null)
                StopCoroutine(checkCollision);
        });

        _attackSequence.Play();
        yield return _attackSequence.WaitForCompletion();
        yield return new WaitForSeconds(0.05f);
    }

    private IEnumerator CheckPlayerCollisionDuringLunge()
    {
        bool hasHit = false;
        float checkRadius = _enemyCollider.bounds.extents.magnitude;

        while (_isAttacking && !hasHit)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, checkRadius, LayerMask.GetMask("Player"));

            if (hit != null && hit.CompareTag("Player"))
            {
                hasHit = true;

                var player = hit.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(_damage);
                    DamagePopupManager.Instance.ShowPopup((int)_damage, player.transform.position, Color.red);
                }
            }

            yield return null;
        }
    }
}