namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System;
    using UnityEngine.UI;

    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private RulesResolver rulesResolver;
        
        [SerializeField]
        private CardsDeck cardsDeck;

        [SerializeField]
        private CardsPile cardsPile;

        public int initialCardsPerPlayer = 5;

        public int botsAmount = 1;

        [SerializeField]
        private UserPlaceholder userHolder;
        
        [SerializeField]
        private List<BotPlaceholder> botHolders;

        [SerializeField]
        private BotPlayer botPrefab;

        [SerializeField]
        private UserPlayer userPrefab;

        [SerializeField]
        private Background background;

        [SerializeField]
        private Text winText;

        [SerializeField]
        private GameSettings settings;

        private List<Player> players = new List<Player>();
        private int playerIndex;

        private void Start()
        {
            SpawnPlayers();
            DistributeCards();
            StartGame();
        }

        // private void Update() 
        // {
        //     if (Input.GetKey("escape"))
        //     {
        //         menuIsShown = !menuIsShown;
        //         menu.gameObject.SetActive(menuIsShown);
        //     }
        // }

        private void SpawnPlayers()
        {
            // spawn player
            UserPlayer user = userHolder.PlaceUser(userPrefab);
            user.Init(rulesResolver, cardsDeck, cardsPile);
            players.Add(user);
            // spawn required bots amount
            for (var i = 0; i < botsAmount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefab);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
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
            pileCard.flipper.Flip();
            cardsPile.PushCard(pileCard);
        }

        private async void StartGame()
        {
            playerIndex = GetStartPlayerIndex();
            
            while (players.Count != 1) // Not game is completed
            {
                Player player = players[playerIndex];

                if (!CheckSkippingTurn(player)) // if user can make a turn
                {
                    player.OnCardSelected += OnPlayerSelectedCard;
                    player.OnCardsMissing += OnPlayerMissingCards;

                    Task turnTask = player.MakeTurn();
                    await turnTask;

                    player.OnCardSelected -= OnPlayerSelectedCard;
                    player.OnCardsMissing -= OnPlayerMissingCards;

                    if (player.GetCardsCount() == 0)
                    {
                        EndGame(player);
                    }
                    else if (cardsPile == null)
                    {
                        EndGame(players);
                    }
                }

                if (!cardsPile.Reversed)
                {
                    playerIndex = GetNextPlayerIndex(playerIndex);
                }
                else
                {
                    playerIndex = GetNextPlayerIndexReversed(playerIndex);
                }
            }

            EndGame(players);
        }

        private void EndGame(List<Player> players)
        {
            Player winner = players[0];
            bool twoMorePlayersHaveEqualPoints = false;

            for (var i = 1; i < players.Count; i++)
            {
                if (players[i].GetPointsNumber() < winner.GetPointsNumber())
                {
                    winner = players[i];
                }
                else if (players[i].GetPointsNumber() == winner.GetPointsNumber())
                {
                    twoMorePlayersHaveEqualPoints = true;
                }
            }

            if (!twoMorePlayersHaveEqualPoints)
            {
                EndGame(winner);
            }
            else
            {
                winText.text = $"Player {winner} Win!";
                winText.gameObject.SetActive(true);
            }
        }

        private void EndGame(Player winner)
        {
            background.ShowWinMessage(winner);
            players.Remove(winner);

            foreach (Player player in players)
            {
                if (player != winner)
                {
                    if (player.GetPointsNumber() > settings.MaximalScore)
                    {
                        players.Remove(player);
                    }
                }
            }
        }

        private bool CheckSkippingTurn(Player player)
        {
            if (cardsPile.SkipTurn)
            {
                cardsPile.SkipTurn = false;
                player.AddCards(new List<Card>() { cardsDeck.GetCard() });

                return true;
            }
            
            if (!player.CanMakeTurn)
            {
                player.CanMakeTurn = true;

                return true;
            }
            
            return false;
        }

        private bool OnPlayerSelectedCard(Player player, Card card)
        {
            if (!rulesResolver.CanPushCard(card))
            {
                return false;
            }

            player.RemoveCard(card);
            cardsPile.PushCard(card);
            
            if (cardsPile.PeekCard().nominal == Nominal.Jack)
            {
                players[GetNextPlayerIndex(playerIndex)].CanMakeTurn = false;
            }

            return true;
        }

        private void OnPlayerMissingCards(Player player)
        {
            Card lastPileCard = cardsPile.PeekCard();

            if (cardsPile.IsIncrementing && !player.
                ContainsCard(card => (card.nominal == lastPileCard.nominal + 1) && (lastPileCard.suit == card.suit)))
            {
                for (var i = 0; i < (int)lastPileCard.nominal; i++)
                {
                    player.AddCards(new List<Card>() { cardsDeck.GetCard() });
                }

                cardsPile.IsIncrementing = false;
            }
            else
            {
                player.AddCards(new List<Card>() { cardsDeck.GetCard() });
            }
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

        private int GetNextPlayerIndexReversed(int index)
        {
            return (index - 1 + players.Count) % players.Count;
        }
    }
}
