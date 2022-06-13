namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LastCard.Logic;
    using UnityEngine;

    public abstract class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();
        protected CardsDeck deck;
        protected RulesResolver resolver;


        [SerializeField]
        private Transform cardsHolder;

        public event Action<Player, Card> OnCardSelected;
        public event Action<Player> OnCardsMissing;
        public bool CanMakeTurn { get; set; } = true;
        
        public virtual void AddCards(List<Card> additionalCards)
        {
            cards.AddRange(additionalCards);
            // Place cards?
            foreach (Card card in additionalCards)
            {
                card.transform.SetParent(cardsHolder, false);
            }
        }

        public void Init(RulesResolver rulesResolver, CardsDeck cardsDeck)
        {
            resolver = rulesResolver;
            deck = cardsDeck;
        }

        public int GetPointsNumber()
        {
            int result = 0;

            foreach (Card card in cards)
            {
                result += (int)card.nominal;
            }

            return result;
        }

        public int GetCardsCount()
        {
            return cards.Count;
        }

        public virtual void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        protected void SendCardSelected(Card card)
        {
            OnCardSelected?.Invoke(this, card);
        }

        protected void TakeCard()
        {
            OnCardsMissing?.Invoke(this);
        }

        public abstract Task MakeTurn();
    }
}
