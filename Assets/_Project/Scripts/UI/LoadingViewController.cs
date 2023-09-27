using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityAssert = UnityEngine.Assertions.Assert;
using TMPro;
using DG.Tweening;

namespace Project.UI
{
    public class LoadingViewController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fadeInDuration;

        [Header("References")]
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _loadingText;

        private void OnValidate()
        {
            UnityAssert.IsNotNull(_background);
            UnityAssert.IsNotNull(_loadingText);
        }

        private void Start()
        {
            Color originalBackgroundColor = _background.color;
            _background.color = new Color(0, 0, 0, 0);

            Color originalLoadingTextColor = _loadingText.color;
            _loadingText.color = new Color(0, 0, 0, 0);

            _background.DOColor(originalBackgroundColor, _fadeInDuration);
            _loadingText.DOColor(originalLoadingTextColor, _fadeInDuration);
        }
    }
}