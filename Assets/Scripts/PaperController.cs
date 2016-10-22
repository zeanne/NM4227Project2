using UnityEngine;
using System.Collections;

public class PaperController : MonoBehaviour {

	private Rigidbody2D rb2d;

	private float YFORCE = 0.04f;
	private float XMAX = 0.01f;
	private float rotationAngle = 5;
	private bool fading = false;
	private bool counted = false;

	private string TAG_PLAYER = "Player";
	private string TAG_FLOOR = "Floor";


	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

		Vector2 f = new Vector2 (Random.value * 2 * XMAX - XMAX, YFORCE);
		rb2d.AddForce(f);
	}
	
	// Update is called once per frame
	void Update () {
		Rotate ();	

		if (fading) {
			Color tempColor = this.gameObject.GetComponent<SpriteRenderer> ().color;
			tempColor.a -= 0.05f;

			if (tempColor.a < 0.5) {
				Destroy (this.gameObject);
			} else {
				this.gameObject.GetComponent<SpriteRenderer> ().color = tempColor;
			}
		}
	}

	void Rotate() {
		rb2d.transform.Rotate (new Vector3 (0, 0, rotationAngle));
	}

	void Fade() {
		rotationAngle = 0;
		fading = true;
		counted = true;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!counted) {
			
			if (other.gameObject.CompareTag (TAG_PLAYER)) {
				Fade ();
				this.SendMessageUpwards ("OnPaperCaught");
			} else if (other.gameObject.CompareTag (TAG_FLOOR)) {
				Fade ();
				this.SendMessageUpwards ("OnPaperDropped");
			}
		}
	}

}
