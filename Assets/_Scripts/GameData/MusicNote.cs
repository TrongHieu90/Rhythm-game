using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MusicNoteState { Spawned, Ready, Clicked }
public class MusicNote : MonoBehaviour
{
	[System.Serializable]
	public struct MusicNoteData
	{
		public float Speed;
		public int NoteIndex;
		public Vector3 Position;
		public MusicNoteState CurrentMusicNoteState;
		public bool InBonusZone;

		public MusicNoteData(int noteIndex, float speed, Vector3 position, MusicNoteState musicNoteState, bool inBonusZone)
		{
			NoteIndex = noteIndex;
			Speed = speed;
			Position = position;
			CurrentMusicNoteState = musicNoteState;
			InBonusZone = inBonusZone;
		}
	}

	public MusicNoteData m_MusicNoteData;

	#region Shader Properties

	private float _dissolveSpeed = 1.0f;
	private MaterialPropertyBlock _propBlock;
	private Renderer _renderer;
	[Tooltip("The time duration of dissolve when a music note is clicked on")]
	[SerializeField] private float _dissolveDuration = 2f;
	private float _smoothness = 0.02f; 

	#endregion

	private void Start()
	{
		_propBlock = new MaterialPropertyBlock();
		_renderer = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		if (m_MusicNoteData.CurrentMusicNoteState != MusicNoteState.Spawned)
		{
			transform.Translate(-Vector3.forward * m_MusicNoteData.Speed * Time.deltaTime);
		}
	}

	public IEnumerator DissolveOnClick()
	{
		float progress = 0;
		float increment = _smoothness / _dissolveDuration;
		_renderer.GetPropertyBlock(_propBlock);

		while (progress < 1)
		{
			//0 means full opaque, 1 means full dissolved. Lerp in between to show the dissolve effect
			_propBlock.SetFloat("_Amount", Mathf.Lerp(0, 1, progress));
			progress += increment;
			_renderer.SetPropertyBlock(_propBlock);

			yield return new WaitForSeconds(_smoothness);
		}

		//Todo: implement ObjectPool to handle this
		this.gameObject.SetActive(false);
	}
}


