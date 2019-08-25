using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState { Start, Playing, Fail, Win}
public class GameManager : Singleton<GameManager>
{
	public GameState _currentGameState;
	private GameState cachedCurrentState;
	[SerializeField]private HighScoreManager _highScoreManager;

	[SerializeField]private GameObject _levelFailPanel;
	[SerializeField]private GameObject _levelWinPanel;
	[SerializeField] private TextMeshProUGUI _winText;
	[SerializeField]private SpawnerManager _spawnerManager;

	void Awake()
	{
		_currentGameState = GameState.Start;
		cachedCurrentState = _currentGameState;
	}

	void Start()
    {
	    OnGameStateStart();
    }

    void Update()
    {
	    if (_currentGameState != cachedCurrentState)
	    {
			OnStateChange(_currentGameState);
			cachedCurrentState = _currentGameState;
	    }
    }

    private void OnStateChange(GameState currentState)
    {
	    switch (currentState)
	    {
		    case GameState.Start:
			{
				OnGameStateStart();
				break;
			}
			case GameState.Playing:
		    {
			    OnGameStatePlaying();
			    break;
		    }

			case GameState.Fail:
			{
				OnGameStateFail();
				break;
			}

		    case GameState.Win:
		    {
			    OnGameStateWin();
			    break;
		    }
	    }
	}

    private void OnGameStateStart()
    {
	    if (_levelFailPanel.activeInHierarchy)
	    {
		    _levelFailPanel.SetActive(false);
	    }
	    if (_levelWinPanel.activeInHierarchy)
	    {
		    _levelWinPanel.SetActive(false);
	    }
		Time.timeScale = 1;
    }
    private void OnGameStatePlaying()
    {
	    
	}
	private void OnGameStateFail()
    {
	    if (!_levelFailPanel.activeInHierarchy)
	    {
		    _levelFailPanel.SetActive(true);
	    }

	    AudioManager.Instance.Stop("MainTheme");
	    AudioManager.Instance.Play("NoteOnHit", 0.5f);

		Time.timeScale = 0;
    }
    private void OnGameStateWin()
    {
	    if (!_levelWinPanel.activeInHierarchy)
	    {
		    _levelWinPanel.SetActive(true);
	    }

	    if (_highScoreManager.TotalScore > PlayerPrefs.GetInt("HighScoreRecord"))
	    {
		    PlayerPrefs.SetInt("HighScoreRecord", _highScoreManager.TotalScore);
		}

	    _winText.text = $"Level Wins! \n " +
	                    $"Last Highscore: {PlayerPrefs.GetInt("HighScoreRecord")} \n" +
	                    $"Get Higher Score?";

	    Time.timeScale = 0;
	}

    public void OnResetButtonPressed()
    {
		//Reload current active scene
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void OnStartButtonPressed()
    {
	    _currentGameState = GameState.Playing;
	    StartCoroutine(_spawnerManager.SpawningMusicNote());
	}

	public void OnExitButtonPressed()
	{
		Application.Quit();
	}
}
