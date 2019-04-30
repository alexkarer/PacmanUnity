using Assets._scripts.HelperClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GhostBehavior : MonoBehaviour
{
    // ENUM
    public enum GhostType { Red, pink, orange, cyan};

    public enum GhostStates { Regular, BeforeSpawn, Vulnerable };


    //SERIALIZEFIELD
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    [SerializeField]
    Sprite spriteLeft = null;
    [SerializeField]
    Sprite spriteUp = null;
    [SerializeField]
    Sprite spriteDown = null;
    [SerializeField]
    Sprite spriteVulnerable1 = null;
    [SerializeField]
    Sprite spriteVulnerable2 = null;

    [SerializeField]
    GameObject player = null;
    [SerializeField]
    GhostType MoveMode = GhostType.Red;
    [SerializeField]
    SpriteRenderer ghostGate = null;

    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float preSpawnTime = 5;


    // MEMBERS
    private Direction ghostDir;
    public GhostStates ghostState;

    private LayerMask borderLayer;
    List<Direction> sampleList = new List<Direction>();

    private float spawnTime;
    private float timeStampVulnerable;

    private float blinkTimeStamp;
    private bool blinkingFlag = false;

    Rigidbody2D playerRigid2D = null;
    PlayerController pc = null;

    // UNITY METHODS
    private void Awake()
    {
        borderLayer = LayerMask.GetMask("Border");
        playerRigid2D = player.GetComponent<Rigidbody2D>();

        pc = player.GetComponent<PlayerController>();
        pc.BigPointConsumed += Pc_BigPointConsumed;
    }

    private void Pc_BigPointConsumed(object sender, BigPointConsumedEventArgs e)
    {
        blinkTimeStamp = 0.0f;
        ghostState = GhostStates.Vulnerable;
        spriteRenderer.sprite = spriteVulnerable1;
        timeStampVulnerable = Time.time + e.ghostVulnerableTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteLeft;
        spriteRenderer.flipX = false;

        ghostDir = Direction.left;
        ghostState = GhostStates.BeforeSpawn;

        spawnTime = Time.time + preSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostState == GhostStates.Regular)
        {
            TurnGhost();
            GhostMove();
        }
        else if(ghostState == GhostStates.Vulnerable)
        {
            GhostVulnerableController();
            GhostMove();
        }
        else
        {
            if(Time.time >= spawnTime)
            {
                ghostGate.enabled = false;
                GhostSpawn();
            }
            else
            {
                RaycastHit2D hit2D = new RaycastHit2D();
                switch (ghostDir)
                {
                    case Direction.right:
                        hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, borderLayer);
                        break;
                    case Direction.left:
                        hit2D = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, borderLayer);
                        break;
                    case Direction.up:
                        hit2D = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, borderLayer);
                        break;
                    case Direction.down:
                        hit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, borderLayer);
                        break;
                }

                if(hit2D)
                    GhostMoveBeforeSpawn();

                TurnGhost();
                GhostMove();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1, borderLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1, borderLayer);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down,1, borderLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 1, borderLayer);

            bool foundDirection = false;

            Direction[] preferedDirections = GhostAIDirectionChooser.GetPreferedDirections(MoveMode, playerRigid2D.position, transform.position);

            for (int i = 0; i < preferedDirections.Length; i++)
            {
                switch (preferedDirections[i])
                {
                    case Direction.right:
                        if (!hitRight)
                        {
                            ghostDir = Direction.right;
                            foundDirection = true;
                        }
                        break;
                    case Direction.left:
                        if (!hitLeft)
                        {
                            ghostDir = Direction.left;
                            foundDirection = true;
                        }
                        break;
                    case Direction.up:
                        if (!hitUp)
                        {
                            ghostDir = Direction.up;
                            foundDirection = true;
                        }
                        break;
                    case Direction.down:
                        if (!hitDown)
                        {
                            ghostDir = Direction.down;
                            foundDirection = true;
                        }
                        break;
                }
                if (foundDirection)
                    break;
            }
        }
    }


    // PRIVATE METHODS
    void TurnGhost()
    {
        switch (ghostDir)
        {
            case Direction.right:
                spriteRenderer.sprite = spriteLeft;
                spriteRenderer.flipX = true;
                break;
            case Direction.left:
                spriteRenderer.sprite = spriteLeft;
                spriteRenderer.flipX = false;
                break;
            case Direction.up:
                spriteRenderer.sprite = spriteUp;
                spriteRenderer.flipX = false;
                break;
            case Direction.down:
                spriteRenderer.sprite = spriteDown;
                spriteRenderer.flipX = false;
                break;
        }
    }

    void GhostMove()
    {

        if (ghostDir == Direction.left && transform.position.x <= -14.5f)
        {
            transform.position = new Vector2(14.38f, this.transform.position.y);
        }
        else if (ghostDir == Direction.right && transform.position.x >= 14.38f)
        {
            transform.position = new Vector2(-14.5f, this.transform.position.y);
        }

        switch (ghostDir)
        {
            case Direction.right:
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                break;
            case Direction.left:
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                break;
            case Direction.up:
                transform.Translate(Vector2.up * speed * Time.deltaTime);
                break;
            case Direction.down:
                transform.Translate(Vector2.down * speed * Time.deltaTime);
                break;
        }
    }

    void GhostMoveBeforeSpawn()
    {
        sampleList.Clear();
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        sampleList.Add(Direction.up);
        sampleList.Add(Direction.down);
        sampleList.Add(Direction.left);
        sampleList.Add(Direction.right);

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, borderLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, borderLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, borderLayer);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, borderLayer);

        if (hitRight)
            sampleList.Remove(Direction.right);
        if (hitLeft)
            sampleList.Remove(Direction.left);
        if (hitDown)
            sampleList.Remove(Direction.down);
        if (hitUp)
            sampleList.Remove(Direction.up);

        int index = (int)(UnityEngine.Random.value * 100) % sampleList.Count;
        ghostDir = sampleList[index];
    }

    void GhostSpawn()
    {
        if (transform.position.x != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, transform.position.y), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 4.5f), speed * Time.deltaTime);
        }

        if (transform.position.x == 0 && transform.position.y == 4.5f)
        {
            ghostState = GhostStates.Regular;
        }
    }

    void GhostVulnerableController()
    {
        if (Time.time >= timeStampVulnerable * 0.75f)
        {
            if (!blinkingFlag && Time.time >= blinkTimeStamp)
            {
                blinkTimeStamp = Time.time + 0.2f;
                blinkingFlag = true;

                spriteRenderer.sprite = spriteVulnerable2;
            }
            else if (blinkingFlag && Time.time >= blinkTimeStamp)
            {
                blinkTimeStamp = Time.time + 0.2f;
                blinkingFlag = false;

                spriteRenderer.sprite = spriteVulnerable1;
            }
        }

        if (Time.time >= timeStampVulnerable)
        {
            ghostGate.enabled = true;
            ghostState = GhostStates.Regular;
            ghostDir = Direction.left;
        }
    }
}