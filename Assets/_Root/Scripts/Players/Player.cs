namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();

        public virtual void AddCards(List<Card> additionalCards)
        {
            cards.AddRange(additionalCards);
            // Place cards?
            
        }
    }
}
