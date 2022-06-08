namespace Kursach
{
    public class SimulatedPlayer : Player
    {
        public SimulatedPlayer(String name, CardPile UsedPile, int handSize = 7) : base(name, UsedPile, handSize)
        {
            
        }

        public void showHandCards()
        {

        }

        public bool Pass()
        {
            if (CompareSpecial(UsedPile.Pile.Peek()))
            {
                return true;
            }

            return false;
        }

        public bool CompareSpecial(Card cardOnPile)
        {
            foreach (Card i in CardsOnHand)
            {
                if (cardOnPile.Name == i.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompareToPlay(Card cardOnPile)
        {
            foreach (Card i in CardsOnHand)
            {
                if ((cardOnPile.Name == i.Name) || (cardOnPile.Suit == i.Suit))
                {
                    return true;
                }
            }

            return false;
        }

        public Card CardToPlay(Card cardOnPile)
        {
            foreach (Card i in CardsOnHand)
            {
                if ((cardOnPile.Name == i.Name) || (cardOnPile.Suit == i.Suit))
                {
                    return i;
                }
            }

            return null;
        }

        public Card SpecialCardToPlay(Card cardOnPile)
        {
            foreach (Card i in CardsOnHand)
            {
                if ((cardOnPile.Name == i.Name))
                {
                    return i;
                }
            }

            return null;
        }

        public void PlayCard()
        {
            if (UsedPile.Pile.Count != 0)
            {
                if ((UsedPile.Pile.Peek().Name == "7") && Semaphore)
                {
                    if (Pass())
                    {
                        Semaphore=false; 
                        NextPlayer += Turn;
                        return;
                    }
                }

                if ((UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0))
                {
                    if (Pass())
                    {
                        foreach (Card c in SpecialCardContainer)
                        {
                            CardsOnHand.AddLast(c);
                        }
                    
                        SpecialCardContainer = new Stack<Card>();
                        NextPlayer += Turn;
                        return;
                    }
                }

                if ((UsedPile.Pile.Peek().Name == "8") && Semaphore)
                {
                    if (Pass())
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
                                NextPlayer=NumberOfPlayers-1;
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
                                NextPlayer=NumberOfPlayers-2;
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
            }

            char m = 'n';

            if ((SpecialCardContainer.Count == 0) && !Semaphore)
            { //For non special cards
                if ((UsedPile.Pile.Count == 0) || CompareToPlay(UsedPile.Pile.Peek()))
                {
                    m = 'N';
                }
                else
                {
                    m = 'D';
                }
            }
            else if (CompareSpecial(UsedPile.Pile.Peek()))
            { //For Special cards
                m = 'S';
            }
            else
            {
                m = 'P';
            }

            if (m == 'D')
            {
                DrawCard(); 
                NextPlayer += Turn;
                return;
            }

            if (m == 'P')
            {
                if (UsedPile.Pile.Peek().Name == "2")
                {
                    foreach (Card c in SpecialCardContainer)
                    {
                        CardsOnHand.AddLast(c);
                    }

                    SpecialCardContainer = new Stack<Card>();
                    NextPlayer += Turn;
                    return;
                }

                Semaphore = false;
                NextPlayer += Turn;
                return;
            }
            else
            {
                String cardName;
                String cardSuit;

                if ((SpecialCardContainer.Count != 0) || Semaphore)
                {
                    if (UsedPile.Pile.Count == 0)
                    {
                        cardName = CardsOnHand.ElementAt(0).Name;
                        cardSuit = CardsOnHand.ElementAt(0).Suit;
                    }
                    else
                    {
                        cardName = SpecialCardToPlay(UsedPile.Pile.Peek()).Name;
                        cardSuit = SpecialCardToPlay(UsedPile.Pile.Peek()).Suit;
                    }
                }
                else
                {
                    if (UsedPile.Pile.Count == 0)
                    {
                        cardName = CardsOnHand.ElementAt(0).Name;
                        cardSuit = CardsOnHand.ElementAt(0).Suit;
                    }
                    else
                    {
                        cardName = CardToPlay(UsedPile.Pile.Peek()).Name;
                        cardSuit = CardToPlay(UsedPile.Pile.Peek()).Suit;
                    }
                }

                for (int i = 0; i < CardsOnHand.Count; i++)
                {

                    if ((CardsOnHand.ElementAt(i).Name == cardName) && (CardsOnHand.ElementAt(i).Suit == cardSuit))
                    {
                        Card card=CardsOnHand.ElementAt(i);

                        if (UsedPile.Pile.Count == 0 || (card.Name == UsedPile.Pile.Peek().Name) || (card.Suit == UsedPile.Pile.Peek().Suit) || (card.Name == "J") || (UsedPile.Pile.Peek().Name == "J"))
                        {
                            if (UsedPile.Pile.Count != 0)
                            {
                                if ((card.Name == "7") && !Semaphore)
                                {
                                    Semaphore=true;
                                    ThrowCard(card);          //Playing 7 for the first time
                                    NextPlayer += Turn;
                                    return;
                                }
                                else if((UsedPile.Pile.Peek().Name == "7") && (card.Name != "7") && Semaphore)
                                {
                                    Semaphore=false;
                                    Penalt(2);           //Responding wrong card instead of 7
                                    NextPlayer += Turn;
                                    return;
                                }
                                else if((UsedPile.Pile.Peek().Name == "7") && (card.Name == "7") && Semaphore)
                                {
                                    //Semaphore=false;       we don't set it false until the neturnt Player plays
                                    ThrowCard(card);    //Responding 7 to
                                    NextPlayer += Turn;
                                    return;
                                }

                                if(!Semaphore && (card.Name == "8"))
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
                                else if((UsedPile.Pile.Peek().Name == "8") && Semaphore && (card.Name != "8"))
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
                                            NextPlayer=NumberOfPlayers-1;
                                        }

                                        return;
                                    }
                                    else if(NextPlayer == -1) 
                                    {
                                        if(NumberOfPlayers==2) 
                                        {
                                            NextPlayer=1; 
                                            Turn=1; 
                                            return;
                                        }
                                        else
                                        {
                                            NextPlayer = NumberOfPlayers-2;
                                        }

                                        return;
                                    }
                                    else
                                    {
                                        NextPlayer += Turn;
                                    }
                                    
                                    return;
                                }

                                if ((card.Name == "2") && (SpecialCardContainer.Count == 0) || (SpecialCardContainer.Count != 0))
                                {
                                    ThrowCard(card);
                                    UpdateTemp();               //Playing 2 for the first time or respondig to the UsedPileayed 2
                                    NextPlayer += Turn;
                                    return;
                                }
                            }

                            
                            else if ((UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0) && (card.Name != "2"))
                            {
                                //System.out.println("Ooooops! Wrong card");
                                Penalt(2);

                                foreach (Card c in SpecialCardContainer)
                                {             //Played wrong card instead of 2
                                    CardsOnHand.AddLast(c);    //to be examined later
                                }
                                
                                SpecialCardContainer=new Stack<Card>();
                                NextPlayer += Turn;
                                return;
                            }

                            ThrowCard(card);
                            NextPlayer += Turn;
                            return;
                        }
                        else 
                        {
                            if ((UsedPile.Pile.Count != 0) && (UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0))
                            {
                                foreach (Card c in SpecialCardContainer)
                                {             //Played wrong card instead of 2
                                    this.CardsOnHand.AddLast(c);    //to be examined later
                                }

                                SpecialCardContainer=new Stack<Card>();
                            }

                            //System.out.println("Ooooops! Wrong card");
                            Semaphore=false;
                            Penalt(2);
                            NextPlayer += Turn;
                            return;
                        }

                    }
                    else if ((i + 1) == CardsOnHand.Count)
                    {
                        if ((UsedPile.Pile.Count != 0) && (UsedPile.Pile.Peek().Name == "2") && (SpecialCardContainer.Count != 0))
                        {
                            foreach (Card c in SpecialCardContainer)
                            {             //Played wrong card instead of 2
                                this.CardsOnHand.AddLast(c);    //to be examined later
                            }

                            SpecialCardContainer=new Stack<Card>();
                        }

                        //System.out.println("Ooooops! Wrong card");
                        Semaphore = false;
                        Penalt(2);
                        NextPlayer += Turn;
                        return;
                    }
                }
            }
        }
    }
}