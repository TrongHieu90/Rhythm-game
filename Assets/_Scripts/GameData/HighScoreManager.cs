using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _highscoreText;
	[SerializeField] private TextMeshProUGUI _multiplierText;

	public int TotalScore = 0;
	[SerializeField] private int ScorePerNote = 1;
	private int CurrentMultiplier = 1;
	private int MultiplierTracker = 0;
	[Tooltip("How many notes we have to successfully clicked to reach new multiplier")]
	[SerializeField] private int[] MultiplerThresholds;

	private bool _scoreChanged;

	private void Start()
	{
		_highscoreText.text = TotalScore.ToString();
	}

	private void Update()
	{
		if (_scoreChanged)
		{
			_highscoreText.text = TotalScore.ToString();

			if (CurrentMultiplier > 1)
			{
				_multiplierText.text = "x" + CurrentMultiplier.ToString();
			}
			_scoreChanged = false;
		}
	}

	public int UpdateScore()
	{
		_scoreChanged = true;
		return TotalScore += ScorePerNote * CurrentMultiplier;
	}

	public void UpdateMultiplier()
	{
		if (CurrentMultiplier - 1 < MultiplerThresholds.Length)
		{
			MultiplierTracker++;
			if (MultiplerThresholds[CurrentMultiplier - 1] <= MultiplierTracker)
			{
				MultiplierTracker = 0;
				CurrentMultiplier++;
			}
		}
	}

	public void ResetMultiplier()
	{
		MultiplierTracker = 0;
		CurrentMultiplier = 1;
		_multiplierText.text = "";
	}

}
