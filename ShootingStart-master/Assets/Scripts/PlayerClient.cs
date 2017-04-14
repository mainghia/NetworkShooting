using UnityEngine;
using System.Collections;

public class PlayerClient : MonoBehaviour {
	public float moveSpeed = 650;
	public float jumpHeight = 400;
	public float timeToJumpApex = .4f;
	public float shootCooldownTime = .5f;




	Vector2 cursorPos;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
	bool canShoot = true;
	float maxShootRange = 9999;

	Collider2D coll;
	Controller2D controller;
	public InputManager inputManager;

	PlayerNetwork player;

	void Start() {
		inputManager = InputManager.Instance;
		player = GetComponent<PlayerNetwork> ();
		controller = GetComponent<Controller2D> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		coll = GetComponent<Collider2D>();

		if(inputManager != null)
		{
			inputManager.OnInputXChanged += OnInputXChanged;
			inputManager.OnJumpButtonPressed += OnJumpPressed;
			//  inputManager.OnShootButtonPressed += OnShootPressed;
			inputManager.OnCursorMoved += OnCursorMoved;
		}
	}
	private void OnInputXChanged(float inputX)
	{
		float targetVelocityX = inputX * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
	}

	private void OnJumpPressed()
	{
		if (controller.collisions.below)
		{
			velocity.y = jumpVelocity;
		}
	}

	//    private void OnShootPressed()
	//    {
	//        Vector2 direction = cursorPos - (Vector2)gunTransform.position;
	//        RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, direction, maxShootRange, platformLayerMask);
	//
	//        float shootRange = hit ? hit.distance : maxShootRange;
	//        RaycastHit2D[] hits = Physics2D.RaycastAll(gunTransform.position, direction, shootRange, playerLayerMask);
	//
	//        foreach(RaycastHit2D playerHit in hits)
	//        {
	//            if (playerHit.collider == coll) continue;
	//
	//            playerHit.collider.GetComponent<Player>().Die();
	//        }
	//
	//        Vector2 to = (Vector2)gunTransform.position + direction.ScaleTo(shootRange);
	//        RayFactory.Instance.NewRayFromTo(gunPointTransform.position, to);
	//    }
	//
	    private void OnCursorMoved(Vector2 mousePos)
	    {
	        cursorPos = mousePos;
			player.CmdChangeCursorPosition (cursorPos);
	        Aim();
	    }
	
	    private void Aim()
	    {
			Vector2 direction = cursorPos - (Vector2) player.gunTransform.position;
	        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	        player.gunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	        if (angle > 360) angle -= 360;
	        if (angle < 0) angle += 360;
	        if (angle > 90 && angle < 270) player.bodyTransform.localScale = new Vector3(-1, 1, 1);
	        else player.bodyTransform.localScale = Vector3.one;
	    }
    
	void FixedUpdate() {
		if (!coll.enabled) return;

		if (controller.collisions.above || (controller.collisions.below && velocity.y < 0)) {
			velocity.y = 0;
		}

		velocity.y += gravity * Time.fixedDeltaTime;
		controller.Move (velocity * Time.fixedDeltaTime);
		player.CmdChangePosition (transform.position);
	}
}
