using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

    [System.Serializable]
    public class Buttons {
        public GameObject gameObject;

        [HideInInspector]
        public Animation anim;
        [HideInInspector]
        public RectTransform rect;

        public Buttons(GameObject _gameObject) {
            gameObject = _gameObject;
            anim = _gameObject.GetComponent<Animation>();
            rect = _gameObject.GetComponent<RectTransform>();
        }
    }

    public camera iniCam;
    public GameObject[] scenes;
    public GameObject[] scenesMenu;
    public Buttons[] btnAnimation;
    public AudioSource audioBtn;

    static int btnSelect;
    bool btnDown;


    void Awake() {
        scenes[0].SetActive(true);
        scenes[1].SetActive(false);
        scenes[2].SetActive(false);
        scenesMenu[0].SetActive(true);
        scenesMenu[1].SetActive(false);
        scenesMenu[2].SetActive(false);
        scenesMenu[3].SetActive(false);
        btnDown = true;
        btnSelect = 0;
        for (int i = 0; i < btnAnimation.Length; i++){
            btnAnimation[i] = new Buttons(btnAnimation[i].gameObject);
        }
        RefreshBtn();
    }

    void Update() {
        if(Cursor.visible) {
            Cursor.visible = false;
        }

        if (Input.GetAxisRaw("Vertical") > 0 && btnDown) {
            btnDown = false;
            btnSelect--;
            if(btnSelect < 0) {
                btnSelect = 4;
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Vertical") < 0 && btnDown) {
            btnDown = false;
            btnSelect++;
            if(btnSelect > 4) {
                btnSelect = 0;
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && !btnDown) {
            btnDown = true;
        }


        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) {
            if(scenesMenu[1].activeSelf) {
                scenesMenu[0].SetActive(true);
                scenesMenu[1].SetActive(false);
                scenesMenu[2].SetActive(false);
                scenesMenu[3].SetActive(false);
                btnSelect = 3;
                RefreshBtn();
            }
            else if(scenesMenu[2].activeSelf) {
                scenesMenu[0].SetActive(true);
                scenesMenu[1].SetActive(false);
                scenesMenu[2].SetActive(false);
                scenesMenu[3].SetActive(false);
                btnSelect = 1;
                RefreshBtn();
            }
            else if(scenesMenu[3].activeSelf) {
                scenesMenu[0].SetActive(true);
                scenesMenu[1].SetActive(false);
                scenesMenu[2].SetActive(false);
                scenesMenu[3].SetActive(false);
                btnSelect = 2;
                RefreshBtn();
            }
            else if (btnSelect == 0) {
                scenes[0].SetActive(false);
                scenes[1].SetActive(true);
                scenes[2].SetActive(false);
                iniCam.enabled = true;
            }
            else if (btnSelect == 1) {
                scenesMenu[0].SetActive(false);
                scenesMenu[1].SetActive(false);
                scenesMenu[2].SetActive(true);
                scenesMenu[3].SetActive(false);
            }
            else if(btnSelect == 2) {
                scenesMenu[0].SetActive(false);
                scenesMenu[1].SetActive(false);
                scenesMenu[2].SetActive(false);
                scenesMenu[3].SetActive(true);
            }
            else if(btnSelect == 3) {
                scenesMenu[0].SetActive(false);
                scenesMenu[1].SetActive(true);
                scenesMenu[2].SetActive(false);
                scenesMenu[3].SetActive(false);
            }
            else if(btnSelect == 4) {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }

    void RefreshBtn() {
        audioBtn.Play();
        for (int i = 0; i < btnAnimation.Length; i++){
            if(i == btnSelect) {
                btnAnimation[btnSelect].anim.Play();
            }
            else {
                btnAnimation[i].anim.Stop();
                btnAnimation[i].rect.localScale = new Vector2(0.7f,0.7f);
            }
        }
    }
}
