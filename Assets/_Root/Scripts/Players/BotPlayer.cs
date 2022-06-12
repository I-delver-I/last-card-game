namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

    public class BotPlayer : Player
    {
        public override void AddCards(List<Card> additionalCards)
        {
            base.AddCards(additionalCards);

            foreach (Card card in cards)
            {
                card.flipper.Flip();
            }
        }

        public override Task MakeTurn()
        {
            Debug.Log("Bot turn");
            return Task.CompletedTask;
        }
    }
}
