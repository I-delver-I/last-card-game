using System.Collections.Generic;
using UnityEngine;

namespace LastCard
{
    using System.Threading.Tasks;

    public class BotPlayer : Player
    {
        public override Task MakeTurn()
        {
            return Task.CompletedTask;
        }
    }
}
