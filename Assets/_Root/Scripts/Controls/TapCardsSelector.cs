namespace LastCard.Controls
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TapCardsSelector : MonoBehaviour
    {
        public event Action<Card> OnCardSelected;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var cursorPosition = Input.mousePosition;
                TrySelectCard(cursorPosition);
            }
        }


        private void TrySelectCard(Vector2 screenPosition)
        {
            //Debug.Log($"Tap on: {screenPosition} position");
            // Select UI object of type Card by mouse tap / by screen position;
            // card = someCard;
            //Camera camera = new Camera();
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(screenPosition).x,
                Camera.main.ScreenToWorldPoint(screenPosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit.transform.gameObject.name.Contains("Card"))
            {
                print($"Hit {hit.transform.gameObject.name}");

                // if (isCardSelected)
                // {
                //     OnCardSelected?.Invoke(card);
                // }
            }
            else
            {
                print("No hit");
            }
        }
    }
}