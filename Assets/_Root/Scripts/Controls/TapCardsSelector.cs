namespace LastCard.Controls
{
    using System;
    using UnityEngine;

    public class TapCardsSelector : MonoBehaviour
    {
        public event Action<Card> OnCardSelected;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var cursorPosition = Input.mousePosition;
                TrySelectCard(cursorPosition);
            }
        }

        private void TrySelectCard(Vector2 screenPosition)
        {
            Debug.Log($"Tap on: {screenPosition} position");
            // Select UI object of type Card by mouse tap / by screen position;
            // card = someCard;
            
            /*
            if (isCardSelected)
            {
                OnCardSelected?.Invoke(card);
            }
            */
        }
    }
}