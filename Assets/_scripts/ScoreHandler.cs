using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    GameObject player = null;

    TextMeshPro tmp = null;
    PlayerController pc = null;

    private int score = 0;
    private int lastScore = 0;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Null reference Error: Player Gameovject not set!");

        pc = player.GetComponent<PlayerController>();
        pc.ScoreAdd += Pc_ScoreAdd;

        tmp.text = "00000000";
    }

    private void Pc_ScoreAdd(object sender, PlayerController.ScoreAddEventArgs e)
    {
        score += e.score;
    }

    // Update is called once per frame
    void Update()
    {
        if(score != lastScore)
        {
            lastScore = score;
            tmp.text = score.ToString("00000000");
        }
    }
}
