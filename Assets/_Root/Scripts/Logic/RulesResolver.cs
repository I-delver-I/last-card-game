namespace LastCard.Logic
{
    using UnityEngine;

    public class RulesResolver : MonoBehaviour
    {
        private CardsPile cardsPile;

        public void Init(CardsPile pile)
        {
            cardsPile = pile;
        }

        public bool CanPushCard(Card card)
        {
            if (card == null)
            {
                return false;
            }

            if (cardsPile.HasAliasThree || (card.nominal == Nominal.Eight) 
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
