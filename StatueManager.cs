using UnityEngine;
using System.Collections;

public class StatueManager : MonoBehaviour {

    public RectTransform barLife;
    public Vector2 minMaxLifeBarPos;
    public GameObject[] scenes;

    public static int Life;

    float rangeLifePos;

    static float posLife;
    
    void Start() {
        rangeLifePos = minMaxLifeBarPos.y - minMaxLifeBarPos.x;
        Life = 1000;
    }
    void FixedUpdate() {
        posLife = ((Life / 1000f) * rangeLifePos) + minMaxLifeBarPos.x;
        if(posLife != barLife.localPosition.x) {
            UpdatePosition();
            
        }
        if(Life <= 0) {
            scenes[0].SetActive(false);
            scenes[1].SetActive(true);
        }
    }

    void UpdatePosition() {
        barLife.localPosition = new Vector2(posLife,barLife.localPosition.y);
    }
    

    public static void LifeRemove(int aux) {
        Life -= aux;
    }
}
