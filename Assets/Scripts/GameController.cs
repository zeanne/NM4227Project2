
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject angstBar;
	public GameObject caughtCount;
	public GameObject menuPressSpace;
	public GameObject menuPressSpaceText;

	public GameObject instructions;
	public GameObject paperGenerator;

	public GameObject angstFill;
	public GameObject angstBg;
	public GameObject angstHandle;

	public GameObject player;
	public GameObject prologueText;
	public GameObject prologueCanvas;

	public GameObject epilogueCanvas;
	public GameObject epilogueText;
	public GameObject epilogueStats;

	private GameObject fadingText;

	bool fading;
	bool fadingIn;
	float startFadeTime;
	float fadeTime;

	bool first = true;
	bool gamePause = false;
	bool fadeAngstBar = false;
	bool shrink = false;
	bool dimScreen = false;
	bool dimScreenEpilogue = false;
	bool epilogueStarted = false;
	bool epilogueEnded = false;

	int itemsCollected;

	float timerStart;
	private int nextStr = 0;

	string[] monologue = new string[] {
		"Since young, my dream was always to be the world's greatest songwriter. But writing was never easy. With opportunities coming my way, the stress to write the next hit followed. I wrote the first piece...",
		"I wasn’t satisfied. I threw it away.",
		"Then came the second piece, third piece, fourth piece…. I wanted my songs to be perfect.",
		"The journey was lonely. At the end of each day, I would look at my dustbin, filled with pieces of my mind. The dustbin was my only companion.",
		"The stress in me gradually became a burden. I started feeling a sense of loss, a sense of ANGST.",
		"I wanted to be a perfectionist.",
		"When I see my room in chaos with my lost mind, the anger in me grew. It’s like… angst could never leave me. It has became a part of me.",
		"I wasn’t always this lonely. I had family, I had friends.",
		"But slowly, as I faded into my world of music, they left. One after another. Perhaps, it was me who threw them away with those imperfect music pieces. All of them.",
		"Gradually, I lost the ability to be normal. I eat for the sake of being alive. I take showers to keep me awake. But all of it, they mean nothing to me.",
		"Occasionally, I would go out to buy necessities. They were not toothbrush, soap or tissue paper. I would buy manuscript papers, which most landed back in the dustbin.",
		"But to me, leaving my table, my pen, my manuscripts was a waste of time. The anger in me grew.",
		"So I continued writing, and the imperfections started to build up.",
		"Until one day... I realized that I have to let go of these imperfections before I can move on.",
		"So, I swept away whatever I have built within me.",
		"But, an unknown sense of emptiness started to engulf me.... Like an uncontrollable fire burning me alive. Loss. Anger.",
		"So I wrote, and wrote and wrote..... \nAnd I threw, and threw, and threw....",
		"What? Even my only companion is dreading me? It looked like she had shrunk as my imperfections grew bigger. We grew apart.",
		"Gradually, I got lost... The lights began to dim. All my imperfections...",
		"I was drowning in them.",
		"",
		"After being buried in darkness, I finally saw the light. It dawned onto me that in life, perfection is not always achievable.\n\nThe melodies I wrote... They were brokenly perfect. ",
		"The angst in me... \nIt kept me going, it kept me alive, but it kept me from finding peace and happiness... \n\nTo the point where it exploded me. ",
		"When I finally played back the imperfections altogether, they sounded like a perfect piece. \n\nPerhaps, the hit song that I yearned for was there all along, but I was too unwilling to embrace my imperfect perfect piece.",
		"Imperfections kept me angst. \n\nPerfectionism kept me away from all that my life could have been."
	};

	// Use this for initialization
	void Start () {

		fadingText = prologueText;

		paperGenerator.SendMessage ("SetNextTimeToPaperSpawn", 60f);

		InvokeRepeating ("OneSec", 0, 1f);

		// for prologue
		Invoke ("StartNextMonologueSlow", 0f);
		Invoke ("StartNextMonologueFast", 15f);
		Invoke ("StartNextMonologueFast", 19f);

		Invoke ("showInstructions", 15f);

		Invoke ("EndNextMonologueSlow", 12f);
		Invoke ("EndNextMonologueFast", 18f);
		Invoke ("EndNextMonologueFast", 22f);

		Invoke ("EndPrologue", 24f);


		// game officially starts
		Invoke ("StartNextMonologueSlow", 26f);
		Invoke ("StartNextMonologueSlow", 40f); // angstbar
		Invoke ("StartNextMonologueFast", 46f);
		Invoke ("StartNextMonologueSlow", 50f);
		Invoke ("StartNextMonologueSlow", 60f); // photo
		Invoke ("StartNextMonologueSlow", 68f);
		Invoke ("StartNextMonologueSlow", 78f);
		Invoke ("StartNextMonologueSlow", 88f); // necessities
		Invoke ("StartNextMonologueFast", 101f); // clock
		Invoke ("StartNextMonologueFast", 115f); // faster
		Invoke ("StartNextMonologueFast", 121f); // broom
		Invoke ("StartNextMonologueSlow", 127f);
		Invoke ("StartNextMonologueSlow", 139f); // faster
		Invoke ("StartNextMonologueSlow", 145f);
		Invoke ("StartNextMonologueSlow", 170f); // shrink
		Invoke ("StartNextMonologueSlow", 185f); // dim
		Invoke ("StartNextMonologueFast", 203f);

		Invoke ("EndNextMonologueSlow", 35f);
		Invoke ("EndNextMonologueFast", 45f); // angstbar
		Invoke ("EndNextMonologueFast", 49f);
		Invoke ("EndNextMonologueSlow", 57f);
		Invoke ("EndNextMonologueSlow", 65f); // photo
		Invoke ("EndNextMonologueSlow", 75f);
		Invoke ("EndNextMonologueSlow", 85f);
		Invoke ("EndNextMonologueSlow", 98f); // necessities
		Invoke ("EndNextMonologueSlow", 110f); // clock
		Invoke ("EndNextMonologueFast", 119f); // faster
		Invoke ("EndNextMonologueFast", 125f); // broom
		Invoke ("EndNextMonologueSlow", 135f);
		Invoke ("EndNextMonologueFast", 143f); // faster
		Invoke ("EndNextMonologueSlow", 150f);
		Invoke ("EndNextMonologueSlow", 180f); // shrink
		Invoke ("EndNextMonologueSlow", 200f); // dim
		Invoke ("EndNextMonologueSlow", 217f);

		// angst bar appear
		Invoke ("showAngstBar", 41f);

		// photo appear
		InvokeRepeating ("throwPhoto", 62f, 5f);
		Invoke ("stopPhotoThrow", 100f);
//		Invoke ("throwPhoto", 62f);
//		Invoke ("throwPhoto", 64f);
//		Invoke ("throwPhoto", 66f);

		// necessities appear
		Invoke ("throwNecessities", 90f);
		Invoke ("throwNecessities", 94f);

		// clock appear, and every 20 seconds thereafter
		InvokeRepeating ("throwClock", 102f, 20f);

		// broom appear, and every 30 seconds thereafter until got lost
		InvokeRepeating ("throwBroom", 124f, 30f);
		Invoke ("cancelBroomThrow", 185f);

		// paper appear faster
		InvokeRepeating("throwPaperFaster", 115f, 20f);

		// shrink basket
		Invoke ("shrinkBasket", 155f);

		// start dimming screen
		Invoke ("startScreenDim", 190f);

		// fill the room with paper
		Invoke ("fillRoomFaster", 180f);
		Invoke ("fillRoomFaster", 190f);
		Invoke ("fillRoomFaster", 200f);


		// start epilogue after 5 mins if player still surviving
		Invoke ("startEpilogueIfAlive", 300f);


	}

	void Update() {

		if (epilogueEnded) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}

			return;
		}

		if (gamePause) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				GameStart ();
			}
		} else if (fading) {
			float timePassed = Time.realtimeSinceStartup - startFadeTime;
			if (fadingIn) {
				Color temp = fadingText.GetComponent<Text> ().color;
				temp.a = timePassed / fadeTime;

				if (temp.a >= 0.9) {
					temp.a = 1;
					fading = false;
				}

				fadingText.GetComponent<Text> ().color = temp;

			} else {
				Color temp;

				if (first) {
					temp = prologueCanvas.GetComponent<RawImage> ().color;
					temp.a = 1 - timePassed / fadeTime;
					prologueCanvas.GetComponent<RawImage> ().color = temp;
				}

				temp = fadingText.GetComponent<Text> ().color;
				temp.a = 1 - timePassed / fadeTime;

				if (temp.a <= 0.1) {
					temp.a = 0;
					fading = false;
					first = false;
					paperGenerator.SendMessage ("SetNextTimeToPaperSpawn", 2f);
				}

				fadingText.GetComponent<Text> ().color = temp;
			}
		}


		if (epilogueStarted) {
			if (epilogueCanvas.GetComponent<RawImage> ().color.a < 1) {

				if (prologueCanvas.GetComponent<RawImage> ().color.a < 1) {
					Color temp = prologueCanvas.GetComponent<RawImage> ().color;
					temp.a += 0.01f;
					prologueCanvas.GetComponent<RawImage> ().color = temp;

				} else {
					Color temp = epilogueCanvas.GetComponent<RawImage> ().color;
					temp.a += 0.005f;
					epilogueCanvas.GetComponent<RawImage> ().color = temp;
				}
			}

		} else {
			if (dimScreen && prologueCanvas.GetComponent<RawImage> ().color.a < 0.8) {
				Color temp = prologueCanvas.GetComponent<RawImage> ().color;
				temp.a += 0.0005f;
				prologueCanvas.GetComponent<RawImage> ().color = temp;
			}


			if (fadeAngstBar) {

				Color temp;

				temp = angstFill.GetComponent<Image> ().color;
				Debug.Log (temp);
				temp.a = Mathf.Min (temp.a + 0.01f, 1f);
				angstFill.GetComponent<Image> ().color = temp;

				temp = angstHandle.GetComponent<Image> ().color;
				temp.a = Mathf.Min (temp.a + 0.01f, 1f);
				angstHandle.GetComponent<Image> ().color = temp;

				temp = angstBg.GetComponent<Image> ().color;
				temp.a = Mathf.Min (temp.a + 0.01f, 1f);
				angstBg.GetComponent<Image> ().color = temp;

				if (temp.a == 1) {
					fadeAngstBar = false;
				}
			}

			if (shrink) {
				// initial scale = 0.5, final = 0.35
				Vector3 scale = player.transform.localScale;

				scale.x = Mathf.Max (scale.x - 0.0001f, 0.35f);
				scale.y = Mathf.Max (scale.y - 0.0001f, 0.35f);
				scale.z = Mathf.Max (scale.z - 0.0001f, 0.35f);

				player.transform.localScale = scale;

				if (scale.x <= 0.35) {
					shrink = false;
				}
			}
		}
	}

	void SetNextText() {
		if (nextStr >= monologue.Length) {
			return;
		}

		fadingText.GetComponent<Text>().text = monologue [nextStr];
		nextStr++;
	}

	void FadingIn(float fadingDuration) {
		fading = true;
		fadingIn = true;
		fadeTime = fadingDuration;
		startFadeTime = Time.realtimeSinceStartup;
		SetNextText ();
	}

	void FadingOut(float fadingDuration) {
		fading = true;
		fadingIn = false;
		fadeTime = fadingDuration;
		startFadeTime = Time.realtimeSinceStartup;
	}

	public void GameStart() {
		Time.timeScale = 1;
		menuPressSpace.SetActive (false);
		menuPressSpaceText.SetActive (false);
		gamePause = false;

		paperGenerator.SendMessage ("StartGame");
		caughtCount.SetActive (true);

	}


	void StartNextMonologueSlow() {
		Debug.Log ("next monologue start slow");
		FadingIn (3f);
	}

	void StartNextMonologueFast() {
		Debug.Log ("next monologue start fast");
		FadingIn (1f);
	}

	void EndNextMonologueSlow() {
		Debug.Log ("next monologue end slow");
		FadingOut (3f);
	}

	void EndNextMonologueFast() {
		Debug.Log ("next monologue end fast");
		FadingOut(1f);
	}

	void EndPrologue() {

		GameObject pp = GameObject.FindGameObjectWithTag ("Paper");
		if (pp != null) {
			Destroy (pp);
		}
		menuPressSpace.SetActive (true);
		menuPressSpaceText.SetActive (true);
		gamePause = true;
		Time.timeScale = 0;
	}

	void showInstructions() {
		instructions.SetActive (true);
	}

	void showAngstBar() {
		fadeAngstBar = true;
		angstBar.SetActive (true);
	}

	void throwPhoto() {
		paperGenerator.SendMessage ("SpawnPhoto");
	}

	void stopPhotoThrow() {
		CancelInvoke ("throwPhoto");
	}

	void throwNecessities() {
		paperGenerator.SendMessage ("SpawnNecessities");
	}

	void throwClock() {
		paperGenerator.SendMessage ("SpawnClock");
	}

	void throwBroom() {
		paperGenerator.SendMessage ("SpawnBroom");
	}

	void cancelBroomThrow() {
		CancelInvoke ("throwBroom");
	}

	void shrinkBasket() {
		shrink = true;
	}

	void throwPaperFaster() {
		paperGenerator.SendMessage ("IncreasePaperGeneration");
	}

	void cancelFasterPaperThrow() {
		CancelInvoke ("throwPaperFaster");
	}

	void fillRoomFaster() {
		paperGenerator.SendMessage ("IncreaseRoomFill");
	}

	void startScreenDim() {
		dimScreen = true;
	}

	void startEpilogue() {

		nextStr = 20;

		CancelInvoke ();
		paperGenerator.SendMessage ("GetPaperCaughtCount", gameObject);

		InvokeRepeating ("OneSec", 0, 1f);

		epilogueStarted = true;
		fadingText = epilogueText;

		// for epilogue
		Invoke ("StartNextMonologueSlow", 5f);
		Invoke ("EndNextMonologueFast", 7f);

		Invoke ("StartNextMonologueSlow", 8f);
		Invoke ("StartNextMonologueSlow", 18f);
		Invoke ("StartNextMonologueSlow", 25f);
		Invoke ("StartNextMonologueSlow", 38f);

		Invoke ("EndNextMonologueSlow", 16f);
		Invoke ("EndNextMonologueSlow", 22f);
		Invoke ("EndNextMonologueSlow", 35f);
		Invoke ("EndNextMonologueFast", 45f);
		Invoke ("EndNextMonologueFast", 43f);
		Invoke ("EndNextMonologueSlow", 52f);
		Invoke ("EndNextMonologueSlow", 62f);

		Invoke ("ShowFinalStats", 70f);

//		Invoke ("StartNextMonologueSlow", 8f);
//		Invoke ("StartNextMonologueSlow", 20f);
//		Invoke ("StartNextMonologueSlow", 30f);
//		Invoke ("StartNextMonologueSlow", 40f);
//
//		Invoke ("EndNextMonologueSlow", 16f);
//		Invoke ("EndNextMonologueSlow", 26f);
//		Invoke ("EndNextMonologueSlow", 36f);
//		Invoke ("EndNextMonologueFast", 46f);
//
//		Invoke ("ShowFinalStats", 50f);

	}

	void ShowFinalStats() {
		epilogueEnded = true;
//		epilogueStats.GetComponent<Text>().text = "Items Collected : " + itemsCollected;
		epilogueStats.SetActive (true);
	}

	void SetItemsCaught (int caught) {
		itemsCollected = caught;
	}

	void startEpilogueIfAlive() {
		if (!epilogueStarted) {
			startEpilogue ();
		}
	}

	void OneSec() {
		Debug.Log (Time.time);
	}
		
}

