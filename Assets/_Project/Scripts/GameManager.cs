using UnityEngine;

namespace Project
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            UnfreezeTime();
        }

        public void PauseGame()
        {
            FreezeTime();
        }

        public void ResumeGame()
        {
            UnfreezeTime();
        }

        public void SaveAndQuitGame()
        {
            SaveGame();
            ChangeToGameInitScene();
        }

        private void FreezeTime()
        {
            Time.timeScale = 0;
        }

        private void UnfreezeTime()
        {
            Time.timeScale = 1;
        }

        private void SaveGame()
        {
            // TODO
        }

        private void ChangeToGameInitScene()
        {
            LoadSceneManager.LoadScene(SceneName.GameInitScene);
        }
    }
}