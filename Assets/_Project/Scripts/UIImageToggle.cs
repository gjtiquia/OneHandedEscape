using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    [RequireComponent(typeof(Image))]
    public class UIImageToggle : MonoBehaviour
    {
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetActiveImage()
        {
            _image.sprite = _activeSprite;
        }

        public void SetInactiveImage()
        {
            _image.sprite = _inactiveSprite;
        }
    }
}