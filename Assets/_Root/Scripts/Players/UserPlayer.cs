namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controls;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Linq;

    public class UserPlayer : Player
    {
        [SerializeField]
        private TapCardsSelector cardsSelector;

        private TaskCompletionSource<bool> turnTcs;

        public Button endTurnButton;
        public HorizontalLayoutGroup hlg;

        private void Awake()
        {
            cardsSelector.OnCardSelected += OnCardTap;
        }

        private void OnCardTap(Card selectedCard)
        {
            if (turnTcs == null || turnTcs.Task.IsCompleted)
            {
                return;
            }

            if (cards.Contains(selectedCard))
            {
                bool canSendCard = SendCardSelected(selectedCard);
                Debug.Log($"Can send card: {canSendCard}");

                if (canSendCard)
                {
                    // if (selectedCard.nominal == Nominal.Eight)
                    // {
                    //     EndTurn();
                    // }
                    // else if (selectedCard.nominal == Nominal.Ace)
                    // {
                    //     EndTurn();
                    // }
                    
                    if (selectedCard.nominal != Nominal.Three)
                    {
                        EndTurn();
                    }
                }
            }
            else if (deck.ContainsCard(selectedCard))
            {
                TakeCards();
                EndTurn();
            }
            else
            {
                return;
            }
        }

        public override void AddCards(List<Card> additionalCards)
        {
            // foreach (Card card in additionalCards)
            // {
            //     if (card == null)
            //     {
            //         return;
            //     }
            // }

            if (cards.Count != 0)
            {
                int hlgWidth = 1300;
                hlgWidth = (int)hlg.transform.localScale.x;
                int cardWidth = 150;
                cardWidth = (int)additionalCards.FirstOrDefault().transform.localScale.x;

                hlg.spacing = (hlgWidth - cardWidth * cards.Count) / (cards.Count - 1);
            }

            foreach (Card card in additionalCards)
            {
                card.flipper.Flip();
            }

            base.AddCards(additionalCards);
        }

        public override Task MakeTurn()
        {
            Debug.Log("User turn");
            turnTcs = new TaskCompletionSource<bool>();
            
            return turnTcs.Task;
        }

        public override void EndTurn()
        {
            if (turnTcs == null && (pile.PeekCard() != null))
            {
                return;
            }

            TakeCards();

            turnTcs.TrySetResult(true);
        }

        public void ForceEndTurn()
        {
            if (deck.CardsLeft == 0)
            {
                EndTurn();
            }
        }

        private void OnDestroy()
        {
            cardsSelector.OnCardSelected -= OnCardTap;
        }
    }
}
