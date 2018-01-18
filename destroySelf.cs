using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

    public float delay;

	IEnumerator Start() {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
