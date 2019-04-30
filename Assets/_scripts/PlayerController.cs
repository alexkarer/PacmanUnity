using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // EVENTS
    public delegate void ScoreAddDelegate(object sender, ScoreAddEventArgs e);
    public event ScoreAddDelegate ScoreAdd = delegate { };

    public delegate void ScoreConsumedDelegate(object sender, ScoreConsumedEventArgs e);
    public event ScoreConsumedDelegate ScoreConsumed = delegate { };

    public delegate void LiveLostDelegate(object sender, LiveLostEventArgs e);
    public event LiveLostDelegate LiveLost = delegate { };

    public delegate void BigPointConsumedDelegate(object sender, BigPointConsumedEventArgs e);
    public event BigPointConsumedDelegate BigPointConsumed = delegate { };


    // SERIALIZED FIELDS
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private Rigidbody2D body2D = null;
    [SerializeField]
    private int smallPointWorth = 10;
    [SerializeField]
    private int bigPointWorth = 100;
    [SerializeField]
    private int ghostWorth = 250;
    [SerializeField]
    private float guardPeriodTime = 3;
    [SerializeField]
    private float ghostVulnerableTime = 6;


    // ENUMS
    public enum Direction { up, down, right, left };
    private Direction pacmanDir;

    private enum PacManStates { move, death, guard};
    private PacManStates pacManState;

    
    // MEMBERS
    private LayerMask borderLayer;
    private Animator animator;

    private Vector2 startPosition;

    private float timeStamp;


    // UNITY METHODS
    private void Awake()
    {
        if (body2D == null)
            body2D = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();
        borderLayer = LayerMask.GetMask("Border");
    }

    // Use this for initialization
    void Start()
    {
        pacmanDir = Direction.right;
        pacManState = PacManStates.move;
        startPosition = body2D.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (pacManState == PacManStates.move)
        {
            pacmanTurn();
            pacmanMove();
        }
        else if (pacManState == PacManStates.guard)
        {
            if (timeStamp <= Time.time)
            {
                animator.ResetTrigger("PacManGuardTime");
                animator.SetTrigger("PacManRespawnCompleted");
                pacManState = PacManStates.move;
            }
            pacmanTurn();
            pacmanMove();
        }
        else
        {
            MoveBackToOrigin();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 13 && pacManState == PacManStates.move || pacManState == PacManStates.guard)
        {
            // Collision with a small point
            collision.gameObject.transform.position = new Vector2(20, 20);
            ScoreAdd(this, new ScoreAddEventArgs(smallPointWorth));
            ScoreConsumed(this, new ScoreConsumedEventArgs());
        }
        else if(collision.gameObject.layer == 12 && pacManState == PacManStates.move || pacManState == PacManStates.guard)
        {
            // Collision with a big point
            collision.gameObject.transform.position = new Vector2(20, 20);
            ScoreAdd(this, new ScoreAddEventArgs(bigPointWorth));
            BigPointConsumed(this, new BigPointConsumedEventArgs(ghostVulnerableTime));
        }
        else if(collision.gameObject.layer == 9 && pacManState == PacManStates.move 
            && collision.gameObject.GetComponent<GhostBehavior>().ghostState == GhostBehavior.GhostStates.Regular)
        {
            // Collision with a Ghost in regular mode
            LiveLost(this, new LiveLostEventArgs());
            animator.ResetTrigger("PacManRespawnCompleted");
            animator.SetTrigger("PacManDeath");

            pacManState = PacManStates.death;
            body2D.velocity = Vector2.zero;
            body2D.isKinematic = true;
        }
        else if (collision.gameObject.layer == 9 && pacManState == PacManStates.move
            && collision.gameObject.GetComponent<GhostBehavior>().ghostState == GhostBehavior.GhostStates.Vulnerable)
        {
            //Collision with a Ghost in the vulnerable state
            ScoreAdd(this, new ScoreAddEventArgs(ghostWorth));
            
            //TODO ghost death 





        }
    }


    // PRIVATE METHODS
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

    private void MoveBackToOrigin()
    {
        body2D.position = Vector2.MoveTowards(body2D.position, startPosition, speed * 1.5f * Time.fixedDeltaTime);

        if (body2D.position == startPosition)
        {
            pacManState = PacManStates.guard;
            timeStamp = Time.time + guardPeriodTime;

            animator.ResetTrigger("PacManDeath");
            animator.SetTrigger("PacManGuardTime");

            pacmanDir = Direction.right;

            body2D.isKinematic = false;
        }
    }
   

    // EVENT ARGS
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

    public class LiveLostEventArgs : EventArgs
    {

    }

    public class BigPointConsumedEventArgs : EventArgs
    {
        public readonly float ghostVulnerableTime;

        public BigPointConsumedEventArgs(float ghostVulnerableTime)
        {
            this.ghostVulnerableTime = ghostVulnerableTime;
        }
    }
}
