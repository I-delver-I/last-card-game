namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;

    public class BotPlayer : Player
    {
        public override void EndTurn()
        {
            //throw new System.NotImplementedException();
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

            TakeCards();
            
            return Task.CompletedTask;
        }
    }
}
