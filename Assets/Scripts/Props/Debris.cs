using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void DropAndDestroy() {
        rb.isKinematic = false;
        rb.AddExplosionForce(10f, transform.position, 10f);
        StartCoroutine(EDropAndDestroy());
    }
    
    IEnumerator EDropAndDestroy() {
        for (float i = 0f; i < 2f; i += Time.deltaTime) {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i / 2f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
