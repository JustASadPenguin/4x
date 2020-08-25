using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeight : MonoBehaviour {
    private void Awake() {
        var pos = transform.position;
        float height = Mathf.PerlinNoise(pos.x, pos.z) * 2.0f;
        transform.position = transform.position + Vector3.up * height;
    }
}
