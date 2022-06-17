namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Logic;
    using UnityEngine;
    using System.Linq;
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
            List<Card> tempCards = new List<Card>(cards);

            while (tempCards.Count != 0)
            {
                Card maxCard = GetMaximalCard(tempCards);
                tempCards.Remove(maxCard);

                if (SendCardSelected(maxCard))
                {
                    maxCard.flipper.Flip();

                    if (maxCard.nominal == Nominal.Eight)
                    {
                        System.Random random = new System.Random();
                        maxCard.suit = (Suit)random.Next(1, 4);
                    }
                    else if (maxCard.nominal == Nominal.Ace)
                    {
                        pile.Reversed = !pile.Reversed;
                    }
                    else if (maxCard.nominal == Nominal.Three)
                    {
                        foreach (Card card in cards)
                        {
                            if (SendCardSelected(card))
                            {
                                card.flipper.Flip();

                                return MakeTurn();
                            }
                        }
                    }
                    else
                    {
                        return Task.CompletedTask;
                    }
                    
                }
            }

            TakeCards();
            
            return Task.CompletedTask;
        }

        private Card GetMaximalCard(List<Card> botCards)
        {
            // List<Nominal> priorityList = new List<Nominal>()
            // {
            //     Nominal.Eight, Nominal.Ace, Nominal.Four, Nominal.Two, Nominal.Eight, Nominal.Three
            // };

            // Dictionary<Nominal, int> priorityQueue = new Dictionary<Nominal, int>()
            // {
            //     (ace)
            // };

            return botCards.Find(card => card.nominal == botCards.Max(card => card.nominal));
        }
    }
}
