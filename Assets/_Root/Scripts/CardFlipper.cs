using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class CardFlipper : MonoBehaviour
    {
        private Sprite CardFront;
        public Sprite CardBack;

        private void Awake() 
        {
            CardFront = gameObject.GetComponent<Image>().sprite;
        }

        public void Flip()
        {
            Sprite currentSprite = gameObject.GetComponent<Image>().sprite;

            if (currentSprite == CardFront)
            {
                gameObject.GetComponent<Image>().sprite = CardBack;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = CardFront;
            }
        }
    }
}
