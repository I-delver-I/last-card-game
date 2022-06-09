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
            UserPlayer user = gameObject.GetComponent<UserPlaceholder>().PlaceUser(GetComponent<UserPlayer>());
            players.Add(user);
            // spawn required bots amount
            for (var i = 0; i < botsAmount; i++)
            {
                BotPlayer bot = gameObject.GetComponent<BotPlaceholder>().PlaceBot(GetComponent<BotPlayer>(), i + 1);
                players.Add(bot);
            }
            // add them to players/initialize
        }

        private void DistributeCards()
        {
            foreach (Player player in players)
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
