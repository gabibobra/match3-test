using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private Board board;
    private UIManager scoreManager;
    public int goal;
    private DataController dataController;

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
        board = FindObjectOfType<Board>();
        scoreManager = FindObjectOfType<UIManager>();

    }
    public void ResetGoals()
    {

        dataController.ResetGoals();
        SceneManager.LoadScene("Game");
    }

    public void RestartWithNewGoal()
    {
        if(scoreManager.buttonWinText.text == "New Goal")
        {
            dataController.SubmitPlayerGoal(scoreManager.goalScore + 100);
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }     
    }

}
