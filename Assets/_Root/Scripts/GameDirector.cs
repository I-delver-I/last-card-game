namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System.Threading;

    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private RulesResolver rulesResolver;
        
        [SerializeField]
        private CardsDeck cardsDeck;

        [SerializeField]
        private CardsPile cardsPile;

        [SerializeField]
        private UserPlaceholder userHolder;
        
        [SerializeField]
        private List<BotPlaceholder> botHolders;

        [SerializeField]
        private BotPlayer botPrefab;

        [SerializeField]
        private UserPlayer userPrefab;

        [SerializeField]
        private GameSettings settings;

        //private Button endTurnButton;

        private List<Player> players = new List<Player>();
        private int playerIndex;
        private bool gameIsFinished = false;

        private void Start()
        {
            SpawnPlayers();
            DistributeCards();
            StartGame();
        }

        private void Update() 
        {
            if (Input.GetKey("escape"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void SpawnPlayers()
        {
            // spawn player
            UserPlayer user = userHolder.PlaceUser(userPrefab);
            user.Init(rulesResolver, cardsDeck, cardsPile);
            players.Add(user);
            //endTurnButton = user.endTurnButton;
            // spawn required bots amount
            for (var i = 0; i < settings.BotsCount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefab);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
                players.Add(bot);
                bot.name += $" - {i + 1}";
            }
            // add them to players/initialize
        }

        private void DistributeCards()
        {
            foreach (Player player in players)
            {
                var cards = cardsDeck.GetCards(settings.InitialCardsCount);

                player.AddCards(cards);
            }

            var pileCard = cardsDeck.GetCard();
            pileCard.flipper.Flip();
            cardsPile.PushCard(pileCard);
        }

        private async void StartGame()
        {
            playerIndex = GetStartPlayerIndex();
            Player player = players[playerIndex];
            
            while (!gameIsFinished) // Not game is completed
            {
                player = players[playerIndex];
                
                if (!CheckSkippingTurn(player)) // if user can make a turn
                {
                    player.OnCardSelected += OnPlayerSelectedCard;
                    player.OnCardsMissing += OnPlayerMissingCards;

                    Task turnTask = player.MakeTurn();
                    await turnTask;

                    player.OnCardSelected -= OnPlayerSelectedCard;
                    player.OnCardsMissing -= OnPlayerMissingCards;
                }

                if (!cardsPile.Reversed)
                {
                    playerIndex = GetNextPlayerIndex(playerIndex);
                }
                else
                {
                    playerIndex = GetNextPlayerIndexReversed(playerIndex);
                }

                gameIsFinished = CheckIsCompleted(player);
            }

            EndGame(GetWinner());
        }

        private void EndGame(Player winner)
        {
            Debug.Log("WINNER IS " + winner.name);
            settings.WinnerName = winner.name;
            players.Remove(winner);
            ExcludeLosers();

            foreach (Player player in players)
            {
                settings.RunnerUpps.Add(($"{player.name} - {player.GetPointsNumber()}"));
            }

            SceneManager.LoadScene("GameEnding");
        }

        private bool CheckIsCompleted(Player player)
        {
            if (player.GetCardsCount() == 0)
            {
                return true;
            }

            if ((cardsPile.PeekCard() == null) && NobodyCanMakeTurn())
            {
                return true;
            }

            return false;
        }

        private void ExcludeLosers()
        {
            List<Player> tempPlayers = new List<Player>(players);

            foreach (Player player in tempPlayers)
            {
                if (player.GetPointsNumber() > settings.MaximalScore)
                {
                    players.Remove(player);
                }
            }
        }

        private bool NobodyCanMakeTurn()
        {
            foreach (Player player in players)
            {
                foreach (Card card in player.GetCards())
                {
                    if (rulesResolver.CanPushCard(card))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private Player GetWinner()
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

            if (twoMorePlayersHaveEqualPoints)
            {
                return null;
            }

            return winner;
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

            if (cardsPile.PeekCard() == null) 
            {
                foreach (Card card in player.GetCards())
                {
                    if (rulesResolver.CanPushCard(card))
                    {
                        return false;
                    }
                }

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

            cardsPile.PushCard(card);

            if (player is BotPlayer bot && card.nominal == Nominal.Three)
            {
                Card cardToPush = bot.GetCardToPush();
                cardsPile.PushCard(cardToPush);
                bot.RemoveCard(cardToPush);
            }
            
            if (cardsPile.PeekCard().nominal == Nominal.Jack)
            {
                players[GetNextPlayerIndex(playerIndex)].CanMakeTurn = false;
            }
            else if (card.nominal == Nominal.Eight)
            {
                // Announce new suit
                System.Random random = new System.Random();
                card.suit = (Suit)random.Next(1, 4);
            }
            else if (card.nominal == Nominal.Ace)
            {
                cardsPile.Reversed = !cardsPile.Reversed;
                Debug.Log(cardsPile.Reversed);
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
