using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Prop : MonoBehaviour {
    // FIXME hide rb and use methods instead
    public Rigidbody rb;
    public bool throwable = true;
    public bool succable = true;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
}
