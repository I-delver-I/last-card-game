using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastCard
{
    [CreateAssetMenu(fileName = "Setting", menuName = "Game settings")]
    public class GameSettings : ScriptableObject
    {
        public int BotsCount = 1;
        public int InitialCardsCount = 4;
        public int MaximalScore = 20;
        public bool GameStarted = false;
        public string WinnerName;
        public List<string> RunnerUpps = new List<string>();
    }
}
