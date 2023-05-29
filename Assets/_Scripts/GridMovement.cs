using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=AiZ4z4qKy44&ab_channel=Comp-3Interactive

    private bool isMoving;
    private Vector3 origPos, targetPos;
    private RaycastHit2D hit;
    private BoxCollider2D boxCollider;

    public float timeToMove = 0.3f;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
            StartCoroutine(MoveActor(Vector3.up * GameManager.Instance.tileHeight));

        if (Input.GetKey(KeyCode.A) && !isMoving)
            StartCoroutine(MoveActor(Vector3.left * GameManager.Instance.tileWidth));

        if (Input.GetKey(KeyCode.S) && !isMoving)
            StartCoroutine(MoveActor(Vector3.down * GameManager.Instance.tileHeight));

        if (Input.GetKey(KeyCode.D) && !isMoving)
            StartCoroutine(MoveActor(Vector3.right * GameManager.Instance.tileWidth));

    }

    private IEnumerator MoveActor(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;
        
        origPos = transform.position;
        targetPos = origPos + direction;

        direction = Vector3.Normalize(direction);


        
        //if (direction.y != 0)
        //    hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, direction, GameManager.Instance.tileHeight, LayerMask.GetMask("Actor", "Blocking"));
        //else if (direction.x != 0)
        //    hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, direction, GameManager.Instance.tileWidth, LayerMask.GetMask("Actor", "Blocking"));

        if (hit.collider == null)
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            transform.position = targetPos; //just to be safe
        }


        isMoving = false;
    }
}
