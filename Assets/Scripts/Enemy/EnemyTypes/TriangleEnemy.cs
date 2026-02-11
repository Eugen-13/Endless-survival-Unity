using DG.Tweening;
using UnityEngine;

namespace EnemyTypes
{
    public class TriangleEnemy : BaseEnemy
    {
        protected override void InitaliceAtackSequence()
        {
            _attackSequence = DOTween.Sequence();
            _attackSequence.Append(transform.DOScale(0.2f, 0.1f));
            _attackSequence.Append(transform.DOScale(0.12f, 0.2f));
            _attackSequence.Join(_spriteRenderer.DOColor(Color.red, 0.1f));
            _attackSequence.Append(_spriteRenderer.DOColor(Color.white, 0.2f))
                .Pause()
                .SetAutoKill(false);
        }

        protected override void InitaliceMovementTween()
        {
            _movementTween = transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}
