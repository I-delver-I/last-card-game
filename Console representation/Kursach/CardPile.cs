namespace Kursach
{
    public class CardPile
    {
        public Stack<Card> Pile { get; set; } = new Stack<Card>();
        
        public void ShufflePileCards(CardDeck usedDeck)
        {
            Card top = Pile.Pop();
            usedDeck.Deck = Pile;
            Pile = new Stack<Card>();
            Pile.Push(top);

            Card[] cardArray = new Card[Pile.Count];
            Random random = new Random();

            for (var i = 1; i <= Pile.Count; i++)     
            {
                int randomCard = random.Next(Pile.Count);

                while (cardArray[randomCard] != null)
                {
                    randomCard = random.Next(Pile.Count);
                }

                cardArray[randomCard] = usedDeck.Deck.Pop();
            }

            for (var i = 0; i < Pile.Count; i++)
            {
                usedDeck.Deck.Push(cardArray[i]);
            }
        }
    }
}