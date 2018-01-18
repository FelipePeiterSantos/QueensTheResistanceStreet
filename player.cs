using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    public float speedRun = 2f;
    public GameObject txtComboObj;
    public UnityEngine.UI.Text txtCombo;
    public UnityEngine.UI.Text txtCombo1;
    public string[] words;
    public GameObject atkCollider;
    public GameObject shotCollider;
    public GameObject meteor;
    public GameObject unicorn;
    public GameObject revive;
    public AudioClip[] soundEffects;

    int moveHorizontal;
    int moveVertical;
    float runTime;
    bool isAtk;
    bool isAtk1;
    bool isUlt;
    bool isUlt1;
    float continueCombo;
    bool isTypeCombo;
    int comboKeys;
    bool knockback;
    bool gotHit;
    bool dead;
    bool invencible;
    int comboRevive;
    KeyCode[] keyComboRevive;
    KeyCode[] keyDownCombo;
    KeyCode[] keyDownCombo1;
    
    static SpriteRenderer body;
    static Rigidbody2D rig2D;
    static Animator anim;
    static AudioSource audioSource;

    void Awake(){
        txtComboObj.SetActive(false);
        txtCombo.text = words[0];
        txtCombo1.text = words[1];
        comboKeys = 0;
        moveHorizontal = 0;
        moveVertical = 0;
        runTime = 0;
        isAtk = false;
        continueCombo = 0f;
        isAtk1 = false;
        isTypeCombo = false;
        knockback = false;
        isUlt = false;
        isUlt1 = false;
        gotHit = false;
        dead = false;
        revive.SetActive(false);
        comboRevive = 0;
        keyComboRevive = new KeyCode[] { KeyCode.J, KeyCode.K};
        invencible = false;
        EnemiesManager.IgnorePlayer = false;
        rig2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body = transform.FindChild("body").GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

	void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            isAtk = true;
            continueCombo = 0.25f;
        }
        else if(Input.GetKeyDown(KeyCode.J)) {
            isAtk1 = true;
        }

        if(continueCombo > 0){
            continueCombo -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Return) && !dead && !PauseGame.pause && LifePlayerManager.ActiveUlti) {
            isTypeCombo = true;
            txtComboObj.SetActive(isTypeCombo);
            txtCombo.text = words[0];
            txtCombo1.text = words[1];
            keyDownCombo = ConvertStringToKeycode(txtCombo.text);
            keyDownCombo1 = ConvertStringToKeycode(txtCombo1.text);
            comboKeys = 0;
            isAtk = false;
            isAtk1 = false;
            isUlt = false;
            isUlt1 = false;
        }

        if(Input.GetAxisRaw("Horizontal") > 0) {
            if(moveHorizontal != 1) {
                moveHorizontal = 1;
                runTime = 0;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") < 0) {
            if(moveHorizontal != 2) {
                moveHorizontal = 2;
                runTime = 0;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") == 0){
            if(moveHorizontal != 0) {
                moveHorizontal = 0;
            }
        }

        if(Input.GetAxisRaw("Vertical") > 0) {
            moveVertical = 1;
        }
        else if(Input.GetAxisRaw("Vertical") < 0) {
            moveVertical = 2;
        }

        else if(Input.GetAxisRaw("Vertical") == 0){
            moveVertical = 0;
        }

        if(dead) {
            if(Input.GetKeyDown(keyComboRevive[comboRevive])) {
                LifePlayerManager.LifeModify(1);
                if(LifePlayerManager.Life >= 100) {
                    dead = false;
                    invencible = false;
                    anim.SetTrigger("revive");
                    PlaySoundEffect(3);
                    revive.SetActive(false);
                    anim.SetInteger("animation",0);
                    if(knockback || gotHit) {
                        knockback = false;
                        gotHit = false;
                    }
                }
                comboRevive++;
                if(comboRevive >= keyComboRevive.Length) {
                    comboRevive = 0;
                }
            }
        }
        else if(isTypeCombo) {
            if (!EnemiesManager.IgnorePlayer) {
                EnemiesManager.IgnorePlayer = true;
            }
            if(Input.GetKeyDown(keyDownCombo[comboKeys])) {
                ComboInputs(0,comboKeys);
                if (comboKeys < keyDownCombo.Length-1) {
                    comboKeys++;
                }
                else {
                    isTypeCombo = false;
                    LifePlayerManager.EnergyModify(-1);
                    txtComboObj.SetActive(false);
                    isUlt = true;
                }
            }
            else if(Input.GetKeyDown(keyDownCombo1[comboKeys<keyDownCombo1.Length ? comboKeys : 0]) && comboKeys < keyDownCombo1.Length) {
                ComboInputs(1,comboKeys);
                if (comboKeys < keyDownCombo1.Length-1) {
                    comboKeys++;
                }
                else {
                    isTypeCombo = false;
                    LifePlayerManager.EnergyModify(-1);
                    txtComboObj.SetActive(false);
                    isUlt1 = true;
                }
            }
            else if(Input.anyKeyDown) {
                comboKeys = 0;
                ComboInputs(0,-1);
            }
        }
    }

    void FixedUpdate(){
        if(dead) {
            invencible = true;
            rig2D.velocity = new Vector2(0,0);
            if(!EnemiesManager.IgnorePlayer) {
                EnemiesManager.IgnorePlayer = true;
            }
            if(!revive.activeSelf){
                revive.SetActive(true);
            }
        }
        else if(isTypeCombo) {
            rig2D.velocity = new Vector2(0,0);
            if (anim.GetInteger("animation") != 0 || anim.GetBool("endRun")) {
                anim.SetInteger("animation", 0);
                anim.SetBool("endRun",false);
            }
        }
        else if(isUlt) {
            invencible = true;
            rig2D.velocity = new Vector2(0,0);
            if (anim.GetInteger("animation") != 4) {
                anim.SetInteger("animation", 4);
                GameObject tempInst = Instantiate(unicorn, Vector3.zero, Quaternion.identity) as GameObject;
                tempInst.transform.localScale = new Vector3(body.flipX ? -1 : 1 ,1,1);
            }
            EnemiesManager.IgnorePlayer = true;
        }
        else if(isUlt1) {
            invencible = true;
            rig2D.velocity = new Vector2(0,0);
            if (anim.GetInteger("animation") != 5) {
                anim.SetInteger("animation", 5);
                GameObject tempInst = Instantiate(meteor, new Vector3(body.flipX ? transform.position.x+1.53f : transform.position.x-1.53f, -0.581f, -0.581f), Quaternion.identity) as GameObject;
                tempInst.transform.localScale = new Vector3(body.flipX ? -1 : 1 ,1,1);
            }
            EnemiesManager.IgnorePlayer = true;
        }
        else if (knockback || gotHit) {
            if(isAtk || isAtk1 || isUlt || isUlt1 || rig2D.velocity.y != 0 || atkCollider.activeSelf || shotCollider.activeSelf) {
                isAtk = false;
                isAtk1 = false;
                isUlt = false;
                isUlt1 = false;
                rig2D.velocity = new Vector2(knockback ? rig2D.velocity.x : 0,0);
                atkCollider.SetActive(false);
                shotCollider.SetActive(false);
            }
            if(gotHit) {
                rig2D.velocity = new Vector2(0,0);
            }            
            else if(rig2D.velocity.x > 0.05f) {
                rig2D.velocity += new Vector2(-0.05f,0);
            }
            else if(rig2D.velocity.x < -0.05f) {
                rig2D.velocity += new Vector2(+0.05f,0);
            }
            else {
                rig2D.velocity = new Vector2(0,0);
            }
        }
        else if(isAtk){
            rig2D.velocity = new Vector2(0,0);
            if (anim.GetInteger("animation") != 2) {
                anim.SetInteger("animation", 2);
            }
        }
        else if(isAtk1){
            rig2D.velocity = new Vector2(0,0);
            if (anim.GetInteger("animation") != 3) {
                anim.SetInteger("animation", 3);
            }
        }
        
        else {

            if(!isAtk1) {
                if(atkCollider.activeSelf) {
                    atkCollider.SetActive(false);
                }
            }
            else if(!isAtk) {
                if(shotCollider.activeSelf) {
                    shotCollider.SetActive(false);
                }
            }

            if(EnemiesManager.IgnorePlayer) {
                EnemiesManager.IgnorePlayer = false;
            }
            if(moveHorizontal == 0) {
                if(rig2D.velocity.x > 0.05f && rig2D.velocity.y == 0 && runTime > 0.5f) {
                    rig2D.velocity += new Vector2(-0.05f,0);
                    if(anim.GetBool("endRun") != true) {
                        anim.SetBool("endRun", true);
                    }
                }
                else if(rig2D.velocity.x < -0.05f && rig2D.velocity.y == 0 && runTime > 0.5f) {
                    rig2D.velocity += new Vector2(+0.05f,0);
                    if(anim.GetBool("endRun") != true) {
                        anim.SetBool("endRun", true);
                    }
                }
                else {
                    runTime = 0;
                    rig2D.velocity = new Vector2(0,rig2D.velocity.y);
                    if(anim.GetBool("endRun") != false) {
                        anim.SetBool("endRun", false);
                    }
                }
            }
            else {
                runTime += Time.deltaTime;
                rig2D.velocity = new Vector2((moveHorizontal == 1) ? speedRun : -speedRun,rig2D.velocity.y);
                if(body.flipX != (moveHorizontal == 1) ? false : true) {
                    body.flipX = (moveHorizontal == 1) ? false : true;
                    atkCollider.transform.localPosition = new Vector2((moveHorizontal == 1) ? 0.2f : -0.2f,0);
                    shotCollider.transform.localPosition = new Vector2((moveHorizontal == 1) ? 0.9f : -0.9f,0);
                }
                if(anim.GetInteger("animation") != 1) {
                    anim.SetInteger("animation", 1);
                }
            }

            if(moveVertical == 0) {
                rig2D.velocity = new Vector2(rig2D.velocity.x,0);
            }
            else if(moveVertical == 1) {
                rig2D.velocity = new Vector2(rig2D.velocity.x,speedRun/1.5f);
                if(anim.GetInteger("animation") != 1) {
                    anim.SetInteger("animation", 1);
                }
            }
            else if(moveVertical == 2) {
                rig2D.velocity = new Vector2(rig2D.velocity.x,-speedRun/1.5f);
                if(anim.GetInteger("animation") != 1) {
                    anim.SetInteger("animation", 1);
                }
            }

            if(moveVertical == 0 && moveHorizontal == 0) {
                if (anim.GetInteger("animation") != 0) {
                    anim.SetInteger("animation", 0);
                }                
            }

            transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.y);
        }
    }
    

    private KeyCode[] ConvertStringToKeycode(string convertWord) {
        KeyCode[] converted = new KeyCode[convertWord.Length];
        for (int i = 0; i < converted.Length; i++) {
            converted[i] = (KeyCode)System.Enum.Parse(typeof (KeyCode),convertWord.ToCharArray(i, 1)[0].ToString());
        }
        return converted;
    }

    private void ComboInputs(int text ,int aux) {
        if (aux >= 0) {
            if(text == 0) {
                txtCombo.text = "<color=green>"+words[0].Substring(0,aux+1)+"</color>"+words[0].Substring(aux+1,words[0].Length-aux-1);
            }
            else {
                txtCombo1.text = "<color=green>"+words[1].Substring(0,aux+1)+"</color>"+words[1].Substring(aux+1,words[1].Length-aux-1);
            }
        }
        else {
            txtCombo.text = words[0];
            txtCombo1.text = words[1];
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(!invencible && !EnemiesManager.IgnorePlayer) {
            if(coll.name == "e_atkCollider") {
                gotHit = true;
                invencible = true;
                anim.SetInteger("animation",1);
                StartCoroutine("HitFeed");
                anim.SetTrigger("hit");
                LifePlayerManager.LifeModify(-5);
                LifePlayerManager.EnergyModify(10);
                if(LifePlayerManager.Life <= 0) {
                    PlaySoundEffect(2);
                    dead = true;
                    anim.SetTrigger("dead");
                }
            }
            if(coll.name == "t_atkCollider") {
                body.flipX = coll.transform.position.x < transform.position.x? true: false;
                StartCoroutine("HitFeed");
                Knockback(0);
                invencible = true;
                LifePlayerManager.LifeModify(-15);
                LifePlayerManager.EnergyModify(10);
                if(LifePlayerManager.Life <= 0) {
                    PlaySoundEffect(2);
                    dead = true;
                    anim.SetTrigger("dead");
                }
            }
        }
    }

    IEnumerator HitFeed() {
        PlaySoundEffect(1);
        body.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        body.color = Color.white;
    }

    public void EndAttack() {
        if(continueCombo <= 0) {
            isAtk = false;
        }
        isAtk1 = false;
        isUlt = false;
        isUlt1 = false;
        gotHit = false;
        invencible = false;
    }

    public void ContinueCombo() {
        if (body.flipX != false && moveHorizontal == 1) {
            body.flipX = false;
            atkCollider.transform.localPosition = new Vector2(0.2f,0);
        }
        else if(body.flipX != true && moveHorizontal == 2) {
            body.flipX = true;
            atkCollider.transform.localPosition = new Vector2(-0.2f,0);
        }
        if(continueCombo <= 0) {
            EndAttack();
        }
    }

    public void ActiveCollider(int active) {
        if(isAtk){
            atkCollider.SetActive(active == 1 ? true : false);
            if(active == 1) {
                PlaySoundEffect(0);
            }
        }
        else if(isAtk1) {
            shotCollider.SetActive(active == 1 ? true : false);
            if(active == 1) {
                PlaySoundEffect(5);
            }
        }
        else {
            atkCollider.SetActive(false);
            shotCollider.SetActive(false);
        }
    }

    public void Knockback(int aux) {
        if(aux == 0) {
            PlaySoundEffect(2);
            knockback = true;
            invencible = true;
            rig2D.velocity = new Vector2((body.flipX)? speedRun:-speedRun,rig2D.velocity.y);
            anim.SetTrigger("fall");

        }
        else if(aux == 1) {
            knockback = false;
            invencible = false;
        }
    }

    public void PlaySoundEffect(int aux) {
        if(audioSource.clip != soundEffects[3] || !audioSource.isPlaying) {
            audioSource.clip = soundEffects[aux];
            audioSource.pitch = Random.Range(0.9f,1.1f);
            audioSource.Play();
        }
    }
}
