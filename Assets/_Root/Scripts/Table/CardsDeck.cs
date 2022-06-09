namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CardsDeck : MonoBehaviour
    {
        [SerializeField]
        private int maxCardsAmount = 52;

        [SerializeField]
        private List<Card> cardsPrefabs;

        private List<Card> cards;

        public int CardsLeft => cards.Count;

        private void Awake()
        {
            cards = CreateCards();
        }

        private List<Card> CreateCards()
        {
            var result = new List<Card>();
            
            for (var i = 0; i < maxCardsAmount; i++)
            {
                var newCard = Instantiate(cardsPrefabs[i], this.transform);
                result.Add(newCard);
            }
            
            return result;
        }

        public List<Card> GetCards(int amount)
        {
            System.Random random = new System.Random();
            List<Card> result = new List<Card>();

            while (cards.Count != 0)
            {
                Card newCard = cards[random.Next(0, cards.Count)];
                result.Add(newCard);
                cards.Remove(newCard);
            }

            return result;
        }

        public Card GetCard()
        {
            System.Random random = new System.Random();
            
            if (cards.Count != 0)
            {
                Card result = cards[random.Next(0, cards.Count)];
                cards.Remove(result);

                return result;
            }

            return null;
        }
    }
}
