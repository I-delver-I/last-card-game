namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private CardsDeck cardsDeck;

        [SerializeField]
        private CardsPile cardsPile;

        [SerializeField]
        private int initialCardsPerPlayer = 5;

        [SerializeField]
        private int botsAmount = 1;

        private List<Player> players = new List<Player>();

        private void Start()
        {
            SpawnPlayers();
            DistributeCards();
            StartGame();
        }

        private void SpawnPlayers()
        {
            // spawn player
            // spawn required bots amount
            // add them to players/initialize
        }

        private void DistributeCards()
        {
            foreach (var player in players)
            {
                var cards = cardsDeck.GetCards(initialCardsPerPlayer);
                player.AddCards(cards);
            }

            var pileCards = cardsDeck.GetCard();
            cardsPile.PushCard(pileCards);
        }

        private void StartGame()
        {
            
        }
    }
}
