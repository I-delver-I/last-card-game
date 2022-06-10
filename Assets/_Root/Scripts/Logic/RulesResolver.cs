namespace LastCard.Logic
{
    using UnityEngine;

    public class RulesResolver : MonoBehaviour
    {
        [SerializeField]
        private CardsPile cardsPile;

        public bool CanPushCard(Card card)
        {
            var canPush = card.suit == cardsPile.PeekCard.suit || card.nominal == cardsPile.PeekCard.nominal;
            return canPush;
        }
    }
}
