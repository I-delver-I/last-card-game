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
                    RemoveCard(selectedCard);

                    if (selectedCard.nominal != Nominal.Three)
                    {
                        EndTurn();
                    }
                }
            }
        }

        public override void AddCards(List<Card> cardsToAdd)
        {
            if (cardsToAdd.Count == 0)
            {
                return;
            }
                
            int hlgWidth = (int)hlg.transform.localScale.x;
            int cardWidth = (int)cardsToAdd.FirstOrDefault().transform.localScale.x;
            hlg.spacing = (hlgWidth - cardWidth * cards.Count) / (cards.Count - 1);

            foreach (Card card in cardsToAdd)
            {
                card.flipper.Flip();
            }

            base.AddCards(cardsToAdd);
        }

        public override Task MakeTurn()
        {
            Debug.Log("User turn");
            turnTcs = new TaskCompletionSource<bool>();
            
            return turnTcs.Task;
        }

        public void EndTurn()
        {
            if (turnTcs == null && (pile.PeekCard() != null))
            {
                return;
            }
            Debug.Log($"Cards count - {GetCardsCount()}");
            turnTcs.TrySetResult(true);
        }

        public void PassTurn()
        {
            TakeCards();
            EndTurn();
        }

        private void OnDestroy()
        {
            cardsSelector.OnCardSelected -= OnCardTap;
        }
    }
}
