using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D playerBasket; 

	// Use this for initialization
	void Start () {
		playerBasket = GetComponent<Rigidbody2D> ();	
		playerBasket.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");

		Vector3 movement = new Vector3 (moveHorizontal, 0, 0);
		playerBasket.MovePosition (transform.position + movement);
	}
		
}
