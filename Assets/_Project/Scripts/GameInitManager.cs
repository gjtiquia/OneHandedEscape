using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GameInitManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }

        public void PressPlay()
        {
            LoadSceneManager.LoadScene(SceneName.GameScene);
        }
    }
}
