using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRegulate : MonoBehaviour
{
    [SerializeField]
    GameObject player = null;

    [SerializeField]
    List<GameObject> smallPoints = null;


    public delegate void allPointsConsumedDelegate(object sender, allPointsConsumedEventArgs e);
    public event allPointsConsumedDelegate allPointsConsumed = delegate { };


    PlayerController pc = null;

    List<Vector2> pointPositions = new List<Vector2>();

    private int scoreConsumedCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Null reference Error: Player Gameovject not set!");

        pc = player.GetComponent<PlayerController>();
        pc.ScoreConsumed += Pc_ScoreConsumed;

        foreach(var point in smallPoints)
        {
            pointPositions.Add(new Vector2(point.transform.position.x, point.transform.position.y));
        }
    }

    private void Pc_ScoreConsumed(object sender, PlayerController.ScoreConsumedEventArgs e)
    {
        scoreConsumedCounter++;

        if(scoreConsumedCounter >= smallPoints.Capacity)
        {
            scoreConsumedCounter = 0;
            for(int i=0; i < smallPoints.Capacity; i++)
            {
                smallPoints[i].transform.position = pointPositions[i];
            }
            allPointsConsumed(this, new allPointsConsumedEventArgs());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class allPointsConsumedEventArgs : EventArgs
    {

    }
}
