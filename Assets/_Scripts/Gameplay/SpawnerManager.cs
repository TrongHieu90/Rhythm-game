using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
	private GameObject _notePrefab;
	private float _noteSpeed;
	private int _noteQuantity;

	private MeshRenderer _planeMeshRenderer;
	private float[] _lanePos;
	private MeshRenderer _noteMeshRenderer;

	[SerializeField] private Transform _spawnStartLocation;
	[SerializeField] private Transform _noteContainer;

	[SerializeField] private GameDataSettings _gameDataSettings;

	void Start()
	{
		SetUpNoteParameter();
		SetupSpawningPlane();

		MusicNoteManager.Instance.MusicNoteGameObjects = new List<GameObject>(_noteQuantity);
	}

	private void SetUpNoteParameter()
	{
		if (!_gameDataSettings)
		{
			Debug.LogError("Must set up the GameDataSettings Scriptable Object");
		}

		_notePrefab = _gameDataSettings.notePrefab;
		_notePrefab.transform.localScale = new Vector3(0.5f, 0.5f, _gameDataSettings.NoteSize);
		_noteSpeed = _gameDataSettings.NoteSpeed;
		_noteQuantity = _gameDataSettings.NoteQuantity;
		
		_noteMeshRenderer = (_notePrefab)?.GetComponent<MeshRenderer>();
	}


	public IEnumerator SpawningMusicNote()
	{
		for (int i = 0; i < _noteQuantity; i++)
		{
			var noteZPosition = (i == 0) 
				              ? _spawnStartLocation.position.z
				              : MusicNoteManager.Instance.MusicNoteGameObjects[i - 1].transform.position.z + _noteMeshRenderer.bounds.max.z * 2;
			int randomValue = Random.Range(0, _lanePos.Length);

			var currentNote = GameObject.Instantiate(_notePrefab,
				new Vector3(_lanePos[randomValue], 0.2f, noteZPosition), Quaternion.identity);
			currentNote.name = $"Note: {i}";
			currentNote.transform.parent = _noteContainer;

			NoteDataPopulate(currentNote, i, _noteSpeed);

			yield return new WaitForEndOfFrame();
			currentNote.GetComponent<MusicNote>().m_MusicNoteData.CurrentMusicNoteState = MusicNoteState.Ready;

			MusicNoteManager.Instance.MusicNoteGameObjects.Add(currentNote);
			yield return null;
		}
	}

	private void SetupSpawningPlane()
	{
		_planeMeshRenderer = GetComponent<MeshRenderer>();

		var boundMinPoint = _planeMeshRenderer.bounds.min;
		var planeSize = _planeMeshRenderer.bounds.size.x / 4;

		//Make 4 lanes to spawn the music notes with padding space
		_lanePos = new float[4];
		for (int i = 0; i < _lanePos.Length; i++)
		{
			_lanePos[i] = (i == 0) 
						? _planeMeshRenderer.bounds.min.x + (planeSize / 2) 
						: _lanePos[i - 1] + planeSize; ;
		}
	}

	private void NoteDataPopulate(GameObject note, int noteIndex, float speed)
	{
		var currentNote = note.GetComponent<MusicNote>();
		currentNote.m_MusicNoteData = new MusicNote.MusicNoteData
		{
			NoteIndex = noteIndex,
			Speed = speed,
			Position = note.transform.position,
			CurrentMusicNoteState = MusicNoteState.Spawned,
			InBonusZone = false
		};
	}

	
}