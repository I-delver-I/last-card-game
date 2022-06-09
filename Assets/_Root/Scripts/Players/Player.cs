namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

    public abstract class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();

        [SerializeField]
        private Transform cardsHolder;

        public event Action<Card> OnCardSelected;
        
        public virtual void AddCards(List<Card> additionalCards)
        {
            cards.AddRange(additionalCards);
            // Place cards?
            foreach (Card card in additionalCards)
            {
                card.transform.SetParent(cardsHolder, false);
            }
        }

        protected void SendCardSelected(Card card)
        {
            OnCardSelected?.Invoke(card);
        }

        public abstract Task MakeTurn();
    }
}
