using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster GM;
        public string WinnerName;
        public List<string> RunnerUpps;
        public bool GameStarted = false;

        private void Awake() 
        {
            if (GM != null)
            {
                GameObject.Destroy(GM);
            }
            else
            {
                GM = this;
            }

            DontDestroyOnLoad(this);
        }
    }
}
