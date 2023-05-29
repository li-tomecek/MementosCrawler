using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool isMoving;
    private Vector3 origPos, targetPos;
    public float timeToMove = 0.5f;


    float tileHeight, tileWidth;

    // Start is called before the first frame update
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
        moveToStart();
        tileHeight = GameManager.Instance.tileHeight;
        tileWidth = GameManager.Instance.tileWidth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkMovementInputs();

    }
    //Moves the player character to the bottom row in the middle/left-mid cell of the grid
    private void moveToStart()  
    {
        int column = Mathf.CeilToInt(GameManager.Instance.columns / 2.0f) - 1; 
        transform.position = (GameObject.Find("PlayingField").GetComponent<MapGrid>().gridToWorldCoords(column, 0));
    }

    private void checkMovementInputs()
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

        //will only move player if they stay within the grid
        Vector2Int coord = MapGrid.Instance.worldToGridCoords(targetPos);
        //Debug.Log(coord.x + "," + coord.y);
        if (coord.x != -1 && !MapGrid.Instance.tiles[coord.x, coord.y].isOccupied())
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






//------------------old movement code----------------------
//private BoxCollider2D boxCollider;
//private Vector3 moveDelta;
//private RaycastHit2D hit;

//float x = Input.GetAxisRaw("Horizontal");
//float y = Input.GetAxisRaw("Vertical");

//moveDelta = new Vector3(x,y,0);

////swap sprite direction
//if (moveDelta.x > 0)
//    transform.localScale = Vector3.one;
//else if (moveDelta.x < 0)
//    transform.localScale = new Vector3(-1, 1, 1);

////can only move in one axis direction at a time. prioritize x axis for 
//if (moveDelta.x != 0)
//    moveHori();
//else if (moveDelta.y != 0)
//    moveVert();


//private void moveVert()
//{
//    hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), GameManager.Instance.tileHeight, LayerMask.GetMask("Actor", "Blocking"));
//    if (hit.collider == null)
//    {
//        transform.Translate(0, tileHeight * moveDelta.y, 0);
//    }
//}

//private void moveHori()
//{
//    hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), GameManager.Instance.tileWidth, LayerMask.GetMask("Actor", "Blocking"));
//    if (hit.collider == null)
//    {
//        transform.Translate(tileWidth * moveDelta.x, 0, 0);
//    }
//}

//-------------------  OLD MOVEMENT (free movement) code: -------------------

////make sure we can move in y direction before we move there
//hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
//if(hit.collider == null)
//{
//    transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
//}

////make sure we can move in x direction before we move there
//hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
//if (hit.collider == null)
//{
//    transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
//}