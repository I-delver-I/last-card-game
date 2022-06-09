using System.Collections.Generic;
using UnityEngine;

namespace LastCard
{
    public class UserPlayer : Player
    {
        public override void AddCards(List<Card> additionalCards)
        {
            base.AddCards(additionalCards);

            foreach (var card in cards)
            {
                Card newCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.transform.SetParent(GameObject.Find("UserPlayer").transform, false);
            }
        }
    }
}
