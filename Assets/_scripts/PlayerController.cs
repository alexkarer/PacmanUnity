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

    bool KeyUp = false;
    bool KeyDown = false;
    bool KeyRight = false;
    bool KeyLeft = false;

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


        if (pacmanDir == Direction.right)
        {
            //body2D.MovePosition(body2D.position + Vector2.right * speed * Time.fixedDeltaTime);
            body2D.velocity = Vector2.right * speed;
        }
        else if (pacmanDir == Direction.left)
        {
            //body2D.MovePosition(body2D.position + Vector2.left * speed * Time.fixedDeltaTime);
            body2D.velocity = Vector2.left * speed;
        }
        else if (pacmanDir == Direction.up)
        {
            //body2D.MovePosition(body2D.position + Vector2.up * speed * Time.fixedDeltaTime);
            body2D.velocity = Vector2.up * speed;
        }
        else if (pacmanDir == Direction.down)
        {
            //body2D.MovePosition(body2D.position + Vector2.down * speed * Time.fixedDeltaTime);
            body2D.velocity = Vector2.down * speed;
        }
    }

    private void pacmanTurn()
    {
        if (KeyUp)
        {
            body2D.MoveRotation(90);
            pacmanDir = Direction.up;
        }
        else if (KeyDown)
        {
            body2D.MoveRotation(-90);
            pacmanDir = Direction.down;
        }
        else if (KeyLeft)
        {
            body2D.MoveRotation(180);
            pacmanDir = Direction.left;
        }
        else if (KeyRight)
        {
            body2D.MoveRotation(0);
            pacmanDir = Direction.right;
        }
    }

    void GetInput()
    {
        if (Input.GetAxis("Vertical") > 0.1f)
        {
            KeyUp = true;
            KeyDown = false;
            KeyRight = false;
            KeyLeft = false;
        }
        else if (Input.GetAxis("Vertical") < -0.1f)
        {
            KeyUp = false;
            KeyDown = true;
            KeyRight = false;
            KeyLeft = false;
        }
        else if (Input.GetAxis("Horizontal") < -0.1f)
        {
            KeyUp = false;
            KeyDown = false;
            KeyRight = false;
            KeyLeft = true;
        }
        else if (Input.GetAxis("Horizontal") > 0.1f)
        {
            KeyUp = false;
            KeyDown = false;
            KeyRight = true;
            KeyLeft = false;
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
