using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FidgetSpinnerWeakPoint : MonoBehaviour {
    [field: SerializeField]
    public bool isDestroyed { get; private set; } = false;
    
    private void OnCollisionEnter(Collision other) {
        if (!isDestroyed && other.gameObject.TryGetComponent(out Brothers player)) {
            isDestroyed = true;
            GetComponent<MeshCollider>().enabled = false;
        }
    }
}