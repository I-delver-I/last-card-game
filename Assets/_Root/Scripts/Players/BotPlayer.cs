namespace LastCard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BotPlayer : Player
    {
        public override void AddCards(List<Card> additionalCards)
        {
            base.AddCards(additionalCards);

            foreach (Card card in cards)
            {
                card.GetComponent<CardFlipper>().Flip();
            }
        }

        public override Task MakeTurn()
        {
            return Task.CompletedTask;
        }
    }
}
