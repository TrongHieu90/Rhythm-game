using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bonus zone where when clicked will get multiplier on highscore
/// </summary>
public class BonusZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("MusicNote"))
		{
			var musicNoteHit = other.gameObject.GetComponent<MusicNote>();
			musicNoteHit.m_MusicNoteData.InBonusZone = true;
		}
	}
}
