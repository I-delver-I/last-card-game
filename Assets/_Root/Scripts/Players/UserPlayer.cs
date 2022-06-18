namespace LastCard
{
    using System;
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
                bool isCardPushed = SendCardSelected(selectedCard);

                if (isCardPushed)
                {
                    if (selectedCard.nominal == Nominal.Eight)
                    {
                        // Announce new suit
                        System.Random random = new System.Random();
                        selectedCard.suit = (Suit)random.Next(1, 4);
                        EndTurn();
                    }
                    else if (selectedCard.nominal == Nominal.Ace)
                    {
                        pile.Reversed = !pile.Reversed;
                        EndTurn();
                    }
                    else if (selectedCard.nominal != Nominal.Three)
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
            foreach (Card card in additionalCards)
            {
                if (card == null)
                {
                    return;
                }
            }

            if (cards.Count != 0)
            {
                RectTransform hlgRect = (RectTransform)hlg.transform;
                RectTransform cardRect = (RectTransform)cards.FirstOrDefault().transform;
                hlg.spacing = (hlgRect.rect.width - cardRect.rect.width * cards.Count) / (cards.Count - 1);
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
