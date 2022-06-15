namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public class CardsPile : MonoBehaviour
    {
        public Transform cardsHolder;

        private List<Card> cards = new List<Card>();
        public bool IsIncrementing { get; set; } = false;
        public bool HasAliasThree { get; set; } = false;
        
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
            card.transform.SetParent(cardsHolder.transform, false);

            if (card.nominal == Nominal.Four)
            {
                IsIncrementing = true;
            }
            else if (card.nominal == Nominal.Three)
            {
                HasAliasThree = true;
            }
        }

        public void ChangeCardSuit()
        {   
            PeekCard().suit = (Suit)(((int)PeekCard().suit + 1) % 4);
        }
    }
}
