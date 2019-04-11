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
    GhostType MoveMode = GhostType.Red; 
    [SerializeField]
    private float speed = 3.0f;

    private Direction ghostDir;

    bool[] dirs;
    int index = 0;

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

            // TODO calculate availible Directions and call
            // GhostAIDirectionChooser.GetPreferedDirections();
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