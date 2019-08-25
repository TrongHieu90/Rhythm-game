using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check for Fail condition when the music note touches this zone
/// </summary>
public class FailZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
    {
	    if (other.gameObject.CompareTag("MusicNote"))
	    {
		    GameManager.Instance._currentGameState = GameState.Fail;
	    }
    }
}
