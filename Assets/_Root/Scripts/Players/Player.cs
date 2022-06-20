namespace LastCard
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LastCard.Logic;
    using UnityEngine;

    public abstract class Player : MonoBehaviour
    {
        protected List<Card> cards = new List<Card>();
        protected CardsDeck deck;
        protected RulesResolver resolver;
        protected CardsPile pile;

        [SerializeField]
        private Transform cardsHolder;

        public event Func<Player, Card, bool> OnCardSelected;
        public event Action<Player> OnCardsMissing;
        public bool CanMakeTurn { get; set; } = true;
        public bool DontTurn = false;
        
        public void Init(RulesResolver rulesResolver, CardsDeck cardsDeck, CardsPile cardsPile)
        {
            resolver = rulesResolver;
            deck = cardsDeck;
            pile = cardsPile;
        }

        public virtual void AddCards(List<Card> cardsToAdd)
        {
            List<Card> result = new List<Card>();

            foreach (Card card in cardsToAdd)
            {
                if (card != null)
                {
                    result.Add(card);
                }
            }

            foreach (Card card in result)
            {
                card.transform.SetParent(cardsHolder, false);
            }

            cards.AddRange(result);
        }

        public int GetPointsNumber()
        {
            int result = 0;

            if (cards.Count == 0)
            {
                return result;
            }

            foreach (Card card in cards)
            {
                result += (int)card.nominal;
            }

            return result;
        }

        public int GetCardsCount()
        {
            return cards.Count;
        }

        public List<Card> GetCards()
        {
            return new List<Card>(cards);
        }

        public bool ContainsCard(Card card)
        {
            return cards.Contains(card);
        }

        public bool ContainsCard(Predicate<Card> predicate)
        {
            return cards.Find(predicate);
        }

        public virtual void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        protected bool SendCardSelected(Card card)
        {
            return OnCardSelected.Invoke(this, card);
        }

        protected void TakeCards()
        {
            OnCardsMissing?.Invoke(this);
        }

        public abstract Task MakeTurn();
    }
}
