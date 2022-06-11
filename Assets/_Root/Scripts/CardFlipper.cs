using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class CardFlipper : MonoBehaviour
    {
        private Sprite cardFront;

        public Sprite CardBack;

        private void Awake()
        {
            cardFront = GetComponent<Image>().sprite;
        }

        public void Flip()
        {
            Sprite currentSprite = GetComponent<Image>().sprite;

            if (currentSprite == cardFront)
            {
                GetComponent<Image>().sprite = CardBack;
            }
            else
            {
                GetComponent<Image>().sprite = cardFront;
            }
        }
    }
}
