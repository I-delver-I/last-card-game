namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;

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

        private async void StartGame()
        {
            var playerIndex = GetStartPlayerIndex();
            
            while (true) // Not game is completed
            {
                if (true) // if user can make a turn
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
            return 0;
        }

        private int GetNextPlayerIndex(int index)
        {
            return (index + 1) % players.Count;
        }
    }
}
