namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System;

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

        // private int GetPriority(Card card)
        // {
        //     List<Nominal> priorityList = new List<Nominal>()
        //     {
        //         Nominal.Eight, Nominal.Ace, Nominal.Four, Nominal.Two, Nominal.Eight, Nominal.Three
        //     };

        //     Dictionary<Nominal, int> priorityQueue = new Dictionary<Nominal, int>();
        //     priorityQueue.
        // }
    }
}
