using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

    public Transform[] xBounds;

    static GameObject player;

    static int cameraLockZ = -10;
    bool followPlayer;

    void Start() {
        while (player == null) {
            player = GameObject.Find("Player");
        }
        followPlayer = true;
    }

    void Update() {
        if(followPlayer && player.transform.position.x > xBounds[0].position.x && player.transform.position.x < xBounds[1].position.x ) {
            transform.position = new Vector3(player.transform.position.x,transform.position.y,cameraLockZ);
        }

        if(Cursor.visible){
            Cursor.visible = false;
        }
    }

    
}
