using UnityEngine;
using System.Collections;

public class PlayerServer : MonoBehaviour {
	PlayerNetwork player;

	void Start() {
		player = GetComponent<PlayerNetwork> ();

	}


	public void UpdatePosition (Vector3 position)
	{
		player.currentPosition = position;
	}

	public void UpdateCursorPosition (Vector3 cursorPosition)
	{
		player.currentCursorPosition = cursorPosition;
	}
}
