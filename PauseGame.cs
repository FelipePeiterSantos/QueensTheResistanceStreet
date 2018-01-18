using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

    public GameObject[] sprites;

    public static bool pause;

    void Start() {
        sprites[0].SetActive(false);
        sprites[1].SetActive(false);
    }

	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape)) {
            pause = !pause;
            if(pause) {
                if(Time.timeScale != 0f) { 
                    Time.timeScale = 0f;
                }
                if(!sprites[0].activeSelf || !sprites[1].activeSelf) {
                    sprites[0].SetActive(true);
                    sprites[1].SetActive(true);
                }
            }
            else {
                if(Time.timeScale != 1f) {
                    Time.timeScale = 1f;
                }
                if(sprites[0].activeSelf || sprites[1].activeSelf) {
                    sprites[0].SetActive(false);
                    sprites[1].SetActive(false);
                }
            }
        }

        if(pause && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))) {
            pause = false;
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
	}
}
