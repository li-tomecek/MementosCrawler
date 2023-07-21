using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool isMoving;
    private Vector3 origPos, targetPos;
    public float timeToMove = 5.5f;


    float movementDir;

    // Start is called before the first frame update
    void Start()
    {
        moveToStart();
    }


    //Moves the player character to the bottom row in the middle/left-mid cell of the grid
    private void moveToStart()  
    {
        int column = Mathf.CeilToInt(GameManager.Instance.columns / 2.0f) - 1; 
        transform.position = (GameObject.Find("PlayingField").GetComponent<MapGrid>().gridToWorldCoords(column, 0));
    }

    public void checkInputs()
    {
        //--------player has chosen their location-------------
        if ((GameManager.Instance.getMode() == Mode.BATTLE_MOVE) && (Input.GetKeyDown(KeyCode.Return)))
        {
            GameManager.Instance.setMode(Mode.ACTION_SELECT);
            GameManager.Instance.getMenuManager().getActionSelectMenu().SetActive(true);
            return;
        }

        //--------MOVEMENT---------
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //swap sprite direction
        if (x > 0)
            transform.localScale = Vector3.one;
        else if (x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //actual sprite movement
        if (Input.GetAxisRaw("Vertical") != 0 && !isMoving)
            StartCoroutine(MoveActor(Vector3.up * GameManager.Instance.tileHeight * y));

        if (Input.GetAxisRaw("Horizontal") != 0 && !isMoving)
            StartCoroutine(MoveActor(Vector3.right * GameManager.Instance.tileWidth * x));
        
    }

    private IEnumerator MoveActor(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        //will only move player if they stay within the grid
        Vector2Int coord = MapGrid.Instance.worldToGridCoords(targetPos);
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