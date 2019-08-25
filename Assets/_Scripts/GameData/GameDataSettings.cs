using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "ScriptableObjects/GameSettingsData", order = 1)]
public class GameDataSettings : ScriptableObject
{
	[Tooltip("The prefab of the music note")]
	public GameObject notePrefab;
	[Tooltip("The falling speed of the music note")]
	public float NoteSpeed;
	[Tooltip("The length of the music note in z axis")]
	public float NoteSize;
	[Tooltip("How many notes will be spawn")]
	public int NoteQuantity;
}
