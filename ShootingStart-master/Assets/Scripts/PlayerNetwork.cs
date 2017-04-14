using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof (Controller2D))]
public class PlayerNetwork : NetworkBehaviour {
 
	public Transform bodyTransform;
	public Transform gunTransform;
	public Transform gunPointTransform;
	public LayerMask platformLayerMask;
	public LayerMask playerLayerMask;

	//Networking Variables
	PlayerClient client;
	PlayerObserver observer;
	PlayerServer server;

	[SyncVar] public Vector3 currentPosition;
	[SyncVar] public Vector3 currentCursorPosition;


//    public void Die()
//    {
//        print("Dead");
//        coll.enabled = false;
//        if (inputManager != null)
//        {
//            inputManager.OnInputXChanged -= OnInputXChanged;
//            inputManager.OnJumpButtonPressed -= OnJumpPressed;
//            inputManager.OnShootButtonPressed -= OnShootPressed;
//            inputManager.OnCursorMoved -= OnCursorMoved;
//        }
//
//        StartCoroutine(AnimateDead());
//    }
//
//    private IEnumerator AnimateDead()
//    {
//        float time = 0;
//        while(time < 0.5f)
//        {
//            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathfx.Sinerp(0, 1, time / 0.5f));
//
//            time += Time.deltaTime;
//            yield return null;
//        }
//
//        gameObject.SetActive(false);
//    }

	void Start() {
		if (isLocalPlayer) {
			client = gameObject.AddComponent<PlayerClient> ();
		} else {
			observer = gameObject.AddComponent<PlayerObserver> ();
		}
		if (isServer) {
			server = gameObject.AddComponent<PlayerServer> ();
		}
	}

	[Command]
	public void CmdChangePosition(Vector3 position){
		server.UpdatePosition(position);
	}

	[Command]
	public void CmdChangeCursorPosition(Vector3 CursorPosition){
		server.UpdateCursorPosition(CursorPosition);
	}
  




}
