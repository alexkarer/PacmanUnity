using Assets._scripts.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GhostBehavior : MonoBehaviour
{
    public enum GhostType { Red, pink, orange, cyan};


    [SerializeField]
    Rigidbody2D rigid2D;
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
    }

    private void FixedUpdate()
    {
        getDirection();
        ghostMove();
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
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, rigid2D.transform.forward, 0.6f, 31);
        if (hit2D.collider != null)
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
                            MoveMode, playerRigid2D.position, rigid2D.position, dirList.ToArray());
        }
    }

    void ghostMove()
    {
        switch (ghostDir)
        {
            case Direction.right:
                rigid2D.velocity = Vector2.right * speed;
                break;
            case Direction.left:
                rigid2D.velocity = Vector2.left * speed;
                break;
            case Direction.up:
                rigid2D.velocity = Vector2.up * speed;
                break;
            case Direction.down:
                rigid2D.velocity = Vector2.down * speed;
                break;
        }
    }
}