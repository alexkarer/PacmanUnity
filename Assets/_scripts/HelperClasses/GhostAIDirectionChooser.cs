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
        public static Direction[] GetPreferedDirections(GhostType type, Vector2 playerPos, Vector2 ghostPos, Direction[] availibleDirections)
        {
            Direction[] dirs = new Direction[availibleDirections.Length];

            // TODO calculate the prefered directions of the availible directions and send them back in order

            return dirs;
        }
    }
}