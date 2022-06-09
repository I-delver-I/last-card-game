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
            UserPlayer user = Instantiate(prefab, transform);

            return user;
        }
    }
}
