namespace Kursach
{
    public class Player
    {
        protected static int HandSize { get; set; }
        public static CardDeck UsedDeck { get; set; }
        public static CardPile UsedPile { get; set; }
        public static int NumberOfPlayers { get; set; } = 0;
        public static int NextPlayer { get; set; } = 0;
        protected static int Turn { get; set; } = 1;
        public static bool Semaphore { get; set; } = false;
        public static Stack<Card> SpecialCardContainer { get; set; } = new Stack<Card>();
        public LinkedList<Card> CardsOnHand { get; set; }
        public string PlayerName { get; }
        
        public Player(string playerName, CardPile usedPile, int handSize = 7)
        {
            PlayerName = playerName;
            Player.HandSize = handSize;
            Player.UsedPile = usedPile;
        }

        public void DealCards()
        {
            CardsOnHand = new LinkedList<Card>();

            for (var i = 0; i < Player.HandSize; i++)
            {
                DrawCard();
            }
        }

        public Card DrawCard()
        {
            if (UsedDeck.Deck.Count == 0)
            {
                UsedPile.ShufflePileCards(UsedDeck);
            }

            Card drawnCard = UsedDeck.Deck.Pop();
            CardsOnHand.AddLast(drawnCard);

            return drawnCard;
        }

        public void ShowHandCards()
        {
            throw new NotImplementedException();
        }

        public void ThrowCard(Card cd)
        {
            throw new NotImplementedException();
        }

        public void UpdateTemp()
        {
            if (UsedDeck.Deck.Count < 2)
            {
                UsedPile.ShufflePileCards(UsedDeck);
            }

            for (var i = 0; i < 2; i++)
            {
                SpecialCardContainer.Push(UsedDeck.Deck.Pop());
            }
        }

        public void Penalt(int pv)
        {
            System.Console.WriteLine("\nOoooops! Wrong card");

            for (var i = 0; i < pv; i++)
            {
                DrawCard();
            }
        }

        public void PlayCard(Card plc)
        {
            if (UsedPile.Pile.Count != 0)
            {
                if((UsedPile.Pile.Peek().Name == "7") && Semaphore && (plc.Name == "7"))
                {
                    Semaphore = false;
                    NextPlayer += Turn;
                    return;
                }

                if(((UsedPile.Pile.Peek().Name == "2") && !(SpecialCardContainer.Count == 0)) && (plc.Name == "2"))
                {
                    /*  for(Card c:specialCardContainer){
                            this.cardsOnHand.push(c);
                        }
                        specialCardContainer=new Stack<Card>();
                    */

                    ThrowCard(plc);
                    UpdateTemp();
                    NextPlayer += Turn;
                    return;
                }

                if ((UsedPile.Pile.Peek().Name == "8") && Semaphore && plc.Name == "8")
                {

                        Semaphore = false;
                        Turn *= -1;
                        NextPlayer += Turn;

                        if (NextPlayer == 0) 
                        {
                            if (NumberOfPlayers == 2) 
                            {
                                NextPlayer = 0;
                                Turn = 1;
                                return;
                            }
                            else
                            {
                                NextPlayer = NumberOfPlayers - 1;
                            }

                            return;
                        }
                        else if (NextPlayer == -1) 
                        {
                            if (NumberOfPlayers == 2)
                            {
                                NextPlayer = 1; 
                                Turn = 1;
                                return;
                            }
                            else
                            {
                                NextPlayer = NumberOfPlayers - 2;
                            }

                            return;
                        }
                        else
                        {
                            NextPlayer += Turn;
                        }

                        return;
                }
            }
            
            String cardName = plc.Name;
            String cardSuit = plc.Suit;

            for (var i = 0; i < CardsOnHand.Count; i++)
            {
                if ((CardsOnHand.ElementAt(i).Name == cardName) && (CardsOnHand.ElementAt(i).Suit == cardSuit))
                {
                    Card card = CardsOnHand.ElementAt(i);

                    if ((UsedPile.Pile.Count == 0) || (card.Name == UsedPile.Pile.Peek().Name) 
                        || (card.Suit == UsedPile.Pile.Peek().Suit) || (card.Name == "J") 
                        || (UsedPile.Pile.Peek().Name == "J"))
                    {
                        if (UsedPile.Pile.Count != 0)
                        {
                            if (card.Name == "7" && !Semaphore)
                            {
                                Semaphore = true;
                                ThrowCard(card);          //Playing 7 for the first time
                                NextPlayer += Turn;
                                return;
                            }
                            else if ((UsedPile.Pile.Peek().Name == "7") && (card.Name != "7") && Semaphore)
                            {
                                Semaphore = false;
                                Penalt(2);           //Responding wrong card instead of 7
                                NextPlayer += Turn;
                                return;
                            }
                            else if((UsedPile.Pile.Peek().Name == "7") && (card.Name == "7") && Semaphore)
                            {
                                ThrowCard(card);    //Responding to played 7
                                NextPlayer += Turn;
                                return;
                            }

                            if(!Semaphore && card.Name == "8")
                            {
                                Semaphore = true;
                                ThrowCard(card);               //FIRST TO PLAY 8
                                NextPlayer += Turn;
                                return;
                            }
                            else if((UsedPile.Pile.Peek().Name == "8") && Semaphore && (card.Name == "8"))
                            {
                                Semaphore = false;
                                ThrowCard(card);              //RESPONDING TO PLAYED 8
                                NextPlayer += Turn;
                                return;
                            }
                            else if((UsedPile.Pile.Peek().Name == "8") && Semaphore && !(card.Name == "8"))
                            {
                                Semaphore = false;
                                Penalt(2);                    //HAVE'NT PLAYED 8
                                Turn *= -1;
                                NextPlayer += Turn;

                                if (NextPlayer == 0) 
                                {
                                    if (NumberOfPlayers == 2)
                                    { 
                                        NextPlayer = 0; 
                                        Turn = 1; 
                                        return;
                                    }
                                    else
                                    {
                                        NextPlayer = NumberOfPlayers - 1;
                                    }

                                    return;
                                }
                                else if (NextPlayer == -1) 
                                {
                                    if (NumberOfPlayers == 2) 
                                    {
                                        NumberOfPlayers = 1; 
                                        Turn=1; 
                                        return;
                                    }
                                    else
                                    {
                                        NextPlayer = NumberOfPlayers - 2;
                                    }

                                    return;
                                }
                                else
                                {
                                    NextPlayer += Turn;
                                }

                                return;
                            }

                            if ((card.Name == "2") && (SpecialCardContainer.Count == 0 
                                || SpecialCardContainer.Count != 0))
                            {
                                ThrowCard(card);
                                UpdateTemp();               //Playing 2 for the first time or respondig to the played 2
                                NextPlayer += Turn;
                                return;
                            }
                            else if ((UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0) 
                                && (card.Name != "2"))
                            {
                                Penalt(2);

                                foreach (Card c in SpecialCardContainer)
                                {        //Played wrong card instead of 2
                                    CardsOnHand.AddLast(c);               //to be examined later
                                }
                            
                                SpecialCardContainer = new Stack<Card>();
                                NextPlayer += Turn;
                                return;
                            }

                            ThrowCard(card);
                            NextPlayer += Turn;
                            return;
                        }
                    }
                    else 
                    {
                        if ((UsedPile.Pile.Count != 0) && (UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0))
                        {
                            foreach (Card c in SpecialCardContainer)
                            {             //Played wrong card instead of 2
                                CardsOnHand.AddLast(c);                    //to be examined later
                            }
                            
                            SpecialCardContainer = new Stack<Card>();
                        }

                        Semaphore = false;
                        Penalt(2);
                        NextPlayer += Turn;
                        return;
                    }

                }
                else if ((i + 1) == CardsOnHand.Count)
                {
                    if((UsedPile.Pile.Count != 0) && (UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0))
                    {
                        foreach (Card c in SpecialCardContainer)
                        {             //Played wrong card instead of 2
                            CardsOnHand.AddLast(c);                   //to be examined later
                        }
                        
                        SpecialCardContainer = new Stack<Card>();
                    }

                    Semaphore = false;
                    Penalt(2);
                    NextPlayer += Turn;
                    return;
                }
            }
        }
    }
}