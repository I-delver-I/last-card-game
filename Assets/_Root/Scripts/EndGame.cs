using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LastCard
{
    public class EndGame : MonoBehaviour
    {
        public GameSettings settings;
        public Text Congratulation;
        public Text RunnerUpps;

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += ShowCongratulation;
            SceneManager.sceneLoaded += ShowRunnerUpps;
        }

        private void OnDisable() 
        {
            SceneManager.sceneLoaded -= ShowCongratulation;
            SceneManager.sceneLoaded -= ShowRunnerUpps;
        }

        public void ShowCongratulation(Scene scene, LoadSceneMode mode)
        {
            Congratulation.text = $"Player {settings.WinnerName} Win!";
        }

        public void ShowRunnerUpps(Scene scene, LoadSceneMode mode)
        {
            foreach (string runnerUp in settings.RunnerUpps)
            {
                RunnerUpps.text += runnerUp + '\n';
            }
        }
    }
}
