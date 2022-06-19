namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using System.Linq;
    using System;
    using UnityEngine.UI;

    public class BotPlayer : Player
    {
        public Outline border;
        public Text cardsCount;

        // public override void EndTurn()
        // {
        //     // Thread.Sleep(1500);
        // }

        public override void AddCards(List<Card> additionalCards)
        {
            base.AddCards(additionalCards);

            cardsCount.text = cards.Count.ToString();
        }

        public override async Task MakeTurn()
        {
            Debug.Log("Bot turn");
            
            bool turnIsMade = false;
            border.enabled = true;
            Task turnDuration = Task.Delay(TimeSpan.FromSeconds(2));
            await turnDuration;
            border.enabled = false;

            List<Card> tempCards = new List<Card>(cards);

            while (tempCards.Count != 0)
            {
                Card maxCard = GetMaximalCard(tempCards);
                tempCards.Remove(maxCard);

                if (SendCardSelected(maxCard))
                {
                    maxCard.flipper.Flip();
                    RemoveCard(maxCard);

                    // if (maxCard.nominal == Nominal.Three)
                    // {
                    //     cardToPush = GetCardToPush();
                    //     SendCardSelected(cardToPush);
                    //     RemoveCard(cardToPush);

                    //     // foreach (Card card in cards)
                    //     // {
                    //     //     if (SendCardSelected(card))
                    //     //     {
                    //     //         card.flipper.Flip();

                    //     //         await MakeTurn();
                    //     //     }
                    //     // }
                    // }
                    // else
                    // {
                    //     RemoveCard(cardToPush);
                    // }

                    cardsCount.text = cards.Count.ToString();
                    turnIsMade = true;
                    await Task.CompletedTask;
                    break;
                }
            }

            if (!turnIsMade)
            {
                Debug.Log("Bot takes card");
                TakeCards();
                cardsCount.text = cards.Count.ToString();
            }

            await Task.CompletedTask;
        }

        public Card GetCardToPush()
        {
            foreach (Card card in cards)
            {
                if (resolver.CanPushCard(card))
                {
                    card.flipper.Flip();
                    
                    return card;
                }
            }

            return null;
        }

        private Card GetMaximalCard(List<Card> botCards)
        {
            return botCards.Find(card => card.nominal == botCards.Max(card => card.nominal));
        }
    }
}
