using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ECS.Scripts.Components
{
    public class WaveView : MonoBehaviour
    {
        public Action onReadyClick;

        public void OnReadyClick()
        {
            onReadyClick?.Invoke();
        }

        public void OnBackClick()
        {
            SceneManager.LoadSceneAsync("MenuScene");
            Time.timeScale = 1f;
        }
    }
}