using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickInputController : MonoBehaviour
{
	[SerializeField] private LayerMask hitableLayerMask;
	[SerializeField] private HighScoreManager _highScoreManager;

    void Update()
    {
	    DectectMusicNoteOnScreen();
	}

	private void DectectMusicNoteOnScreen()
    {
		//Make sure when we click on UI at the Gamestate.Start it wont fail the game
	    if (GameManager.Instance._currentGameState != GameState.Playing) return;

#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0))
#else
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
#endif
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100f, hitableLayerMask))
			{
				if (hit.collider.CompareTag("MusicNote"))
				{
					
					//Debug.Log("Music Note hit");
					var musicNoteHit = hit.collider.gameObject.GetComponent<MusicNote>();
					
					var noteIndex = musicNoteHit.m_MusicNoteData.NoteIndex;

					if (noteIndex == 0 ||
						//TODO: insane code smell, need to refactor
						//if not first note, need to check if previous note state is clicked 
						MusicNoteManager.Instance.MusicNoteGameObjects[noteIndex - 1].GetComponent<MusicNote>()
						    .m_MusicNoteData.CurrentMusicNoteState == MusicNoteState.Clicked)
					{
						ClickInteraction(hit, musicNoteHit, Random.Range(1f, 3f));
					}
					if (noteIndex == MusicNoteManager.Instance.MusicNoteGameObjects.Count - 1)//final music note
					{
						ClickInteraction(hit, musicNoteHit, 1f);
						GameManager.Instance._currentGameState = GameState.Win;
					}
				}
				else
				{
					_highScoreManager.ResetMultiplier();
					GameManager.Instance._currentGameState = GameState.Fail;
				}
			} 
		}
	}

	private void ClickInteraction(RaycastHit hit, MusicNote musicNoteHit, float soundPitch)
	{
		if (musicNoteHit.m_MusicNoteData.InBonusZone)
		{
			_highScoreManager.UpdateMultiplier();
		}
		else
		{
			_highScoreManager.ResetMultiplier();
		}

		//turn off collider so we can NOT click on the note 2 times in a row
		hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;

		musicNoteHit.m_MusicNoteData.CurrentMusicNoteState = MusicNoteState.Clicked;
		StartCoroutine(musicNoteHit.DissolveOnClick());

		AudioManager.Instance.Play("NoteOnHit", soundPitch);

		_highScoreManager.UpdateScore();
	}
}
