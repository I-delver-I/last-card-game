using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameDirector director;
        
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Slider botsSlider;

        [SerializeField]
        private Slider cardsSlider;

        public void StartNewGame()
        {
            gameObject.SetActive(false);
            director.StartProgram();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void AssignBotsCount()
        {
            director.botsAmount = (int)botsSlider.value;
        }

        public void AssignCardsCount()
        {
            director.initialCardsPerPlayer = (int)cardsSlider.value;
        }
    }
}
