namespace LastCard.Logic
{
    using UnityEngine;

    public class RulesResolver : MonoBehaviour
    {
        private CardsPile cardsPile;
        private CardsDeck cardsDeck;

        public void Init(CardsPile pile, CardsDeck deck)
        {
            cardsPile = pile;
            cardsDeck = deck;
        }

        public bool CanPushCard(Card card)
        {
            if (card == null)
            {
                return false;
            }

            if (cardsPile.IsIncrementing)
            {
                Card pileCard = cardsPile.PeekCard();

                if ((pileCard.nominal != Nominal.Ten) 
                    && (card.nominal == pileCard.nominal + 1) && (card.suit == pileCard.suit))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if ((card.nominal == Nominal.Eight) || cardsPile.HasAliasThree
                || (card.nominal == Nominal.Four && CanPushFour()) || FollowsBaseCondition(card))
            {
                return true;
            }

            return false;
        }

        private bool CanPushFour()
        {
            Nominal lastPileNominal = cardsPile.PeekCard().nominal;

            if (lastPileNominal == Nominal.Five || lastPileNominal == Nominal.Six || lastPileNominal == Nominal.Seven)
            {
                return true;
            }

            return false;
        }

        private bool FollowsBaseCondition(Card card)
        {
            return (card.suit == cardsPile.PeekCard().suit) || (card.nominal == cardsPile.PeekCard().nominal);
        }
    }
}
