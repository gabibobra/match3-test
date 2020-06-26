using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class FindMatch : MonoBehaviour
{

    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }


    //private List<GameObject> GetNearbyGems(GameObject gem1, GameObject gem2, GameObject gem3)
    //{
    //    if(!currentMatches.Contains(gem1))
    //                            {
    //        currentMatches.Add(gem1);
    //    }
    //    gem1.GetComponent<Gems>().ismatched = true;
    //    if (!currentMatches.Contains(gem2))
    //    {
    //        currentMatches.Add(gem2);
    //    }
    //    gem2.GetComponent<Gems>().ismatched = true;
    //    if (!currentMatches.Contains(gem3))
    //    {
    //        currentMatches.Add(currentGem);
    //    }
    //    currentGem

    //}
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentGem = board.allGems[i, j];
                if(currentGem != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftGem = board.allGems[i - 1, j];
                        GameObject rigthGem = board.allGems[i + 1, j];
                        if(leftGem != null && rigthGem != null)
                        {
                            if(leftGem.tag == currentGem.tag && rigthGem.tag == currentGem.tag)
                            {
                                if (!currentMatches.Contains(leftGem))
                                {
                                    currentMatches.Add(leftGem);
                                }
                                leftGem.GetComponent<Gems>().ismatched = true;
                                if (!currentMatches.Contains(rigthGem))
                                {
                                    currentMatches.Add(rigthGem);
                                }
                                rigthGem.GetComponent<Gems>().ismatched = true;
                                if (!currentMatches.Contains(currentGem))
                                {
                                    currentMatches.Add(currentGem);
                                }
                                currentGem.GetComponent<Gems>().ismatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upGem = board.allGems[i, j + 1];
                        GameObject downGem = board.allGems[i, j - 1];
                        if (upGem != null && downGem != null)
                        {
                            if (upGem.tag == currentGem.tag && downGem.tag == currentGem.tag)
                            {
                                if (!currentMatches.Contains(upGem))
                                {
                                    currentMatches.Add(upGem);
                                }
                                upGem.GetComponent<Gems>().ismatched = true;
                                if (!currentMatches.Contains(downGem))
                                {
                                    currentMatches.Add(downGem);
                                }
                                downGem.GetComponent<Gems>().ismatched = true;
                                if (!currentMatches.Contains(currentGem))
                                {
                                    currentMatches.Add(currentGem);
                                }
                                currentGem.GetComponent<Gems>().ismatched = true;
                            }
                        }
                    }
                }
            }
        }
    }

}
