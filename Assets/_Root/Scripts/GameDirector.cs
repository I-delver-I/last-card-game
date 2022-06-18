namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

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

        private List<Player> players = new List<Player>();
        private int playerIndex;
        private bool gameIsFinished = false;

        private void Awake() 
        {
            rulesResolver.Init(cardsPile);
        }

        private void Start()
        {
            SceneManager.LoadScene("Game"); // TEST
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
            UserPlayer user = userHolder.PlaceUser(userPrefab);
            user.Init(rulesResolver, cardsDeck, cardsPile);
            players.Add(user);
            
            for (var i = 0; i < settings.BotsCount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefab);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
                players.Add(bot);
            }
        }

        private void DistributeCards()
        {
            foreach (Player player in players)
            {
                var cards = cardsDeck.GetCards(settings.InitialCardsCount);

                player.AddCards(cards);
            }

            Card pileCard = cardsDeck.GetCard();
            pileCard.flipper.Flip();
            cardsPile.PushCard(pileCard);
        }

        private async void StartGame()
        {
            playerIndex = GetStartPlayerIndex();
            Player player = players[playerIndex];
            
            while (!gameIsFinished)
            {
                if (!CheckSkippingTurn(player))
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
                player = players[playerIndex];
            }

            EndGame(GetWinner());
        }

        private void EndGame(Player winner)
        {
            if (winner == null)
            {
                settings.WinnerName = "Draw";
            }
            else
            {
                settings.WinnerName = winner.name;
                players.Remove(winner);
            }
            
            ExcludeLosers();

            foreach (Player player in players)
            {
                settings.RunnerUpps.Add(($"{player.name}: {player.GetPointsNumber()} penalty points"));
            }

            SceneManager.LoadScene("GameEnding");
        }

        private bool CheckIsCompleted(Player player)
        {
            if (player.GetCardsCount() == 0)
            {
                return true;
            }
            else if ((cardsDeck.GetCard() == null) && NobodyCanMakeTurn())
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
            Player winner = players.FirstOrDefault();

            for (var i = 1; i < players.Count; i++)
            {
                if (players[i].GetPointsNumber() < winner.GetPointsNumber())
                {
                    winner = players[i];
                }
                else if (players[i].GetPointsNumber() == winner.GetPointsNumber())
                {
                    return null;
                }
            }

            return winner;
        }

        private bool CheckSkippingTurn(Player player)
        {
            if (cardsPile.SkipTurn)
            {
                cardsPile.SkipTurn = false;
                Card cardFromDeck = cardsDeck.GetCard();

                if (cardFromDeck == null)
                {
                    return true;
                }

                player.AddCards(new List<Card>() { cardFromDeck });

                return true;
            }            
            else if (!player.CanMakeTurn)
            {
                player.CanMakeTurn = true;

                return true;
            }
            else if (cardsDeck.GetCard() == null)
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
            else if (card.nominal == Nominal.Jack)
            {
                players[GetNextPlayerIndex(playerIndex)].CanMakeTurn = false;
            }

            cardsPile.PushCard(card);

            if (player is BotPlayer bot && card.nominal == Nominal.Three)
            {
                Card cardToPush = bot.GetCardToPush();
                cardsPile.PushCard(cardToPush);
                bot.RemoveCard(cardToPush);
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
                    Card cardFromDeck = cardsDeck.GetCard();

                    if (cardFromDeck != null)
                    {
                        player.AddCards(new List<Card>() { cardFromDeck });
                    }
                }

                cardsPile.IsIncrementing = false;
            }
            else
            {
                Card cardFromDeck = cardsDeck.GetCard();
                
                if (cardFromDeck != null)
                {
                    player.AddCards(new List<Card>() { cardFromDeck });
                }
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
