using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	public GameObject gameOverPanel;
	public DataController dataController;

	public Text yourScoreTxt;
	public Text goalScoreTxt;
	public Text buttonWinText;
	public Text buttonLoseText;
	public Text checkText;
	public Text scoreTxt;
	public Text timeLeftTxt;

	public Button winButton;
	public Button loseButton;

	private float timeLeft = 120;
	public int score;
	public int goalScore;
	public int newGoal;

	public void Start()
	{
		gameOverPanel.SetActive(false);
		dataController = FindObjectOfType<DataController>();
		instance = GetComponent<UIManager>();
		timeLeftTxt.text = timeLeft.ToString();
		goalScore = dataController.GetGoal();
	}
	public void Update()
	{
		timeLeft -= Time.deltaTime;
		timeLeftTxt.text = Mathf.Round(timeLeft).ToString();
		if (timeLeft < 0)
		{
			GameOver();
			timeLeftTxt.text = "0";

		}
		scoreTxt.text = "" + score;
	}

	public void IncreaseScore(int points)
    {
		score += points;
    }

	// Show the game over panel
	public void GameOver()
	{
		gameOverPanel.SetActive(true);
		winButton.gameObject.SetActive(true);
		loseButton.gameObject.SetActive(true);
		goalScoreTxt.text = "Goal Score: " + goalScore.ToString();
		yourScoreTxt.text = "Your Score: " + score.ToString();
			if(score >= goalScore)
			{
				dataController.SubmitPlayerGoal(goalScore);
				checkText.text = "You Win";
				buttonWinText.text = "New Goal";
			return;
			}
			else
			{
				checkText.text = "You Lose";
				buttonWinText.text = "Try Again";
			}
	}	
}