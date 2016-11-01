using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PaperGenerator : MonoBehaviour {

	public GameObject paperPrefab;
	public GameObject clockPrefab;
	public GameObject broomPrefab;

	public GameObject paperBackground;

	public Slider angstBar;
	public Text caughtText;
	public GameObject gameOverBox;

	private Sprite[] wastePaperArray;

	public float nextTimeToPaperSpawn;

	private float timeToSpawn;
	private int paperCaught = 0;
	private int objectDropped = 0;

	private float BAR_ADD_CATCH = 2;
	private float BAR_ADD_DROP = 8;
	private float BAR_ADD_DROP_GRADUAL = 1f;
	private float BAR_ADD_DROP_MIN = 4;
	private float BAR_GRADUAL = 0.02f;
	private float BAR_MAX = 100;
	private float BAR_MIN = 0;

	private int spriteNum = 0;
	private int spawnNec = 0;

	// Use this for initialization
	void Start () {
		timeToSpawn = 1f;

		angstBar.minValue = BAR_MIN;
		angstBar.maxValue = BAR_MAX;
		angstBar.value = BAR_MIN;
	
		caughtText.text = "Caught: 0...";

		wastePaperArray = Resources.LoadAll<Sprite> ("singlewaste");
	}

	// Update is called once per frame
	void Update () {
		timeToSpawn -= Time.deltaTime;
		if (timeToSpawn < 0) {

			Sprite nextColor = wastePaperArray [Random.Range (0, wastePaperArray.Length - 1)];
			paperPrefab.GetComponent<SpriteRenderer> ().sprite = nextColor;

			GameObject paper = Instantiate (paperPrefab);
			paper.transform.SetParent (this.transform);
			timeToSpawn = nextTimeToPaperSpawn;
		}

		SetPaperFillBackground ();

		angstBar.value -= BAR_GRADUAL;

		// spawn tissue / toothbrush / soap in 3 frames
		switch (spawnNec) {
		case 1 :
			Sprite soapSprite = Resources.Load<Sprite> ("soap");
			GameObject soap = Instantiate (paperPrefab);
			soap.GetComponent<SpriteRenderer> ().sprite = soapSprite;
			soap.transform.SetParent (this.transform);
			spawnNec++;
			break;
		case 2 :
			Sprite tissueSprite = Resources.Load<Sprite> ("tissue");
			GameObject tissue = Instantiate (paperPrefab);
			tissue.GetComponent<SpriteRenderer> ().sprite = tissueSprite;
			tissue.transform.SetParent (this.transform);
			spawnNec++;
			break;
		case 3 :
			Sprite brushSprite = Resources.Load<Sprite> ("toothbrush");
			GameObject brush = Instantiate (paperPrefab);
			brush.GetComponent<SpriteRenderer> ().sprite = brushSprite;
			brush.transform.SetParent (this.transform);
			spawnNec++;
			break;
		default :
			spawnNec = 0;
			break;
		}
	}
				
	void OnPaperCaught() {
		paperCaught++;
		caughtText.text = "Caught: " + paperCaught.ToString ();

		int exclaim = paperCaught;
		while (exclaim >= 15) {
			caughtText.text = caughtText.text + "!";
			exclaim -= 15;
		}

		if (angstBar.value > 0.8 * BAR_MAX) {
		} else {
			angstBar.value += BAR_ADD_CATCH;
		}
	}

	void OnBroomCaught() {
		objectDropped -= 10;
	}

	void OnObjectDropped() {
		objectDropped++;


		if (angstBar.value > 0.8 * BAR_MAX) {
		} else {
			angstBar.value += BAR_ADD_DROP;
		}

		BAR_ADD_DROP -= BAR_ADD_DROP_GRADUAL;
		BAR_ADD_DROP = Mathf.Max (BAR_ADD_DROP, BAR_ADD_DROP_MIN);

	}

	void OnBarMax() {
//		Time.timeScale = 0;
//		BAR_GRADUAL = 0;
	}

	void StartGame() {
		paperCaught = 0;
		objectDropped = 0;

		caughtText.text = "Caught: 0...";
	}

	void SpawnPhoto() {
		Sprite photoSprite = Resources.Load<Sprite> ("family portrait");
		GameObject photo = Instantiate (paperPrefab);
		photo.GetComponent<SpriteRenderer> ().sprite = photoSprite;
		Debug.Log (photo.transform.localScale);
		photo.transform.localScale.Scale (new Vector3 (4f, 4f, 4f));

		photo.transform.SetParent (this.transform);
	}

	void SpawnNecessities() {
		spawnNec = 1;
	}

	void SpawnClock() {
		GameObject clock = Instantiate (clockPrefab);
		clock.transform.SetParent (this.transform);
	}

	void SpawnBroom() {
		GameObject broom = Instantiate (broomPrefab);
		broom.transform.SetParent (this.transform);
	}

	// decrease time to next spawn of paper
	void IncreasePaperGeneration() {
		nextTimeToPaperSpawn -= 0.1f;
		nextTimeToPaperSpawn = Mathf.Max (0.25f, nextTimeToPaperSpawn);
	}

	void SetNextTimeToPaperSpawn(float time) {
		nextTimeToPaperSpawn = time;

		if (nextTimeToPaperSpawn < timeToSpawn) {
			timeToSpawn = nextTimeToPaperSpawn;
		}
	}

	// fills up the screen with paper depending on amount of objects dropped.
	void SetPaperFillBackground() {
		spriteNum = Mathf.Min(objectDropped / 5, 10);
		Sprite ppbg = Resources.Load<Sprite> ("papercover" + spriteNum);

		if (spriteNum == 0) {
		} else if (ppbg != null) {
			paperBackground.GetComponent<SpriteRenderer> ().sprite = ppbg;
			if (spriteNum == 10) {
				angstBar.value = BAR_MAX;
				OnBarMax ();
				gameOverBox.gameObject.SetActive (true);
			}
		}
	}
}
