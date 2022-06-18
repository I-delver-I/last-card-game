namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controls;
    using UnityEngine;
    using UnityEngine.UI;

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
                if (SendCardSelected(selectedCard))
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
                int cardWidth = 150;
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

            turnTcs.TrySetResult(true);
        }

        private void OnDestroy()
        {
            cardsSelector.OnCardSelected -= OnCardTap;
        }
    }
}
