using UnityEngine;
using UnityEngine.Events;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Project.UI
{
    public class GameViewController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _mainPage;
        [SerializeField] private GameObject _pausePage;
        [SerializeField] private GameObject _miniMapView;

        [Header("Events")]
        [SerializeField] private UnityEvent _onPausePressedEvent;
        [SerializeField] private UnityEvent _onResumePressedEvent;
        [SerializeField] private UnityEvent _onSaveAndQuitPressedEvent;

        private void OnValidate()
        {
            UnityAssert.IsNotNull(_mainPage);
            UnityAssert.IsNotNull(_pausePage);
            UnityAssert.IsNotNull(_miniMapView);
        }

        private void Awake()
        {
            ShowMainPageOnly();
            ToggleMinimapView(false);
        }

        // PUBLIC METHODS
        public void OnPausePressed()
        {
            ShowPausePageOnly();
            ToggleMinimapView(true);

            _onPausePressedEvent?.Invoke();
        }

        public void OnResumePressed()
        {
            ShowMainPageOnly();
            ToggleMinimapView(false);

            _onResumePressedEvent?.Invoke();
        }

        public void OnSaveAndQuitPressed()
        {
            _onSaveAndQuitPressedEvent?.Invoke();
        }

        // PRIVATE METHODS
        private void ShowMainPageOnly()
        {
            _mainPage.SetActive(true);
            _pausePage.SetActive(false);
        }

        private void ShowPausePageOnly()
        {
            _mainPage.SetActive(false);
            _pausePage.SetActive(true);
        }

        private void ToggleMinimapView(bool isActive)
        {
            _miniMapView.SetActive(isActive);
        }
    }
}
