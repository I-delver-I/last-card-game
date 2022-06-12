using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class CardFlipper : MonoBehaviour
    {
        private Sprite cardFront;

        [SerializeField]
        private Image image;
        
        public Sprite CardBack;

        private void Awake()
        {
            cardFront = image.sprite;
        }

        public void Flip()
        {
            Sprite currentSprite = image.sprite;

            if (currentSprite == cardFront)
            {
                image.sprite = CardBack;
            }
            else
            {
                image.sprite = cardFront;
            }
        }
    }
}
