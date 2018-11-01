using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private AudioSource audi; // For playing the menu selection sound effect.

	// Get an instance of the audio source.
	private void Awake() {
		audi = GetComponent<AudioSource>();
		Screen.SetResolution(1600, 700, false, 60); // Set resolution to 1600x700.
	}

    //for calling scenes from buttons
    public void LoadLevel(int sceneIndex)
    {
		StartCoroutine(WaitAndLoadLevel(sceneIndex));
    }

	/** Plays the "Menu Selection" soundclip, waits until it's finished, then load the level.
	 * param[sceneIndex] - the index of the scene in BuildSettings that we want to load.
	 */
	private IEnumerator WaitAndLoadLevel(int sceneIndex) {
		if (audi != null && !audi.isPlaying) {
			audi.Play();
		}
		yield return new WaitForSeconds(audi.clip.length);
		SceneManager.LoadScene(sceneIndex);
	}
}
