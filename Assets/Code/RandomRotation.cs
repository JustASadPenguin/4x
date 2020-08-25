using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {
    private void Awake() {
        float angle = Random.Range(0f, 359f); // 0 degrees == 360 degrees
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
