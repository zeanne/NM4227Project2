using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PaperGenerator : MonoBehaviour {

	public GameObject paperPrefab;
	public GameObject clockPrefab;
	public GameObject broomPrefab;
	public GameObject photoPrefab;
	public GameObject neccPrefab;

	public GameObject paperBackground;

	public Slider angstBar;
	public Text caughtText;
	public GameObject gameOverBox;
	public GameObject canvasDimmer;

	private Sprite[] wastePaperArray;

	public float nextTimeToPaperSpawn;

	private float timeToSpawn;
	private int paperCaught = 0;
	private int objectDropped = 0;
	private int objectDropRate = 1;

	private float BAR_ADD_CATCH = 2;
	private float BAR_ADD_DROP = 8;
	private float BAR_ADD_DROP_GRADUAL = 1f;
	private float BAR_ADD_DROP_MIN = 4;
	private float BAR_GRADUAL = 0.02f;
	private float BAR_MAX = 100;
	private float BAR_MIN = 0;

	private int spriteNum = 0;
	private int spawnNec = 0;
	private bool epilogued = false;

	public GameObject canvasToDim;
	bool dimLights;

	// Use this for initialization
	void Start () {
		timeToSpawn = 60f;

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

		if (dimLights && canvasToDim.GetComponent<RawImage>().color.a <= 0.6) {
			Color temp = canvasToDim.GetComponent<RawImage>().color;
			temp.a += 0.2f;
			canvasToDim.GetComponent<RawImage> ().color = temp;
		}

		// spawn tissue / toothbrush / soap in 3 frames
		switch (spawnNec) {
		case 1:
			Sprite soapSprite = Resources.Load<Sprite> ("soap");
			GameObject soap = Instantiate (neccPrefab);
			soap.GetComponent<SpriteRenderer> ().sprite = soapSprite;
			soap.transform.SetParent (this.transform);
			spawnNec = 0;
			Invoke ("AddNecSpawn", 0.2f);
			Invoke ("AddNecSpawn", 0.2f);
			break;
		case 2 :
			Sprite tissueSprite = Resources.Load<Sprite> ("tissue");
			GameObject tissue = Instantiate (neccPrefab);
			tissue.GetComponent<SpriteRenderer> ().sprite = tissueSprite;
			tissue.transform.SetParent (this.transform);
			spawnNec = 0;
			Invoke ("AddNecSpawn", 0.2f);
			Invoke ("AddNecSpawn", 0.2f);
			Invoke ("AddNecSpawn", 0.2f);

			break;
		case 3 :
			Sprite brushSprite = Resources.Load<Sprite> ("toothbrush");
			GameObject brush = Instantiate (neccPrefab);
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

	void OnPhotoCaught() {
		canvasDimmer.GetComponent<Animation> ().Play ();
	}

	void OnObjectDropped() {
		objectDropped = objectDropped + objectDropRate;

		if (angstBar.value > 0.8 * BAR_MAX) {
		} else {
			angstBar.value += BAR_ADD_DROP;
		}

		BAR_ADD_DROP -= BAR_ADD_DROP_GRADUAL;
		BAR_ADD_DROP = Mathf.Max (BAR_ADD_DROP, BAR_ADD_DROP_MIN);

	}

	void StartGame() {
		paperCaught = 0;
		objectDropped = 0;

		caughtText.text = "Caught: 0...";
	}

	void SpawnPhoto() {
		GameObject photo = Instantiate (photoPrefab);
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
		nextTimeToPaperSpawn -= 0.8f;
		nextTimeToPaperSpawn = Mathf.Max (0.50f, nextTimeToPaperSpawn);
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
			paperBackground.GetComponent<SpriteRenderer> ().sprite = null;
		} else if (ppbg != null) {
			paperBackground.GetComponent<SpriteRenderer> ().sprite = ppbg;
			if (spriteNum == 10) {
				if (!epilogued) {
					epilogued = true;
					angstBar.value = BAR_MAX;
					GameObject.FindGameObjectWithTag ("Progress").SendMessage ("startEpilogue");

					SetNextTimeToPaperSpawn (200);
				}
			}
		}
	}

	void IncreaseRoomFill() {
		objectDropRate = Mathf.Min (objectDropRate + 1, 5);
	}

	void DimLights() {
		dimLights = true;
	}

	void AddNecSpawn() {
		spawnNec++;
	}

	void GetPaperCaughtCount(GameObject gameObj) {
		gameObj.SendMessage ("SetItemsCaught", paperCaught);
	}
}
