using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveHandler : MonoBehaviour
{
    [SerializeField]
    GameObject player = null;

    [SerializeField]
    private int lives = 4;

    [SerializeField]
    MeshRenderer GameOverHint = null;

    TextMeshPro tmp = null;
    PlayerController pc = null;
   
    private int lastlives = 0;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("Null reference Error: Player Gameobject not set!");

        pc = player.GetComponent<PlayerController>();
        pc.LiveLost += Pc_LiveLost;

        tmp.text = lives.ToString();
        lastlives = lives;

        GameOverHint.enabled = false;
    }

    private void Pc_LiveLost(object sender, PlayerController.LiveLostEventArgs e)
    {
        lives--;
    }


    // Update is called once per frame
    void Update()
    {
        if (lives != lastlives)
        {
            if(lives == 0)
            {
                GameOverHint.enabled = true;
                Time.timeScale = 0;
            }

            lastlives = lives;
            tmp.text = lives.ToString();
        }
    }
}
