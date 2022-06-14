namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

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
            ShuffleCards(cardsPrefabs);
            cards = CreateCards();
        }

        private List<Card> CreateCards()
        {
            var result = new List<Card>();
            
            for (var i = 0; i < maxCardsAmount; i++)
            {
                var newCard = Instantiate(cardsPrefabs[i], this.transform);
                newCard.flipper.Flip();
                result.Add(newCard);
            }
            
            return result;
        }

        private void ShuffleCards(List<Card> cardsList)
        {
            System.Random random = new System.Random();

            for (var i = 0; i < cardsList.Count; i++)
            {
                int randomIndex = random.Next(0, cardsList.Count - 1);
                Card card = cardsList[i];
                cardsList[i] = cardsList[randomIndex];
                cardsList[randomIndex] = card;
            }
        }

        public bool ContainsCard(Card card)
        {
            return cards.Contains(card);
        }

        public List<Card> GetCards(int amount)
        {
            List<Card> result = new List<Card>();

            while ((cards.Count != 0) && (amount <= cards.Count) && (amount != result.Count))
            {
                Card newCard = cards.Last();
                result.Add(newCard);
                cards.Remove(newCard);
            }

            return result;
        }

        public Card GetCard()
        {
            if (cards.Count != 0)
            {
                Card result = cards.Last();
                cards.Remove(result);

                return result;
            }

            return null;
        }
    }
}
