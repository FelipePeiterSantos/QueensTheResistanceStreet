using UnityEngine;
using System.Collections;

public class FinalScore : MonoBehaviour {

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
    public Buttons[] btnAnimation;
    public UnityEngine.UI.Text insertName,yourName,scoreTxt;
    public AudioSource audioBtn;

    static KeyCode[] letter;
    const string privateCode = "Eag5oaiDNECKMKEUc3gxlQ2jIGqOplfkm722CqZ05jnA";
    const string webUrl = "http://dreamlo.com/lb/";


    int btnSelect;
    bool btnDown;
    bool typingName;

	void Start() {
        if(letter == null) {
            letter = ConvertStringToKeycode("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }
        scoreTxt.text = "0000";
        yourName.text = "";
        typingName = false;
        btnSelect = 2;
        for (int i = 0; i < btnAnimation.Length; i++){
            btnAnimation[i] = new Buttons(btnAnimation[i].gameObject);
        }
        RefreshBtn();
    }

    void Update() {
        if(scoreTxt.text != Score.scorePoints.ToString("0000")) {
            scoreTxt.text = Score.scorePoints.ToString("0000");
        }

        if(typingName) {
            if(Input.anyKeyDown) {
                if(Input.GetKeyDown(KeyCode.Backspace) && yourName.text != "") {
                    yourName.text = yourName.text.Substring(0,yourName.text.Length-1);
                }
                else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    if(yourName.text == "") {
                        insertName.text = "Insert your name";
                        btnSelect = 2;
                    }
                    else {
                        btnSelect = 3;
                    }
                    typingName = false;
                    RefreshBtn();
                }
                else if(Input.inputString != "") {
                    yourName.text += Input.inputString;
                }
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && btnDown) {
            btnDown = false;
            if(btnSelect == 0 || btnSelect == 1) {
                if(yourName.text == "") {
                    btnSelect = 2;
                }
                else {
                    btnSelect = 3;
                }
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Vertical") < 0 && btnDown) {
            btnDown = false;
            if(btnSelect == 2 || btnSelect == 3) {
                btnSelect = 0;
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Horizontal") < 0 && btnDown) {
            btnDown = false;
            if(btnSelect == 1) {
                btnSelect = 0;
            }
            else if(btnSelect == 2 || btnSelect == 3) {
                btnSelect = 0;
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Horizontal") > 0 && btnDown) {
            btnDown = false;
            if(btnSelect == 0) {
                btnSelect = 1;
            }
            else if(btnSelect == 2 || btnSelect == 3) {
                btnSelect = 1;
            }
            RefreshBtn();
        }
        else if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && !btnDown) {
            btnDown = true;
        }
        else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)) {
            if(btnSelect == 0) {
                StartCoroutine("SendScore");
            }
            else if(btnSelect == 1) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            else if(btnSelect == 2 && !typingName) {
                typingName = true;
                insertName.text = "";
                RefreshBtn();
            }
            else if(btnSelect == 3) {
                typingName = true;
                RefreshBtn();
            }
        }
    }

    void RefreshBtn() {
        if(typingName) {
            for (int i = 0; i < btnAnimation.Length; i++){
                btnAnimation[i].anim.Stop();
                btnAnimation[i].rect.localScale = new Vector2(0.7f,0.7f);
            }
        }
        else {
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

    private KeyCode[] ConvertStringToKeycode(string convertWord) {
        KeyCode[] converted = new KeyCode[convertWord.Length];
        for (int i = 0; i < converted.Length; i++) {
            converted[i] = (KeyCode)System.Enum.Parse(typeof (KeyCode),convertWord.ToCharArray(i, 1)[0].ToString());
        }
        return converted;
    }

    IEnumerator SendScore() {
        string haveName = "NoName";
        if(yourName.text != "") {
            haveName = yourName.text;
        }
        WWW www = new WWW(webUrl+privateCode+"/add/"+WWW.EscapeURL(haveName)+"/"+scoreTxt.text);
        yield return www;
        if(string.IsNullOrEmpty(www.error)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else {
            print("Try Again: "+ www.error);
        }
    }
}
