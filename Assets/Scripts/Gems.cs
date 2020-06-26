using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gems : MonoBehaviour
{
    private Vector2 firstTouch;
    private Vector2 finalTouch;
    private Vector2 tempPosition;

    public float swapAngle = 0;
    public float swapResist = .5f;


    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int xtarget;
    public int ytarget;

    public bool ismatched = false;

    private Board board;
    public GameObject targetgem;
    private FindMatch findMatches;

    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatch>();
     
    }
    void Update()
    {

        xtarget = column;
        ytarget = row;
        if (Mathf.Abs(xtarget - transform.position.x) > .1)
        {
            tempPosition = new Vector2(xtarget, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allGems[column, row] != this.gameObject)
            {

                board.allGems[column, row] = this.gameObject;
                AudioManager.instance.PlayAudio(Clip.Swap);
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(xtarget, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(ytarget - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, ytarget);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allGems[column, row] != this.gameObject)
            {
                board.allGems[column, row] = this.gameObject;
                AudioManager.instance.PlayAudio(Clip.Swap);
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, ytarget);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMovesCo()
    {
        yield return new WaitForSeconds(.5f);
        if (targetgem != null)
        {
            if (!ismatched && !targetgem.GetComponent<Gems>().ismatched)
            {
                targetgem.GetComponent<Gems>().row = row;
                targetgem.GetComponent<Gems>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyAllMatches();
            }
            targetgem = null;
        }
    }

    private void OnMouseDown()
    {
        //SAVES MOUSE FIRST POSITION
        if (board.currentState == GameState.move)
        {

            firstTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        //SAVES MOUSE FINAL POSITION
        if (board.currentState == GameState.move)
        {
            finalTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FindAngles();
        }
    }

    void FindAngles()
    {
        //TESTS IF VALIDY MOVE **AVOID ERRORS FROM MISSCLICKS**
        if (Mathf.Abs(finalTouch.y - firstTouch.y) > swapResist || Mathf.Abs(finalTouch.x - firstTouch.x) > swapResist)
        {
            board.currentState = GameState.wait;
            swapAngle = Mathf.Atan2(finalTouch.y - firstTouch.y, finalTouch.x - firstTouch.x) * Mathf.Rad2Deg;
            Movegems();
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void Movegems()
    {
        //RIGHT SWAP
        if (swapAngle > -45 && swapAngle <= 45 && column < board.width - 1)
        {
            targetgem = board.allGems[column + 1, row];
            previousColumn = column;
            previousRow = row;
            targetgem.GetComponent<Gems>().column -= 1;
            column += 1;
            StartCoroutine(CheckMovesCo());
        }
        //LEFT SWAP
        else if ((swapAngle > 135 || swapAngle <= -135) && column > 0)
        {
            targetgem = board.allGems[column - 1, row];
            previousColumn = column;
            previousRow = row;
            targetgem.GetComponent<Gems>().column += 1;
            column -= 1;
            StartCoroutine(CheckMovesCo());
        }
        //UP SWAP
        else if (swapAngle > 45 && swapAngle <= 135 && row < board.height - 1)
        {
            targetgem = board.allGems[column, row + 1];
            previousColumn = column;
            previousRow = row;
            targetgem.GetComponent<Gems>().row -= 1;
            row += 1;
            StartCoroutine(CheckMovesCo());
        }
        // DOWN SWAP
        else if (swapAngle < -45 && swapAngle >= -135 && row > 0)
        {
            targetgem = board.allGems[column, row - 1];
            previousColumn = column;
            previousRow = row;
            targetgem.GetComponent<Gems>().row += 1;
            row -= 1;
            StartCoroutine(CheckMovesCo());
        }
        board.currentState = GameState.move;
    }
}


