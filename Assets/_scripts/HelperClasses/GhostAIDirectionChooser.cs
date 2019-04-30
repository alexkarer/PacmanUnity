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


        /// <summary>
        /// returns the prefered direction of the chost according to the ghost type
        /// </summary>
        /// <param name="type">The type (color) of the ghost</param>
        /// <param name="playerPos">the current position of the player</param>
        /// <param name="ghostPos">the current position of the ghost</param>
        /// <returns></returns>
        public static Direction[] GetPreferedDirections(GhostType type, Vector2 playerPos, Vector2 ghostPos)
        {
            Direction[] directions = new Direction[4];
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            switch (type)
            {
                case GhostType.cyan:

                    sampleList.Add(Direction.up);
                    sampleList.Add(Direction.down);
                    sampleList.Add(Direction.left);
                    sampleList.Add(Direction.right);

                    int indexCyan = (int)(UnityEngine.Random.value * 100) % 4;
                    directions[0] = sampleList[indexCyan];
                    sampleList.RemoveAt(indexCyan);

                    indexCyan = (int)(UnityEngine.Random.value * 100) % 3;
                    directions[1] = sampleList[indexCyan];
                    sampleList.RemoveAt(indexCyan);

                    indexCyan = (int)(UnityEngine.Random.value * 100) % 2;
                    directions[2] = sampleList[indexCyan];
                    sampleList.RemoveAt(indexCyan);

                    directions[3] = sampleList[0];
                    sampleList.RemoveAt(0);

                    break;
                case GhostType.orange:

                    sampleList.Add(Direction.up);
                    sampleList.Add(Direction.down);
                    sampleList.Add(Direction.left);
                    sampleList.Add(Direction.right);

                    int indexOrange = (int)(UnityEngine.Random.value * 100) % 4;
                    directions[0] = sampleList[indexOrange];
                    sampleList.RemoveAt(indexOrange);

                    indexOrange = (int)(UnityEngine.Random.value * 100) % 3;
                    directions[1] = sampleList[indexOrange];
                    sampleList.RemoveAt(indexOrange);

                    indexOrange = (int)(UnityEngine.Random.value * 100) % 2;
                    directions[2] = sampleList[indexOrange];
                    sampleList.RemoveAt(indexOrange);

                    directions[3] = sampleList[0];
                    sampleList.RemoveAt(0);

                    break;
                case GhostType.pink:

                    sampleList.Add(Direction.up);
                    sampleList.Add(Direction.down);
                    sampleList.Add(Direction.left);
                    sampleList.Add(Direction.right);

                    int indexPink = (int)(UnityEngine.Random.value * 100) % 4;
                    directions[0] = sampleList[indexPink];
                    sampleList.RemoveAt(indexPink);

                    indexPink = (int)(UnityEngine.Random.value * 100) % 3;
                    directions[1] = sampleList[indexPink];
                    sampleList.RemoveAt(indexPink);

                    indexPink = (int)(UnityEngine.Random.value * 100) % 2;
                    directions[2] = sampleList[indexPink];
                    sampleList.RemoveAt(indexPink);

                    directions[3] = sampleList[0];
                    sampleList.RemoveAt(0);

                    break;
                case GhostType.Red:

                    sampleList.Add(Direction.up);
                    sampleList.Add(Direction.down);
                    sampleList.Add(Direction.left);
                    sampleList.Add(Direction.right);

                    int indexRed = (int) (UnityEngine.Random.value * 100) % 4;
                    directions[0] = sampleList[indexRed];
                    sampleList.RemoveAt(indexRed);

                    indexRed = (int)(UnityEngine.Random.value * 100) % 3;
                    directions[1] = sampleList[indexRed];
                    sampleList.RemoveAt(indexRed);

                    indexRed = (int)(UnityEngine.Random.value * 100) % 2;
                    directions[2] = sampleList[indexRed];
                    sampleList.RemoveAt(indexRed);

                    directions[3] = sampleList[0];
                    sampleList.RemoveAt(0);

                    break;
            }

            return directions;
        }
    }
}