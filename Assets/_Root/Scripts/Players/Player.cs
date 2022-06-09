namespace LastCard
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();

        public virtual void AddCards(List<Card> additionalCards)
        {
            cards.AddRange(additionalCards);
            // Place cards?
            foreach (Card card in cards)
            {
                Card newCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.transform.SetParent(transform, false);
            }
        }
    }
}
