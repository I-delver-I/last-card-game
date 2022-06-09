namespace LastCard
{
    using UnityEngine;

    public class BotPlaceholder : MonoBehaviour
    {
        public BotPlayer PlaceBot(BotPlayer prefab)
        {
            // Instantiate
            // Place
            BotPlayer bot = Instantiate(prefab, transform);
            
            return bot;
        }
    }
}
