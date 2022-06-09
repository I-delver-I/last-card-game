namespace LastCard
{
    using System.Threading.Tasks;
    using Controls;
    using UnityEngine;

    public class UserPlayer : Player
    {
        [SerializeField]
        private TapCardsSelector cardsSelector;

        private TaskCompletionSource<bool> turnTcs;
        
        private void Awake()
        {
            cardsSelector.OnCardSelected += OnCardTap;
        }

        private void OnCardTap(Card selectedCard)
        {
            if (!cards.Contains(selectedCard))
            {
                return;
            }

            if (turnTcs == null || turnTcs.Task.IsCompleted)
            {
                return;
            }

            SendCardSelected(selectedCard);
        }

        public override Task MakeTurn()
        {
            turnTcs = new TaskCompletionSource<bool>();
            
            return turnTcs.Task;
        }

        public void EndTurn()
        {
            if (turnTcs == null)
            {
                return;
            }

            turnTcs.TrySetResult(true);
        }

        private void OnDestroy()
        {
            cardsSelector.OnCardSelected -= OnCardTap;
        }
    }
}
