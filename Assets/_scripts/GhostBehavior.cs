using Assets._scripts.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GhostBehavior : MonoBehaviour
{
    public enum GhostType { Red, pink, orange, cyan};

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite spriteLeft;
    [SerializeField]
    Sprite spriteUp;
    [SerializeField]
    Sprite spriteDown;

    [SerializeField]
    Rigidbody2D playerRigid2D;
    [SerializeField]
    GhostType MoveMode = GhostType.Red; 
    [SerializeField]
    private float speed = 3.0f;

    private Direction ghostDir;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteLeft;
        spriteRenderer.flipX = false;
        ghostDir = Direction.left;
    }

    // Update is called once per frame
    void Update()
    {
        TurnGhost();
        ghostMove();
    }

    private void FixedUpdate()
    {
        getDirection();
    }

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

    void getDirection()
    {
        RaycastHit2D hit2D = new RaycastHit2D();

        switch (ghostDir)
        {
            case Direction.right:
                hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, 31);
                break;
            case Direction.left:
                hit2D = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, 31);
                break;
            case Direction.up:
                hit2D = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, 31);
                break;
            case Direction.down:
                hit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 31);
                break;
        }

        if (hit2D)
        {
            List<Direction> dirList = new List<Direction>();
            
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, 31);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 31);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, 31);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, 31);
            
            if (!hitUp)
                dirList.Add(Direction.up);
            if (!hitDown)
                dirList.Add(Direction.down);
            if (!hitLeft)
                dirList.Add(Direction.left);
            if (!hitRight)
                dirList.Add(Direction.right);

            ghostDir = GhostAIDirectionChooser.GetPreferedDirection(
                            MoveMode, playerRigid2D.position, transform.position, dirList);
        }
    }

    void ghostMove()
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
}