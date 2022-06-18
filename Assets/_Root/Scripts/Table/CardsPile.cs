namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public class CardsPile : MonoBehaviour
    {
        [SerializeField]
        private Transform cardsHolder;

        private List<Card> cards = new List<Card>();
        public bool IsIncrementing { get; set; } = false;
        public bool HasAliasThree { get; set; } = false;
        public bool SkipTurn { get; set; } = false;
        //public bool IsChangingSuit { get; set; } = false;
        public bool Reversed { get; set; } = false;
        
        public Card PeekCard()
        {
            if (cards.Count - 1 < 0)
            {
                return null;
            }

            return cards.Last();
        }

        public void PushCard(Card card)
        {
            Debug.Log(card.name);

            if (HasAliasThree)
            {
                HasAliasThree = false;
            }

            switch (card.nominal)
            {
                case Nominal.Four:
                    IsIncrementing = true;
                    break;
                case Nominal.Two:
                    SkipTurn = true;
                    break;
                case Nominal.Three:
                    HasAliasThree = true;
                    break;
                case Nominal.Ace:
                    Reversed = !Reversed;
                    break;
                case Nominal.Eight:
                    ChangeCardSuit();
                    break;
                case Nominal.Ten:
                    IsIncrementing = false;
                    break;
            }
            

            // if (card.nominal == Nominal.Four)
            // {
            //     IsIncrementing = true;
            // }
            // else if (card.nominal == Nominal.Two)
            // {
            //     SkipTurn = true;
            // }
            // else if (card.nominal == Nominal.Three)
            // {
            //     HasAliasThree = true;
            // }
            // else if (card.nominal == Nominal.Ace)
            // {
            //     Reversed = !Reversed;
            // }
            // else if (card.nominal == Nominal.Eight)
            // {
            //     ChangeCardSuit();
            // }

            cards.Add(card);
            card.transform.SetParent(cardsHolder.transform, false);
        }

        public void ChangeCardSuit()
        {
            System.Random random = new System.Random();
            PeekCard().suit = (Suit)random.Next(1, 4);
        }
    }
}
