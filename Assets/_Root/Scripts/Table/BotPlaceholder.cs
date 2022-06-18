namespace LastCard
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BotPlaceholder : MonoBehaviour
    {
        [SerializeField]
        private Outline border;

        [SerializeField]
        private Text cardsNumber;

        public BotPlayer PlaceBot(BotPlayer prefab)
        {
            BotPlayer bot = Instantiate(prefab, transform);
            bot.border = border;
            bot.cardsCount = cardsNumber;
            
            return bot;
        }
    }
}
