namespace LastCard.Logic
{
    using UnityEngine;

    public class RulesResolver : MonoBehaviour
    {
        [SerializeField]
        private CardsPile cardsPile;

        private bool pushedThree = false;
        private bool isIncrementing = false;

        public bool CanPushCard(Card card) => FollowsBaseRule(card) || IsEight(card.nominal) || pushedThree
            || (isIncrementing && (card.nominal == cardsPile.PeekCard.nominal + 1));
            

        private bool FollowsBaseRule(Card card)
        {
            return (card.suit == cardsPile.PeekCard.suit) || (card.nominal == cardsPile.PeekCard.nominal);
        }

        private bool IsThree(Nominal nominal)
        {
            if (pushedThree)
            {
                pushedThree = false;

                return true;
            }

            return false;
        }

        private bool IsEight(Nominal nominal)
        {
            return nominal == Nominal.Eight;
        }
    }
}
