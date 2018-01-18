using UnityEngine;
using System.Collections;

public class LifePlayerManager : MonoBehaviour {

    public RectTransform barLife;
    public RectTransform barEnergy;

    public Vector2 minMaxLifeBarPos;
    public Vector2 minMaxEnergyBarPos;
    public GameObject okUlti;

    public static int Life;
    public static int Energy;
    public static bool ActiveUlti;

    float rangeLifePos;
    float rangeEnergyPos;

    static float posLife;
    static float posEnergy;
    
    void Start() {
        rangeLifePos = minMaxLifeBarPos.y - minMaxLifeBarPos.x;
        rangeEnergyPos = minMaxEnergyBarPos.y - minMaxEnergyBarPos.x;

        Life = 100;
        Energy = 0;
        ActiveUlti = false;
        okUlti.SetActive(false);
    }
    void FixedUpdate() {
        posLife = ((Life / 100f) * rangeLifePos) + minMaxLifeBarPos.x;
        posEnergy = ((Energy / 100f) * rangeEnergyPos) + minMaxEnergyBarPos.x;
        if(posLife != barLife.localPosition.x || posEnergy != barEnergy.localPosition.x) {
            UpdatePosition();
        }

        if(ActiveUlti && !okUlti.activeSelf) {
            okUlti.SetActive(true);
        }
        else if(!ActiveUlti && okUlti.activeSelf) {
            okUlti.SetActive(false);
        }
    }

    void UpdatePosition() {
        barLife.localPosition = new Vector2(posLife,barLife.localPosition.y);
        barEnergy.localPosition = new Vector2(posEnergy,barEnergy.localPosition.y);
    }
    

    public static void LifeModify(int aux) {
        Life += aux;
        if (Life > 100) {
            Life = 100;
        }
        else if(Life < 0) {
            Life = 0;
        }
    }

    public static void EnergyModify(int aux) {
        if (aux < 0) {
            Energy = 0;
            ActiveUlti = false;
        }
        else if (Energy < 100) {
            Energy += aux;
        }
        else if(Energy >= 100) {
            Energy = 100;
            ActiveUlti = true;
        }        
    }
}
