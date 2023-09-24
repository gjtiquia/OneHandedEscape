using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GameInitManager : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 1;
            Application.targetFrameRate = 120;
        }

        public void PressPlay()
        {
            LoadSceneManager.LoadScene(SceneName.GameScene);
        }
    }
}
