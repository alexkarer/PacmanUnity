using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite spriteLeft;
    [SerializeField]
    Sprite spriteUp;
    [SerializeField]
    Sprite spriteDown;

    [SerializeField]
    private float speed = 3.0f;

    bool goUp;
    bool goDown;
    bool goLeft;
    bool goRight;

    float timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteLeft;
        spriteRenderer.flipX = false;
        goLeft = true;
        timeStamp = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(goLeft && timeStamp < Time.time)
        {
            goLeft = false;
            goUp = true;
            timeStamp = Time.time + 1;
        }
        else if (goUp && timeStamp < Time.time)
        {
            goUp = false;
            goRight = true;
            timeStamp = Time.time + 1;
        }
        else if (goRight && timeStamp < Time.time)
        {
            goRight = false;
            goDown = true;
            timeStamp = Time.time + 1;
        }
        else if (goDown && timeStamp < Time.time)
        {
            goDown = false;
            goLeft = true;
            timeStamp = Time.time + 1;
        }


        if (goLeft)
        {
            spriteRenderer.sprite = spriteLeft;
            spriteRenderer.flipX = false;
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        else if (goRight)
        {
            spriteRenderer.sprite = spriteLeft;
            spriteRenderer.flipX = true;
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else if (goUp)
        {
            spriteRenderer.sprite = spriteUp;
            spriteRenderer.flipX = false;
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        else if (goDown)
        {
            spriteRenderer.sprite = spriteDown;
            spriteRenderer.flipX = false;
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
    }
}
