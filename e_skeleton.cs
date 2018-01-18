using UnityEngine;
using System.Collections;

public class e_skeleton : MonoBehaviour {

    public AnimationCurve detectionCurve;
    public GameObject atkCollider;
    public GameObject pointFeed;

    static Transform player;
    static Transform statueGoal;

    SpriteRenderer body;
    Rigidbody2D rig;
    Animator anim;
    CircleCollider2D colliderDamage;

    float detection;
    bool playerDetected;
    int isDoing;

    int life;

    void Start() {
        if(EnemiesManager.EnemiesAttacking == null) {
            EnemiesManager.EnemiesAttacking = new System.Collections.Generic.List<int>();
        }
        if(!spawner.spawnCount.Contains(GetInstanceID())) {
            spawner.spawnCount.Add(GetInstanceID());
        }

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        body = GetComponentInChildren<SpriteRenderer>();
        colliderDamage = GetComponentInChildren<CircleCollider2D>();

        if(statueGoal == null) {
            statueGoal = GameObject.Find("crown").transform;
        }

        if (player == null){
            player = GameObject.Find("Player").transform;
        }

        if (player.position.x < transform.position.x) {
            body.flipX = true;
        }
        else {
            body.flipX = false;
        }
        isDoing = 0;
        atkCollider.SetActive(false);
        life = 3;
        InvokeRepeating("DetectPlayerTick",.25f,.25f);
    }
    
    void DetectPlayerTick() {
        if(EnemiesManager.EnemiesAttacking.Count == 2 && !EnemiesManager.EnemiesAttacking.Contains(GetInstanceID())) {
            playerDetected = false;
            EnemiesManager.EnemiesAttacking.Remove(GetInstanceID());
        }
        if(isDoing == 0 && EnemiesManager.EnemiesAttacking.Count < 2) {
            detection = Mathf.Abs(transform.position.x - player.position.x);
            if (Mathf.Abs(Vector2.Distance(transform.position,player.position))<0.2f) {
                AbleToAttack();
            }
            else if (detection < 1.8f){
                if(body.flipX && transform.position.x > player.position.x) {
                    if((transform.position.y - detectionCurve.Evaluate(detection/1.5f)/2.5f) < player.position.y && (transform.position.y + detectionCurve.Evaluate(detection/1.5f)/2.5f) > player.position.y) {
                        AbleToAttack();
                    }
                }
                else if(!body.flipX && transform.position.x < player.position.x)  {
                    if((transform.position.y - detectionCurve.Evaluate(detection/1.5f)/2.5f) < player.position.y && (transform.position.y + detectionCurve.Evaluate(detection/1.5f)/2.5f) > player.position.y) {
                        AbleToAttack();
                    }
                }
            }
        }

        if(!body.isVisible && playerDetected && EnemiesManager.EnemiesAttacking.Contains(GetInstanceID())) {
            playerDetected = false;
            EnemiesManager.EnemiesAttacking.Remove(GetInstanceID());
        }
    }

    void FixedUpdate() {
        if(EnemiesManager.IgnorePlayer && playerDetected) {
            playerDetected = false;
            isDoing = 0;
            EnemiesManager.ResetListAttacking();
        }
        if(isDoing == 0) {
            if(playerDetected) {
                if(!body.flipX && transform.position.x-0.2f > player.position.x) {
                    body.flipX = true;
                }
                else if(body.flipX && transform.position.x+0.2f  < player.position.x) {
                    body.flipX = false;
                }

                if(transform.position.y-0.025f > player.position.y) {
                    rig.velocity = new Vector2(0,-0.5f);
                }
                else if(transform.position.y+0.025f < player.position.y) {
                    rig.velocity = new Vector2(0,0.5f);
                }
                else {
                    rig.velocity = new Vector2(0,0);
                }

                rig.velocity = new Vector2(body.flipX ? -0.5f : 0.5f,rig.velocity.y);

                if (Mathf.Abs(transform.position.x - player.position.x) <= 0.4f && Mathf.Abs(transform.position.y - player.position.y) <= 0.1f && !EnemiesManager.IgnorePlayer){
                    isDoing = 1;
                    anim.SetInteger("animation",1);
                    body.flipX = (transform.position.x > player.position.x) ? true: false;
                    atkCollider.transform.localPosition = new Vector2(body.flipX ? -0.2f : 0.2f,0);
                }
            }
            else {
                if(!body.flipX && transform.position.x-0.5f > statueGoal.position.x) {
                    body.flipX = true;
                }
                else if(body.flipX && transform.position.x+0.5f  < statueGoal.position.x) {
                    body.flipX = false;
                }

                if (Mathf.Abs(transform.position.x - statueGoal.position.x) < 1.5f) {
                    if(transform.position.y > statueGoal.position.y) {
                        rig.velocity = new Vector2(0,-0.2f);
                    }
                    else if(transform.position.y+0.050f < statueGoal.position.y) {
                        rig.velocity = new Vector2(0,0.2f);
                    }
                    else {
                        rig.velocity = new Vector2(0,0);
                    }
                }
                else {
                    rig.velocity = new Vector2(body.flipX ? -0.5f : 0.5f,0);
                }

                rig.velocity = new Vector2(body.flipX ? -0.5f : 0.5f,rig.velocity.y);

                if (Mathf.Abs(transform.position.x - statueGoal.position.x) <= 0.4f && Mathf.Abs(transform.position.y - statueGoal.position.y) <= 0.1f){
                    isDoing = 1;
                    anim.SetInteger("animation",1);
                    body.flipX = (transform.position.x > statueGoal.position.x) ? true: false;
                    atkCollider.transform.localPosition = new Vector2(body.flipX ? -0.2f : 0.2f,0);
                }
            }
        }
        else if(isDoing == 1) {
            rig.velocity = new Vector2(0,0);
        }
        else if(isDoing == 2) {
            rig.velocity = new Vector2(0,0);
        }

        if (life <= 0) {
            rig.velocity = new Vector2(0,0);
            if(isDoing != 3) {
                isDoing = 3;
            }
            if(anim.GetInteger("animation") != 2) {
                anim.SetInteger("animation",2);
            }
            if(colliderDamage.enabled) {
                colliderDamage.enabled = false;
            }
            
        }

        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.name == "bootcollider" || coll.name == "uniCollider") {
            isDoing = 2;
            StartCoroutine("HitFeed");
            life = 0;
            StartCoroutine("Destroy");
            atkCollider.SetActive(false);
        }
        else if (coll.name == "atkCollider") {
            isDoing = 2;
            StartCoroutine("HitFeed");
            anim.SetTrigger("hit");
            life--;
            if(life <= 0) {
                LifePlayerManager.EnergyModify(5);
                StartCoroutine("Destroy");
            }
            else if (!EnemiesManager.EnemiesAttacking.Contains(GetInstanceID()) && EnemiesManager.EnemiesAttacking.Count > 0){
                EnemiesManager.EnemiesAttacking.RemoveAt(0);
                AbleToAttack();
            }
            atkCollider.SetActive(false);
            LifePlayerManager.EnergyModify(1);
        }
        else if(coll.name == "shotCollider" && body.isVisible) {
            isDoing = 2;
            StartCoroutine("HitFeed");
            anim.SetTrigger("hit");
            life--;
            if(life <= 0) {
                LifePlayerManager.EnergyModify(5);
                StartCoroutine("Destroy");
            }
            atkCollider.SetActive(false);
            LifePlayerManager.EnergyModify(1);
        }
    }

    void AttackRange() {
        body.flipX = (transform.position.x > player.position.x) ? true: false;
        if(Mathf.Abs(transform.position.x - player.position.x) > 0.4f || Mathf.Abs(transform.position.y - player.position.y) > 0.1f || isDoing == 2 || EnemiesManager.IgnorePlayer) {
            isDoing = 0;
            anim.SetInteger("animation",0);
            DetectPlayerTick();
        }
    }

    IEnumerator HitFeed() {
        SoundEffectEnemies.Play(0);
        body.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        body.color = Color.white;
    }

    IEnumerator Destroy() {
        if(EnemiesManager.EnemiesAttacking.Contains(GetInstanceID())) {
            EnemiesManager.EnemiesAttacking.Remove(GetInstanceID());
        }
        if(spawner.spawnCount.Contains(GetInstanceID())) {
            spawner.spawnCount.Remove(GetInstanceID());
        }
        Score.AddPoint(5);
        SoundEffectEnemies.Play(2);
        Instantiate(pointFeed,new Vector3(transform.position.x,transform.position.y + 0.7f,transform.position.y),Quaternion.identity);
        yield return new WaitForSeconds(2f);
        body.color = Color.clear;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.clear;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.clear;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        body.color = Color.clear;
        Destroy(this.gameObject);
    }

    void AbleToAttack() {
        if(!EnemiesManager.EnemiesAttacking.Contains(GetInstanceID())){
            playerDetected = true;
            EnemiesManager.EnemiesAttacking.Add(GetInstanceID());
        }
    }

    public void ActiveCollider(int active) {
        if(isDoing == 1){
            atkCollider.SetActive(active == 1 ? true : false);
        }
        else {
            atkCollider.SetActive(false);
        }
    }
}

