using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using static GhostBehavior;
using static PlayerController;

namespace Assets._scripts.HelperClasses
{
    class GhostAIDirectionChooser
    {
        static List<Direction> sampleList = new List<Direction>();

        public static Direction[] GetPreferedDirections(GhostType type, Vector2 playerPos, Vector2 ghostPos)
        {
            Direction[] directions = new Direction[4];
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

                    sampleList.Add(Direction.up);
                    sampleList.Add(Direction.down);
                    sampleList.Add(Direction.left);
                    sampleList.Add(Direction.right);

                    int index = (int) (UnityEngine.Random.value * 100) % 4;
                    directions[0] = sampleList[index];
                    sampleList.RemoveAt(index);

                    index = (int)(UnityEngine.Random.value * 100) % 3;
                    directions[1] = sampleList[index];
                    sampleList.RemoveAt(index);

                    index = (int)(UnityEngine.Random.value * 100) % 2;
                    directions[2] = sampleList[index];
                    sampleList.RemoveAt(index);

                    directions[3] = sampleList[0];
                    sampleList.RemoveAt(0);

                    break;
            }

            return directions;
        }
    }
}