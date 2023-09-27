using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityAssert = UnityEngine.Assertions.Assert;
using TMPro;

namespace Project.UI
{
    public class LoadingViewController : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _loadingText;

        private void OnValidate()
        {
            UnityAssert.IsNotNull(_background);
            UnityAssert.IsNotNull(_loadingText);
        }

        private void Start()
        {
            // TODO : Set all as transparent
            // TODO : Fade in the colors with DOTween   
        }
    }
}