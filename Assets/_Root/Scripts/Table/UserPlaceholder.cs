using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastCard
{
    public class UserPlaceholder : MonoBehaviour
    {
        public UserPlayer PlaceUser(UserPlayer prefab)
        {
            // Instantiate
            // Place
            UserPlayer user = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            user.transform.SetParent(GameObject.Find("Background").transform, false);
            user.transform.position = new Vector3(0, 150, 0);

            return user;
        }
    }
}
