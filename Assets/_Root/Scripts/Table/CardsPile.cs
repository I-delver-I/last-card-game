namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public class CardsPile : MonoBehaviour
    {
        private List<Card> cards = new List<Card>();

        public Card PeekCard()
        {
            if (cards.Count - 1 < 0)
            {
                return null;
            }

            return cards[cards.Count - 1];
        }

        public void PushCard(Card card)
        {
            cards.Add(card);
            card.transform.SetParent(transform, false);
        }
    }
}
