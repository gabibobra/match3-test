using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics.;
using UnityEngine;

public enum GameState
{
    wait,
    move,
}
public class Board : MonoBehaviour
{

    public GameState currentState = GameState.move;

    public int width;
    public int height;
    public int offSet;
    public int baseGemValue = 20;
    private int combosGemValue = 1;

    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[] gems;
    public GameObject[,] allGems;
    private FindMatch findMatch;
    private UIManager scoreManager;


    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<UIManager>();
        findMatch = FindObjectOfType<FindMatch>();
        //allTiles = new BackgroundTile[width, height];
        allGems = new GameObject[width, height];
        Setup();
    }

    private void Setup()
    {
        //BOARD SET UP, FILL COLUMNS & ROWS
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                //GameObject backgtroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                //backgtroundTile.transform.parent = this.transform;
                //backgtroundTile.name = "( " + i + "," + j + ")";
                int gemToUse = UnityEngine.Random.Range(0, gems.Length);
                int maxIterations = 0;
                while (Matches(i, j, gems[gemToUse]) && maxIterations < 100)
                {
                    gemToUse = UnityEngine.Random.Range(0, gems.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                GameObject gem = Instantiate(gems[gemToUse], tempPosition, Quaternion.identity);
                gem.GetComponent<Gems>().row = j;
                gem.GetComponent<Gems>().column = i;
                gem.transform.parent = this.transform;
                gem.name = "( " + i + "," + j + ")";
                allGems[i, j] = gem;
            }
        }
    }
    private bool Matches(int column, int row, GameObject gem)
    {
        //TEST FOR HORIZONTAL AND VERTICAL MATCHES
        if (column > 1 && row > 1)
        {
            if (allGems[column - 1, row].tag == gem.tag && allGems[column - 2, row].tag == gem.tag)
            {
                return true;
            }
            if (allGems[column, row - 1].tag == gem.tag && allGems[column, row - 2].tag == gem.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allGems[column, row - 1].tag == gem.tag && allGems[column, row - 2].tag == gem.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allGems[column - 1, row].tag == gem.tag && allGems[column - 2, row].tag == gem.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allGems[column, row].GetComponent<Gems>().ismatched)
        {
            findMatch.currentMatches.Remove(allGems[column, row]);
            AudioManager.instance.PlayAudio(Clip.Clear);
            Destroy(allGems[column, row]);
            scoreManager.IncreaseScore(baseGemValue * combosGemValue);
            allGems[column, row] = null;
        }
    }

    public void DestroyAllMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatch.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allGems[i, j].GetComponent<Gems>().row -= nullCount;
                    allGems[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }
    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int gemToUse = UnityEngine.Random.Range(0, gems.Length);
                    GameObject gem = Instantiate(gems[gemToUse], tempPosition, Quaternion.identity);
                    allGems[i, j] = gem;
                    gem.GetComponent<Gems>().row = j;
                    gem.GetComponent<Gems>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    if (allGems[i, j].GetComponent<Gems>().ismatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.3f);

        while (MatchesOnBoard())
        {
            combosGemValue++;
            yield return new WaitForSeconds(.4F);
            DestroyAllMatches();
        }
        yield return new WaitForSeconds(.5f);
        if (IsDeadLocked())
        {
            ShuffleBoard();
            Debug.Log("DEADLOCK");
        }
        yield return new WaitForSeconds(.3f);
        currentState = GameState.move;
        combosGemValue = 1;
    }

    private void SwitchGems(int column, int row, Vector2 direction)
    {
        GameObject holder = allGems[column + (int)direction.x, row + (int)direction.y] as GameObject;
        allGems[column + (int)direction.x, row + (int)direction.y] = allGems[column, row];
        allGems[column, row] = holder;
    }

    private bool CheckForMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    if (i < width - 2)
                    {
                        if (allGems[i + 1, j] != null && allGems[i + 2, j] != null)
                        {
                            if (allGems[i, j].CompareTag(allGems[i + 1, j].tag) && allGems[i, j].CompareTag(allGems[i + 2, j].tag))
                            {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2)
                    {
                        if (allGems[i, j + 1] != null && allGems[i, j + 2] != null)
                        {
                            if (allGems[i, j].CompareTag(allGems[i, j + 1].tag) && allGems[i, j].CompareTag(allGems[i, j + 2].tag))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchGems(column, row, direction);
        if (CheckForMatches())
        {
            SwitchGems(column, row, direction);
            return true;
        }
        SwitchGems(column, row, direction);
        return false;
    }

    private bool IsDeadLocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoard()
    {
        List<GameObject> newBoard = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] != null)
                {
                    newBoard.Add(allGems[i, j]);
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gemToUse = UnityEngine.Random.Range(0, newBoard.Count);
                int maxIterations = 0;
                while (Matches(i, j, newBoard[gemToUse]) && maxIterations < 100)
                {
                   gemToUse = UnityEngine.Random.Range(0, newBoard.Count);
                    maxIterations++;
                }
                Gems gem = newBoard[gemToUse].GetComponent<Gems>();
                maxIterations = 0;
                gem.column = i;
                gem.row = j;
                allGems[i, j] = newBoard[gemToUse];
                newBoard.Remove(newBoard[gemToUse]);
            }

            if (IsDeadLocked())
            {
                ShuffleBoard();
            }

        }
    }
}
