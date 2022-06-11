namespace LastCard.Controls
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TapCardsSelector : MonoBehaviour
    {
        public event Action<Card> OnCardSelected;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 cursorPosition = Input.mousePosition;
                TrySelectCard(cursorPosition);
            }
        }

        private void TrySelectCard(Vector2 screenPosition)
        {
            // Select UI object of type Card by mouse tap / by screen position;
            // card = someCard;
            var pointerEventData = new PointerEventData(EventSystem.current) { position = screenPosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            
            if (raycastResults.Count > 0)
            {
                foreach (var result in raycastResults)
                {
                    string name = result.gameObject.name;

                    if (name.Contains("Card") && result.gameObject.transform.
                        IsChildOf(GameObject.Find("UserPlayer(Clone)").transform))
                    {
                        print(name);
                        OnCardSelected?.Invoke(result.gameObject.GetComponent<Card>());
                    }
                    //Card card = result.gameObject.GetComponent<Card>;
                }
            }

            // if (isCardSelected)
            // {
            //     OnCardSelected?.Invoke(card);
            // }
        }
    }
}