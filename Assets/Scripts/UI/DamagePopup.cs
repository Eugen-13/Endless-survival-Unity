using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : PoolableObject
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private  float _fadeDuration = 1f;
    private Sequence _sequence;

    public void Show(int damage, Color color)
    {
        _text.text = "-" + damage.ToString();
        _text.color = color;
        _sequence?.Kill();
  
        Vector3 startPos = transform.position;

        _sequence = DOTween.Sequence()
            .Append(_text.DOFade(0f, _fadeDuration)
                .SetEase(Ease.InCubic))
            .OnComplete(() =>
            {
                _text.alpha = 1f; 
                ReturnToPool();
            });
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}