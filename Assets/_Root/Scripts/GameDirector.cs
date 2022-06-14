namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System;
    using System.Threading;
    using UnityEngine.UI;

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

        [SerializeField]
        private Background background;

        [SerializeField]
        private Text winText;

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
            var playerIndex = GetStartPlayerIndex();
            
            while (players.Count != 1) // Not game is completed
            {
                Player player = players[playerIndex];

                if (player.CanMakeTurn) // if user can make a turn
                {
                    player.OnCardSelected += OnPlayerSelectedCard;
                    player.OnCardsMissing += OnPlayerMissingCards;

                    ProcessSkippingTurn(player, playerIndex);
                    Task turnTask = player.MakeTurn();
                    await turnTask;

                    player.OnCardSelected -= OnPlayerSelectedCard;
                    player.OnCardsMissing -= OnPlayerMissingCards;

                    if (player.GetCardsCount() == 0)
                    {
                        AnnounceWinner(player);
                    }
                    else if (cardsPile == null)
                    {
                        AnnounceWinner(players);
                    }
                }

                if (!GameCanContinue())
                {
                    AnnounceWinner(players);
                }

                playerIndex = GetNextPlayerIndex(playerIndex);
            }
        }

        private bool GameCanContinue()
        {
            foreach (Player player in players)
            {
                if (player.CanMakeTurn)
                {
                    return true;
                }
            }

            return false;
        }

        private void AnnounceWinner(List<Player> players)
        {
            Player winner = players[0];
            bool twoMorePlayersHaveEqualPoints = false;

            for (var i = 1; i < players.Count; i++)
            {
                if (players[i].GetPointsNumber() < winner.GetPointsNumber())
                {
                    winner = players[i];
                }
                else if (players[i].GetPointsNumber() < winner.GetPointsNumber())
                {
                    twoMorePlayersHaveEqualPoints = true;
                }
            }

            if (!twoMorePlayersHaveEqualPoints)
            {
                AnnounceWinner(winner);
            }
        }

        private void AnnounceWinner(Player winner)
        {
            background.ShowWinMessage(winner);
            players.Remove(winner);
        }

        private void ProcessSkippingTurn(Player player, int playerIndex)
        {
            if (cardsPile.PeekCard().nominal == Nominal.Two)
            {
                player.AddCards(new List<Card>() { cardsDeck.GetCard() });
                player.EndTurn();
            }
            else if (!player.CanMakeTurn)
            {
                player.CanMakeTurn = true;
                player.EndTurn();
            }
            else if (cardsPile.PeekCard().nominal == Nominal.Jack)
            {
                players[GetNextPlayerIndex(playerIndex)].CanMakeTurn = false;
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

        private int GetNextPlayerIndexReverse(int index)
        {
            return Math.Abs(index - 1) % players.Count;
        } 
    }
}
