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

    [SerializeField]
    List<GameObject> bigPoints = null;


    public delegate void allPointsConsumedDelegate(object sender, allPointsConsumedEventArgs e);
    public event allPointsConsumedDelegate allPointsConsumed = delegate { };


    PlayerController pc = null;

    List<Vector2> pointPositions = new List<Vector2>();
    List<Vector2> bigPointPositions = new List<Vector2>();

    private int scoreConsumedCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Null reference Error: Player Gameovject not set!");

        pc = player.GetComponent<PlayerController>();
        pc.ScoreConsumed += Pc_ScoreConsumed;
        pc.BigPointConsumed += Pc_BigPointConsumed;

        foreach(var point in smallPoints)
        {
            pointPositions.Add(new Vector2(point.transform.position.x, point.transform.position.y));
        }

        foreach (var bigPoint in bigPoints)
        {
            bigPointPositions.Add(new Vector2(bigPoint.transform.position.x, bigPoint.transform.position.y));
        }
    }

    private void Pc_BigPointConsumed(object sender, PlayerController.BigPointConsumedEventArgs e)
    {
        scoreConsumedCounter++;

        if (scoreConsumedCounter >= smallPoints.Count + bigPoints.Count)
        {
            scoreConsumedCounter = 0;
            for (int i = 0; i < smallPoints.Count; i++)
            {
                smallPoints[i].transform.position = pointPositions[i];
            }
            for(int i = 0; i < bigPoints.Count; i++)
            {
                bigPoints[i].transform.position = bigPointPositions[i];
            }
            allPointsConsumed(this, new allPointsConsumedEventArgs());
        }
    }

    private void Pc_ScoreConsumed(object sender, PlayerController.ScoreConsumedEventArgs e)
    {
        scoreConsumedCounter++;

        if(scoreConsumedCounter >= smallPoints.Count + bigPoints.Count)
        {
            scoreConsumedCounter = 0;
            for(int i=0; i < smallPoints.Count; i++)
            {
                smallPoints[i].transform.position = pointPositions[i];
            }
            for (int i = 0; i < bigPoints.Count; i++)
            {
                bigPoints[i].transform.position = bigPointPositions[i];
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
