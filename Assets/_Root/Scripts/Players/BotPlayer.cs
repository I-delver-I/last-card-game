namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;

    public class BotPlayer : Player
    {
        public override void AddCards(List<Card> additionalCards)
        {
            foreach (Card card in additionalCards)
            {
                card.flipper.Flip();
            }

            base.AddCards(additionalCards);
        }

        public override Task MakeTurn()
        {
            Debug.Log(name);
            
            foreach (Card card in cards)
            {
                if (resolver.CanPushCard(card))
                {
                    card.flipper.Flip();
                    SendCardSelected(card);
                    
                    return Task.CompletedTask;
                }
            }

            Card newCard = deck.GetCard();
            
            if (newCard != null)
            {
                AddCards(new List<Card>() { newCard });
            }

            return Task.CompletedTask;
        }
    }
}
