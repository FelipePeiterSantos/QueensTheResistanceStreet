using UnityEngine;
using System.Collections;

public class crown : MonoBehaviour {

    public UnityEngine.UI.Image[] go;

    static AudioSource audioSource;
    static Transform player;

    SpriteRenderer body;

    void Start() {
        body = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player").transform;
        go[0].color = Color.clear;
        go[1].color = Color.clear;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.name.Contains("e_atkCollider")) {
            StatueManager.LifeRemove(2);
            StartCoroutine("HitFeed");
        }
        else if(coll.name.Contains("t_atkCollider")) {
            StatueManager.LifeRemove(6);
            StartCoroutine("HitFeed");
        }
    }

    IEnumerator HitFeed() {
        if(!audioSource.isPlaying) {
            audioSource.pitch = Random.Range(0.9f,1.1f);
            audioSource.Play();
        }

        if(!body.isVisible) {
            if(transform.position.x < player.position.x) {
                go[0].color = Color.white;
            }
            else {
                go[1].color = Color.white;
            }
        }
        else {
            body.color = Color.red;
        }
        yield return new WaitForSeconds(0.05f);
        body.color = Color.white;
        if(body.isVisible) {
            go[0].color = Color.clear;
            go[1].color = Color.clear;
        }
    }
}
