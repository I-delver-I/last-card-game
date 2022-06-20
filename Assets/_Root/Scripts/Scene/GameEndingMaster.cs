using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace LastCard
{
    public class GameEndingMaster : MonoBehaviour
    {
        [SerializeField]
        private Text congratulation;

        [SerializeField]
        private Text runnerUpps;

        private void Awake() 
        {
            if (GameMaster.GM.Winner != null)
            {
                congratulation.text = GameMaster.GM.Winner;
            }
            else
            {
                congratulation.text = "Draw";
            }

            StringBuilder builder = new StringBuilder();
            
            foreach (string runnerUp in GameMaster.GM.RunnerUpps)
            {
                builder.Append($"{runnerUp}\n");
            }

            if (builder.Length == 0)
            {
                runnerUpps.alignment = TextAnchor.MiddleCenter;
                runnerUpps.text = "Congratulations!\nThe Last Player is found!";
            }
            else
            {
                runnerUpps.text = builder.ToString();
            }
        }

        public void ContinueGameOrExit()
        {
            if ((GameMaster.GM.RunnerUpps.Count == 0) 
                || (!GameMaster.GM.Winner.Contains("UserPlayer") 
                && (GameMaster.GM.RunnerUpps.Find(player => player.Contains("UserPlayer")) == default)))
            {
                Application.Quit();
            }

            if (GameMaster.GM.Winner != "Draw")
            {
                MainMenuMaster.mainMenuMaster.BotsCount = GameMaster.GM.RunnerUpps.Count + 1;
            }
            else
            {
                MainMenuMaster.mainMenuMaster.BotsCount = GameMaster.GM.RunnerUpps.Count;
            }

            MainMenuMaster.mainMenuMaster.StartGame();
        }
    }
}
