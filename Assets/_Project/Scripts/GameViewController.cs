using UnityEngine;
using UnityEngine.Events;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Project
{
    public class GameViewController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _mainPage;
        [SerializeField] private GameObject _pausePage;

        [Header("Events")]
        [SerializeField] private UnityEvent _onPausePressedEvent;
        [SerializeField] private UnityEvent _onResumePressedEvent;
        [SerializeField] private UnityEvent _onSaveAndQuitPressedEvent;

        private void OnValidate()
        {
            UnityAssert.IsNotNull(_mainPage);
            UnityAssert.IsNotNull(_pausePage);
        }

        private void Awake()
        {
            ShowMainPageOnly();
        }

        // PUBLIC METHODS
        public void OnPausePressed()
        {
            ShowPausePageOnly();
            _onPausePressedEvent?.Invoke();
        }

        public void OnResumePressed()
        {
            ShowMainPageOnly();
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
    }
}
