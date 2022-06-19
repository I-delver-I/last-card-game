using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LastCard
{
    public class EndGame : MonoBehaviour
    {
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
            Congratulation.text = $"Player {GameMaster.GM.WinnerName} Win!";
        }

        public void ShowRunnerUpps(Scene scene, LoadSceneMode mode)
        {
            foreach (string runnerUp in GameMaster.GM.RunnerUpps)
            {
                RunnerUpps.text += $"{runnerUp}'\n'";
            }
        }
    }
}
