using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GameInitManager : MonoBehaviour
    {
        public void PressPlay()
        {
            Debug.Log("Play Pressed!");
            LoadSceneManager.LoadGameScene();
        }
    }
}
