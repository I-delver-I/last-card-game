namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System;

    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private RulesResolver rulesResolver;
        
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
            CheckCardsCount();
            SpawnPlayers();
            DistributeCards();
            StartGame();
        }

        private void CheckCardsCount()
        {
            if (initialCardsPerPlayer < 4 || initialCardsPerPlayer > 8)
            {
                throw new ArgumentOutOfRangeException("The cards count musn't be less than 4 or bigger than 8");
            }
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

            var pileCard = cardsDeck.GetCard();
            cardsPile.PushCard(pileCard);
        }

        private async void StartGame()
        {
            var playerIndex = GetStartPlayerIndex();
            
            while (players.Count != 1) // Not game is completed
            {
                if (players[playerIndex].CanMakeTurn) // if user can make a turn
                {
                    players[playerIndex].OnCardSelected += OnPlayerSelectedCard;
                    Task turnTask = players[playerIndex].MakeTurn();
                    await turnTask;
                    players[playerIndex].OnCardSelected -= OnPlayerSelectedCard;
                }

                playerIndex = GetNextPlayerIndex(playerIndex);
            }
        }

        private void OnPlayerSelectedCard(Player player, Card card)
        {
            if (!rulesResolver.CanPushCard(card))
            {
                return;
            }
            
            player.RemoveCard(card);
            cardsPile.PushCard(card);
        }

        private int GetStartPlayerIndex()
        {
            System.Random random = new System.Random();

            return random.Next(0, players.Count - 1);
        }

        private int GetNextPlayerIndex(int index)
        {
            return (index + 1) % players.Count;
        }
    }
}
