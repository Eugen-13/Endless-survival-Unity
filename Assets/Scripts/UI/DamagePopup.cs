using Core.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DamagePopup : PoolableObject
    {
        [SerializeField] private TextMeshPro _text;

        private Sequence _sequence;

        public void Show(string text, Color color, float fadeDuration)
        {
            _text.text = text;
            _text.color = color;
            _sequence?.Kill();

            _sequence = DOTween.Sequence()
                .Append(_text.DOFade(0f, fadeDuration).SetEase(Ease.InCubic))
                .OnComplete(() =>
                {
                    _text.alpha = 1f;
                    ReturnToPool();
                });
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}