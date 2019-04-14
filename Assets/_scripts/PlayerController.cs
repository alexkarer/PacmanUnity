using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private Rigidbody2D body2D = null;

    [SerializeField]
    private Transform smallPointsParent = null;

    [SerializeField]
    private int smallPointWorth = 20;

    public enum Direction { up, down, right, left };

    private Direction pacmanDir;
    private LayerMask borderLayer;

    public delegate void ScoreAddDelegate(object sender, ScoreAddEventArgs e);
    public event ScoreAddDelegate ScoreAdd = delegate { };

    public delegate void ScoreConsumedDelegate(object sender, ScoreConsumedEventArgs e);
    public event ScoreConsumedDelegate ScoreConsumed = delegate { };

    private void Awake()
    {
        if (body2D == null)
            body2D = this.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        pacmanDir = Direction.right;
        borderLayer = LayerMask.GetMask("Border");
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        pacmanTurn();
        pacmanMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.IsChildOf(smallPointsParent))
        {
            collision.gameObject.transform.position = new Vector2(20, 20);
            ScoreAdd(this, new ScoreAddEventArgs(smallPointWorth));
            ScoreConsumed(this, new ScoreConsumedEventArgs());
        }
    }

    private void pacmanMove()
    {
        if(pacmanDir == Direction.left && body2D.position.x <= -14.5f)
        {
            body2D.position = new Vector2(14.38f, this.body2D.position.y);
        }
        else if(pacmanDir == Direction.right && body2D.position.x >= 14.38f)
        {
            body2D.position = new Vector2(-14.5f, this.body2D.position.y);
        }

        switch(pacmanDir)
        {
            case Direction.right:
                body2D.velocity = Vector2.right * speed;
                break;
            case Direction.left:
                body2D.velocity = Vector2.left * speed;
                break;
            case Direction.up:
                body2D.velocity = Vector2.up * speed;
                break;
            case Direction.down:
                body2D.velocity = Vector2.down * speed;
                break;
        }
    }

    private void pacmanTurn()
    {
        switch (pacmanDir)
        {
            case Direction.right:
                body2D.MoveRotation(0);
                break;
            case Direction.left:
                body2D.MoveRotation(180);
                break;
            case Direction.up:
                body2D.MoveRotation(90);
                break;
            case Direction.down:
                body2D.MoveRotation(-90);
                break;
        }
    }

    void GetInput()
    {
        if (Input.GetAxis("Vertical") > 0.1f)   // UP
        {
            RaycastHit2D hit2D = Physics2D.Raycast(body2D.position, Vector2.up, 0.6f, borderLayer);
            if (hit2D)
                return;

            pacmanDir = Direction.up;
        }
        else if (Input.GetAxis("Vertical") < -0.1f) // DOWN
        {
            RaycastHit2D hit2D = Physics2D.Raycast(body2D.position, Vector2.down, 0.6f, borderLayer);
            if (hit2D)
                return;

            pacmanDir = Direction.down;
        }
        else if (Input.GetAxis("Horizontal") < -0.1f) // LEFT
        {
            RaycastHit2D hit2D = Physics2D.Raycast(body2D.position, Vector2.left, 0.6f, borderLayer);
            if (hit2D)
                return;

            pacmanDir = Direction.left;
        }
        else if (Input.GetAxis("Horizontal") > 0.1f) // RIGHT
        {
            RaycastHit2D hit2D = Physics2D.Raycast(body2D.position, Vector2.right, 0.6f, borderLayer);
            if (hit2D)
                return;

            pacmanDir = Direction.right;
        }
    }
   
    public class ScoreAddEventArgs : EventArgs
    {
        public readonly int score;

        public ScoreAddEventArgs(int score)
        {
            this.score = score;
        }
    }

    public class ScoreConsumedEventArgs : EventArgs
    {
        
    }
}
