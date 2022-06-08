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
                var newCard = Instantiate(cardsPrefabs[0], this.transform);
                result.Add(newCard);
            }
            return result;
        }

        public List<Card> GetCards(int amount)
        {
            throw new NotImplementedException();
        }

        public Card GetCard()
        {
            throw new NotImplementedException();
        }
    }
}
