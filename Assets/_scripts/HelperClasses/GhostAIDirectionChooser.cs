using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GhostBehavior;
using static PlayerController;

namespace Assets._scripts.HelperClasses
{
    class GhostAIDirectionChooser
    {
        public static Direction GetPreferedDirection(GhostType type, Vector2 playerPos, Vector2 ghostPos, List<Direction> availibleDirections)
        {
            Direction dir = availibleDirections[0];
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            switch (type)
            {
                case GhostType.cyan:
                    break;
                case GhostType.orange:
                    break;
                case GhostType.pink:
                    break;
                case GhostType.Red:
                    int indexDir = (int)Math.Round((UnityEngine.Random.value * (availibleDirections.Count -0.01f)) - 0.499f);
                    dir = availibleDirections[indexDir];
                    break;
            }

            return dir;
        }
    }
}