namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;

    public class BotPlayer : Player
    {

        private RulesResolver resolver;
        
        public void Init(RulesResolver rulesResolver)
        {
            resolver = rulesResolver;
        }
        
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
            
            return Task.CompletedTask;
        }
    }
}
