using System.Collections.Generic;
using UnityEngine;

namespace LastCard
{
    public class UserPlayer : Player
    {
        public override void AddCards(List<Card> additionalCards)
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
