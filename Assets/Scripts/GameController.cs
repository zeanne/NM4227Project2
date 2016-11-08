
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

	public GameObject musicObject;
	public AudioClip betterdays;
	public AudioClip tomorrow;

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
	bool epilogueStarted = false;
	bool epilogueEnded = false;

	int itemsCollected;

	float timerStart;
	private int nextStr = 0;

	string[] monologue = new string[] {
		"Since young, my dream was always to be the world's greatest songwriter. But writing was never easy. With opportunities coming my way, the stress to write the next hit followed.\n\nI wrote the first piece...",
		"I wasn’t satisfied. I threw it away.",
		"Then came the second piece, third piece, fourth piece…. I wanted my songs to be perfect.",
		"The journey was lonely.\nAt the end of each day, I would look at my dustbin, filled with pieces of my mind.\nThe dustbin was my only companion.",
		"The stress in me gradually became a burden.\nI started feeling a sense of loss,\na sense of ANGST.",
		"I wanted to be a perfectionist.",
		"When I see my room in chaos with my lost mind, the anger in me grew. It’s like… angst could never leave me. It has became a part of me.",
		"I wasn’t always this lonely.\nI had family, I had friends.",
		"But slowly, as I faded into my world of music, they left. One after another. Perhaps, it was me who threw them away with those imperfect music pieces. All of them.",
		"Gradually, I lost the ability to be normal.\nI eat for the sake of being alive.\nI take showers to keep me awake.\nBut all of it, they mean nothing to me.",
		"Occasionally, I would go out to buy necessities. They were not toothbrush, soap or tissue paper.\nI would buy manuscript papers, which most landed back in the dustbin.",
		"But to me, leaving my table, my pen, my manuscripts was a waste of time.\nThe anger in me grew.",
		"So I continued writing, and the imperfections started to build up.",
		"Until one day... I realized that I have to let go of these imperfections before I can move on.",
		"So, I swept away whatever I have built within me.",
		"But, an unknown sense of emptiness started to engulf me.... Like an uncontrollable fire burning me alive. Loss. Anger.",
		"So I wrote, and wrote and wrote..... \nAnd I threw, and threw, and threw....",
		"What? Even my only companion is dreading me?\nIt looked like she had shrunk as my imperfections grew bigger. We grew apart.",
		"Gradually, I got lost...\nThe lights began to dim.\nAll my imperfections...",
		"I was drowning in them.",
		"",
		"After being buried in darkness, I finally saw the light. It dawned onto me that in life, perfection is not always achievable.\nThe melodies I wrote... They were brokenly perfect. ",
		"The angst in me... \nIt kept me going, it kept me alive,\nbut it kept me from finding peace and happiness... \nTo the point where it exploded me. ",
		"When I finally played back the imperfections altogether, they sounded like a perfect piece. \nPerhaps, the hit song that I yearned for was there all along, but I was too unwilling to embrace my imperfect perfect piece.",
		"Imperfections kept me angst. \n\nPerfectionism kept me away from all that\nmy life could have been."
	};

	// Use this for initialization
	void Start () {

		if (musicObject.GetComponent<AudioSource> ().isPlaying) {
			musicObject.GetComponent<AudioSource> ().Stop ();
		}

		fadingText = prologueText;

		paperGenerator.SendMessage ("SetNextTimeToPaperSpawn", 60f);

		// for prologue
		Invoke ("StartNextMonologueSlow", 0f);
		Invoke ("StartNextMonologueFast", 15f);
		Invoke ("StartNextMonologueFast", 19f);

		Invoke ("showInstructions", 15f);

		Invoke ("EndNextMonologueSlow", 12f);
		Invoke ("EndNextMonologueFast", 18f);
		Invoke ("EndNextMonologueFast", 22f);

		Invoke ("EndPrologue", 24f);
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
				temp.a += 0.001f;
				prologueCanvas.GetComponent<RawImage> ().color = temp;
			}

			if (fadeAngstBar) {

				Color temp;

				temp = angstFill.GetComponent<Image> ().color;
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
		//Time.timeScale = 1; /**/
		menuPressSpace.SetActive (false);
		menuPressSpaceText.SetActive (false);
		gamePause = false;

		paperGenerator.SendMessage ("StartGame");
		caughtCount.SetActive (true);

		startGame (); /**/
	}


	void StartNextMonologueSlow() {
		Debug.Log ("next monologue start slow time = " + Time.time);
		FadingIn (3f);
	}

	void StartNextMonologueFast() {
		Debug.Log ("next monologue start fast time = " + Time.time);
		FadingIn (1f);
	}

	void EndNextMonologueSlow() {
		Debug.Log ("next monologue end slow time = " + Time.time);
		FadingOut (3f);
	}

	void EndNextMonologueFast() {
		Debug.Log ("next monologue end fast time = " + Time.time);
		FadingOut(1f);
	}

	void EndPrologue() {

		menuPressSpace.SetActive (true);
		menuPressSpaceText.SetActive (true);
		gamePause = true;
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

	void startGame() {

		musicObject.GetComponent<AudioSource> ().clip = tomorrow;
		musicObject.GetComponent<AudioSource> ().Play ();

		// game officially starts
		Invoke ("StartNextMonologueSlow", 1f);
		Invoke ("StartNextMonologueSlow", 14f); // angstbar
		Invoke ("StartNextMonologueFast", 22f);
		Invoke ("StartNextMonologueSlow", 26f);
		Invoke ("StartNextMonologueSlow", 35f); // photo
		Invoke ("StartNextMonologueSlow", 43f);
		Invoke ("StartNextMonologueSlow", 53f);
		Invoke ("StartNextMonologueSlow", 63f); // necessities
		Invoke ("StartNextMonologueFast", 76f); // clock
		Invoke ("StartNextMonologueFast", 90f); // faster
		Invoke ("StartNextMonologueFast", 96f); 
		Invoke ("StartNextMonologueSlow", 102f); // broom
		Invoke ("StartNextMonologueSlow", 114f); // faster
		Invoke ("StartNextMonologueSlow", 120f);
		Invoke ("StartNextMonologueSlow", 145f); // shrink
		Invoke ("StartNextMonologueSlow", 160f); // dim
		Invoke ("StartNextMonologueFast", 178f);

		Invoke ("EndNextMonologueSlow", 10f);
		Invoke ("EndNextMonologueFast", 20f); // angstbar
		Invoke ("EndNextMonologueFast", 24f);
		Invoke ("EndNextMonologueSlow", 32f);
		Invoke ("EndNextMonologueSlow", 40f); // photo
		Invoke ("EndNextMonologueSlow", 50f);
		Invoke ("EndNextMonologueSlow", 60f);
		Invoke ("EndNextMonologueSlow", 73f); // necessities
		Invoke ("EndNextMonologueSlow", 85f); // clock
		Invoke ("EndNextMonologueFast", 94f); // faster
		Invoke ("EndNextMonologueFast", 100f); // broom
		Invoke ("EndNextMonologueSlow", 110f);
		Invoke ("EndNextMonologueFast", 118f); // faster
		Invoke ("EndNextMonologueSlow", 125f);
		Invoke ("EndNextMonologueSlow", 155f); // shrink
		Invoke ("EndNextMonologueSlow", 175f); // dim
		Invoke ("EndNextMonologueSlow", 192f);

		// angst bar appear
		Invoke ("showAngstBar", 16f);

		// photo appear
		InvokeRepeating ("throwPhoto", 37f, 10f);
		Invoke ("stopPhotoThrow", 75f);
//		Invoke ("throwPhoto", 62f);
//		Invoke ("throwPhoto", 64f);
//		Invoke ("throwPhoto", 66f);

		// necessities appear
		Invoke ("throwNecessities", 65f);
		Invoke ("throwNecessities", 69f);

		// clock appear, and every 20 seconds thereafter
		InvokeRepeating ("throwClock", 77f, 20f);

		// broom appear, and every 30 seconds thereafter until got lost
		InvokeRepeating ("throwBroom", 105f, 30f);
		Invoke ("cancelBroomThrow", 200f);

		// paper appear faster
		InvokeRepeating("throwPaperFaster", 90f, 20f);

		// shrink basket
		Invoke ("shrinkBasket", 130f);

		// start dimming screen
		Invoke ("startScreenDim", 165f);

		// fill the room with paper
		Invoke ("fillRoomFaster", 155f);
		Invoke ("fillRoomFaster", 165f);
		Invoke ("fillRoomFaster", 175f);

		Invoke ("startEpilogueIfAlive", 300f);
	}

	void startEpilogue() {

		if (musicObject.GetComponent<AudioSource> ().isPlaying) {
			musicObject.GetComponent<AudioSource> ().Stop ();
		}

		musicObject.GetComponent<AudioSource> ().clip = betterdays;
		musicObject.GetComponent<AudioSource> ().Play ();

		nextStr = 20;

		CancelInvoke ();
		paperGenerator.SendMessage ("GetPaperCaughtCount", gameObject);
		paperGenerator.SendMessage ("StopPaperSpawn");

		epilogueStarted = true;
		fadingText = epilogueText;

		// for epilogue
		Invoke ("StartNextMonologueSlow", 5f);
		Invoke ("EndNextMonologueFast", 7f);

		Invoke ("StartNextMonologueSlow", 8f);
		Invoke ("StartNextMonologueSlow", 21f);
		Invoke ("StartNextMonologueSlow", 34f);
		Invoke ("StartNextMonologueSlow", 47f);

		Invoke ("EndNextMonologueSlow", 17f);
		Invoke ("EndNextMonologueSlow", 30f);
		Invoke ("EndNextMonologueSlow", 43f);
		Invoke ("EndNextMonologueFast", 65f);

		Invoke ("ShowFinalStats", 55f);

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
}

