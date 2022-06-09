namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();

        [SerializeField]
        private Transform cardsHolder;

        public virtual void AddCards(List<Card> additionalCards)
        {
            cards.AddRange(additionalCards);
            // Place cards?
            foreach (Card card in additionalCards)
            {
                card.transform.SetParent(cardsHolder, false);
            }
        }
    }
}
