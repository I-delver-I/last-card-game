namespace LastCard
{
    using UnityEngine;

    public class BotPlaceholder : MonoBehaviour
    {
        public BotPlayer PlaceBot(BotPlayer prefab, int botNumber)
        {
            // Instantiate
            // Place
            BotPlayer bot = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            bot.transform.SetParent(GameObject.Find("Background").transform, false);
            
            switch (botNumber)
            {
                case 1:
                    bot.transform.position = new Vector3(0, -150, 0);
                    return bot;
                case 2:
                    bot.transform.position = new Vector3(150, 0, 0);
                    return bot;
                case 3:
                    bot.transform.position = new Vector3(-150, 0, 0);
                    return bot;
                case 4:
                    bot.transform.position = new Vector3(-600, -150, 0);
                    return bot;
                case 5:
                    bot.transform.position = new Vector3(600, -150, 0);
                    return bot;
                default:
                    return null;
            }
        }
    }
}
