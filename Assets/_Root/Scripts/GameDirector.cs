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

        [SerializeField]
        private UserPlaceholder userHolder;
        
        [SerializeField]
        private List<BotPlaceholder> botHolders;

        [SerializeField]
        private BotPlayer botPrefab;

        [SerializeField]
        private UserPlayer userPrefab;

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
            UserPlayer user = userHolder.PlaceUser(userPrefab);
            players.Add(user);
            // spawn required bots amount
            for (var i = 0; i < botsAmount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefab);
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
