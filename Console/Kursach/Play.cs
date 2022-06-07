namespace Kursach
{
    public class Play
    {
        public static Player p1;
        public static SimulatedPlayer p2;

        public void Run()
        {
            // try 
            // {
                play();
            // }
            // catch (Exception e){
            //     e.printStackTrace();
            // }
        }

        public void play()
        {
            CardDeck deck = new CardDeck();
            CardPile pile = new CardPile();
            Player.NumberOfPlayers = 2;
            //listOfPlayers = new Player[Player.numberOfPlayers];

            deck.ShuffleCards();
            /*
            for (int i = 0; i < Player.numberOfPlayers; i++) {
                if (i == 0)
                    listOfPlayers[i] = new Player("Yezy", 4, pile);
                else listOfPlayers[i] = new SimulatedPlayer("Ilomo", 4, pile);
                listOfPlayers[0].usedDeck = deck;
                listOfPlayers[i].dealCards();
            }
            */

            p1 = new Player("Yezy", pile);
            p2 = new SimulatedPlayer("SimulatedPlayer", pile);
            Player.UsedDeck=deck;
            p1.DealCards();
            p2.DealCards();

            //while (true) {
                //Player.nextPlayer = Player.nextPlayer % Player.numberOfPlayers;
                //listOfPlayers[Player.nextPlayer].showHandCards();

                p1.ShowHandCards();
                //p2.playCard();

            //}
        }
    }
}