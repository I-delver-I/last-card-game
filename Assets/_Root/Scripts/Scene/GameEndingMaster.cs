using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
