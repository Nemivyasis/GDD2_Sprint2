/** Jonathan So, jds7523@rit.edu
 * Handles vertical remixing of audio pieces.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Jukebox : MonoBehaviour {

	public static Jukebox instance; // Singleton Object.

	public GameObject speaker; // The "speaker" object is just a blank prefab with an AudioSource.

	public AudioClip[] audioLayers; // Populate the list of audio layers with audio clips in the order that you want them to be introduced. E.G, the first clip goes first.
	public List<AudioSource> audioSrcs; 

	private const float EPSILON = 0.05f;
	private const int ONE_SEC = 60;

	public void AddNewSpeaker() {
		for (int i = 0; i < audioSrcs.Count; i++) {
			if (audioSrcs[i].volume == 0) {
				AddSpeaker(i);
				return;
			}
		}
	}

	/** Calls a function to select a speaker to lerp to an audible level. 
	 * Introduce a new channel of audio to the game.
	 * param[speakerIndex] - int; the index of the speaker we want to lerp.
	 */
	public void AddSpeaker(int speakerIndex) {
		StartCoroutine(VRLerp(speakerIndex));
	}

	// DEBUG USED FOR TESTING THE VERTICAL REMIX SYSTEM
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Debug.Log("Adding Speaker [1]");
			AddSpeaker(1);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Debug.Log("Adding Speaker [2]");
			AddSpeaker(2);
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Debug.Log("Adding Speaker [3]");
			AddSpeaker(3);
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Debug.Log("Adding Speaker [4]");
			AddSpeaker(4);
		}
	}

	/** Upon awake, make sure that for every audio layer in our music piece, we have
	 * one "speaker" for each.
	 * Also sets up the singleton object.
	 */
	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		// Create as many audio sources as children as there are audio layers.
		for (int i = 0; i < audioLayers.Length; i++) {
			GameObject result = (GameObject) Instantiate(speaker, transform.position, Quaternion.identity);
			result.GetComponent<AudioSource>().clip = audioLayers[i]; // Set its audio clip
			result.GetComponent<AudioSource>().volume = 0f; // Set its volume to zero.
			audioSrcs.Add(result.GetComponent<AudioSource>());
		}
		AddSpeaker(0);
	}

	// Start up the audio loop for the vertical remix.
	private void Start() {
		StartCoroutine("LoopSongVR");		
	}
		

	/** Wait in the frozen time, if we're ever modifying the time scale.
	* Probably stolen from some tutorial on the internet.
	* param[time] - float that expresses how long to wait.
	*/
	private IEnumerator WaitForRealSeconds(float time) {
		float init = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < init + time) {
			yield return null;
		}
	}

	/** Infinitely loops the layers of the audio.
	 * Tells all speaker objects to play their respective audio clip.
	 * Everything is based on the length of the first one, however.
	 */
	private IEnumerator LoopSongVR () {
		while (true) {
			// Play all layers of audio
			for (int i = 0; i < audioLayers.Length; i++) {
				audioSrcs[i].Play();
			}
			// Wait until the first speaker is done with their audio clip before looping.
			yield return StartCoroutine(WaitForRealSeconds(audioLayers[0].length));
			while (audioSrcs[0].isPlaying) {
				yield return new WaitForEndOfFrame();
			}
		}
	}

	/** Lerps a selected speaker to become audible over one second. 
	 * Gradually lerps a speaker's volume to become 1 / (the number of channels we have).
	 * param[speakerIndex] - int; the index of the speaker we want to lerp.
	 */
	private IEnumerator VRLerp(int speakerIndex) {
		float timer = 0;
		AudioSource currSpeaker = audioSrcs[speakerIndex];
		for (int i = 0; i < ONE_SEC; i++) {
			currSpeaker.volume = Mathf.Lerp(0, (1f / audioLayers.Length), timer);
			timer += Time.fixedDeltaTime;
			yield return new WaitForSeconds(Time.fixedDeltaTime);
		}
	}
}

