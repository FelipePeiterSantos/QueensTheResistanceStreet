using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public static int scorePoints;
    static UnityEngine.UI.Text txtScore;

    void Start() {
        scorePoints = 0;
        txtScore = GetComponentInChildren<UnityEngine.UI.Text>();
        txtScore.text = scorePoints.ToString("0000");
    }

    static public void AddPoint(int pts) {
        scorePoints+=pts;
        txtScore.text = scorePoints.ToString("0000");
    }
}
