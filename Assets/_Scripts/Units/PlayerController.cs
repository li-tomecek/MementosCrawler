using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    [HideInInspector]
    public Direction direction = Direction.S;

    public GameUnit target = null;
    void Start()
    {
        moveToStart();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        direction = Direction.S;
    }

    //----------other methods--------------
    /**Moves the player character to the bottom row in the middle/left-mid cell of the grid**/
    private void moveToStart()  
    {
        int column = Mathf.CeilToInt(GameManager.Instance.columns / 2.0f) - 1; 
        transform.position = (MapGrid.Instance.gridToWorldCoords(column, 0));
        MapGrid.Instance.tiles[column, 0].setTraversible(false);
    }

    public void checkInputs()
    {
        if (GameManager.Instance.getMode() == Mode.ENEMY_TURN || GameManager.Instance.getBattleManager().blockPlayerInputs)
            return;

        //------ACTION INPUTS-------------
        if ((GameManager.Instance.getMode() == Mode.PLAYER_TURN) && (Input.GetKeyDown(KeyCode.Return)))
        {
            GameManager.Instance.setMode(Mode.ACTION_SELECT);
            GameManager.Instance.getMenuManager().getActionSelectMenu().SetActive(true);
            return;
        }

        //--------MOVEMENT INPUTS---------
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //swap sprite direction
        if (x > 0)
        {
            spriteRenderer.flipX = false;
            direction = Direction.E;
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = true;
            direction = Direction.W;
        }
        if (y > 0)
        {
            //change sprite here?
            direction = Direction.N;
        }
        else if (y < 0)
        {
            //change sprite here?
            direction = Direction.S;
        }

        //actual sprite movement
        if (Input.GetAxisRaw("Vertical") != 0 && !isMoving)
        {
            StartCoroutine(MoveActor(Vector3.up * GameManager.Instance.tileHeight * y));
            target = facedTarget();
        }
        if (Input.GetAxisRaw("Horizontal") != 0 && !isMoving)
        {
            StartCoroutine(MoveActor(Vector3.right * GameManager.Instance.tileWidth * x));
            target = facedTarget();
        }



    }

    public GameUnit facedTarget()
    {
       // PlayerController controller = GameManager.Instance.getActivePlayer().GetComponent<PlayerController>();
        Coord adjacent = this.position;
        switch (this.direction)
        {
            case Direction.N:
                adjacent.Y++;
                break;
            case Direction.E:
                adjacent.X++;
                break;
            case Direction.S:
                adjacent.Y--;
                break;
            case Direction.W:
                adjacent.X--;
                break;
        }

        foreach (GameUnit unit in GameManager.Instance.battleManager.activeUnits)
        {
            if (unit.gameObject.GetComponent<UnitController>().position == adjacent)
            {
                if(unit != target)
                    GameManager.Instance.menuManager.sliderCanvas.updateTargetSlider(unit);
                return unit;
            }

        }
        if (target != null)
            GameManager.Instance.menuManager.sliderCanvas.hideTargetSlider();
        return null;
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