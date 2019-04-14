using Assets._scripts.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GhostBehavior : MonoBehaviour
{
    public enum GhostType { Red, pink, orange, cyan};

    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    [SerializeField]
    Sprite spriteLeft = null;
    [SerializeField]
    Sprite spriteUp = null;
    [SerializeField]
    Sprite spriteDown = null;

    [SerializeField]
    Rigidbody2D playerRigid2D = null;
    [SerializeField]
    GhostType MoveMode = GhostType.Red; 
    [SerializeField]
    private float speed = 3.0f;

    private Direction ghostDir;

    private LayerMask borderLayer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteLeft;
        spriteRenderer.flipX = false;
        ghostDir = Direction.left;
        borderLayer = LayerMask.GetMask("Border");
    }

    // Update is called once per frame
    void Update()
    {
        TurnGhost();
        ghostMove();
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