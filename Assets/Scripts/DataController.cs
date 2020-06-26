using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    private PlayerProgress playerProgress;

    private int goalRset = 100;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadPlayerGoals();
    }
    public void SubmitPlayerGoal(int newGoal)
    {
        if (newGoal >= playerProgress.scoreGoal)
        {
            playerProgress.scoreGoal = newGoal;
            SavePlayerGoals();
        }
        else
        {
            SavePlayerGoals();
        }

    }
    public int GetGoal()
    {
        return playerProgress.scoreGoal;
    }
    private void LoadPlayerGoals()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("GoalScore"))
        {
            playerProgress.scoreGoal = PlayerPrefs.GetInt("GoalScore");
        }
    }
    private void SavePlayerGoals()
    {
        PlayerPrefs.SetInt("GoalScore", playerProgress.scoreGoal);
    }

    public void DeleteGoals()
    {

        LoadPlayerGoals();
        SavePlayerGoals();
    }

    public void ResetGoals()
    {
        StartCoroutine(ResetGoalCo());
    }
    private IEnumerator ResetGoalCo()
    {
      playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("GoalScore"))
        {
            playerProgress.scoreGoal = goalRset;
        }
        yield return new WaitForSeconds(.6f);
        SavePlayerGoals();

    }
}