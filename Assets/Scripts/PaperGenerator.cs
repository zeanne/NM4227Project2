using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PaperGenerator : MonoBehaviour {

	public GameObject paperPrefab;
	public GameObject paperBackground;

	public Slider angstBar;
	public Text caughtText;
	public GameObject gameOverBox;

	private Sprite[] wastePaperArray;

	private float timeToSpawn;
	private int paperCaught = 0;
	private int paperDropped = 0;

	private float BAR_ADD_CATCH = 2;
	private float BAR_ADD_DROP = 8;
	private float BAR_ADD_DROP_GRADUAL = 1f;
	private float BAR_ADD_DROP_MIN = 4;
	private float BAR_GRADUAL = 0.02f;
	private float BAR_MAX = 100;
	private float BAR_MIN = 0;

	private int spriteNum = 0;


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

			Sprite nextColor = wastePaperArray[Random.Range(0, wastePaperArray.Length - 1)];
			paperPrefab.GetComponent<SpriteRenderer>().sprite = nextColor;

			GameObject paper = Instantiate (paperPrefab);
			paper.transform.SetParent (this.transform);
			timeToSpawn = Random.value * 3;

		}

		spriteNum = Mathf.Min(paperDropped / 5, 10);
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

		angstBar.value -= BAR_GRADUAL;
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

	void OnPaperDropped() {
		paperDropped++;


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
}
