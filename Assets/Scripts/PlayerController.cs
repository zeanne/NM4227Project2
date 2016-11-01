using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D playerBasket; 

	public GameObject stunnedClock;

	bool stunned = false;
	float stunTimeLeft;

	// Use this for initialization
	void Start () {
		playerBasket = GetComponent<Rigidbody2D> ();	
		playerBasket.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		if (stunned) {
			stunTimeLeft -= Time.deltaTime;

			if (stunTimeLeft <= 0) {
				stunned = false;
				stunnedClock.SetActive (false);
			}

		} else {
			float moveHorizontal = Input.GetAxis ("Horizontal");

			Vector3 movement = new Vector3 (moveHorizontal, 0, 0);
			playerBasket.MovePosition (transform.position + movement);
		}
	}

	void OnClockCaught() {
		stunned = true;
		stunTimeLeft = 5f;

		stunnedClock.SetActive (true);
	}
}
