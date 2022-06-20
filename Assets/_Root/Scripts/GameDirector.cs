namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System.Linq;
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

        // [SerializeField]
        // private GameSettings settings;

        private List<Player> players = new List<Player>();
        private int playerIndex;
        private bool gameIsFinished = false;

        public int BotsCount;
        public int InitialCardsCount;
        public int MaximalPointsCount;

        private void Awake() 
        {
            rulesResolver.Init(cardsPile, cardsDeck);
            cardsPile.Init(cardsDeck);
        }

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
            UserPlayer user = userHolder.PlaceUser(userPrefab);           
            user.Init(rulesResolver, cardsDeck, cardsPile);
            players.Add(user);
            
            for (var i = 0; i < MainMenuMaster.mainMenuMaster.BotsCount; i++)
            {
                BotPlayer bot = botHolders[i].PlaceBot(botPrefab);
                bot.Init(rulesResolver, cardsDeck, cardsPile);
                bot.name += $": {i + 1}";
                players.Add(bot);
            }

            ImprovePlayersNames();
        }

        private void ImprovePlayersNames()
        {
            string stringToDelete = "(Clone)";

            foreach (Player player in players)
            {
                player.name = player.name.Remove(player.name.IndexOf(stringToDelete), stringToDelete.Length);
            }
        }

        private void DistributeCards()
        {
            foreach (Player player in players)
            {
                List<Card> cards = cardsDeck.GetCards(MainMenuMaster.mainMenuMaster.InitialCardsCount);

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
                player = players[playerIndex];

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
            }

            Debug.Log("The game is ending...");

            EndGame(GetWinner());
        }

        private void EndGame(Player winner)
        {
            GameMaster.GM.SetWinner(winner);
            players.Remove(winner);
            ExcludeLosers();
            GameMaster.GM.SetRunnerUpps(players);
            GameMaster.GM.EndGame();
        }

        private bool CheckIsCompleted(Player player)
        {
            Debug.Log($"Check {player} is completed - {player.GetCardsCount()}");

            if (player.GetCardsCount() == 0)
            {
                Debug.Log("Player's cards ended up");

                return true;
            }
            else if ((cardsDeck.CardsLeft == 0) && NobodyCanMakeTurn())
            {
                Debug.Log("Nobody can make turn");

                return true;
            }

            return false;
        }

        private void ExcludeLosers()
        {
            List<Player> tempPlayers = new List<Player>(players);
            bool loserCanBeRemoved = true;

            while (loserCanBeRemoved)
            {
                loserCanBeRemoved = false;
                Player playerToRemove = null;

                foreach (Player player in tempPlayers)
                {
                    if (player.GetPointsNumber() > MainMenuMaster.mainMenuMaster.MaximalPointsCount)
                    {
                        playerToRemove = player;
                        loserCanBeRemoved = true;
                        break;
                    }
                }

                if (loserCanBeRemoved)
                {
                    players.Remove(playerToRemove);
                }
            }
        }

        private bool NobodyCanMakeTurn()
        {
            foreach (Player player in players)
            {
                if (!player.DontTurn)
                {
                    return false;
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

                if (cardFromDeck != null)
                {
                    player.AddCards(new List<Card>() { cardFromDeck });
                }

                return true;
            }            
            else if (!player.CanMakeTurn)
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
            
            if (card.nominal == Nominal.Jack)
            {
                players[GetNextPlayerIndex(playerIndex)].CanMakeTurn = false;
            }

            cardsPile.PushCard(card);
            player.DontTurn = false;

            return true;
        }

        private void OnPlayerMissingCards(Player player)
        {
            Card lastPileCard = cardsPile.PeekCard();

            if (cardsPile.IsIncrementing && !player.
                ContainsCard(card => (card.nominal == lastPileCard.nominal + 1) && (lastPileCard.suit == card.suit)))
            {
                Debug.Log($"{player.name} doesn't have any card to increment pile!");

                for (var i = 0; i < (int)lastPileCard.nominal && (cardsDeck.CardsLeft != 0); i++)
                {
                    player.AddCards(new List<Card>() { cardsDeck.GetCard() });
                    
                    Debug.Log($"{player.name} takes cards");
                }

                cardsPile.IsIncrementing = false;
            }
            else if (cardsDeck.CardsLeft != 0)
            {
                Debug.Log($"{player.name} takes cards");
                player.AddCards(new List<Card>() { cardsDeck.GetCard() });

                return;
            }

            player.DontTurn = true;
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
