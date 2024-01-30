using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FidgetSpinnerWeakPoint : MonoBehaviour {
    [SerializeField]
    private FidgetSpinner fidgetSpinner;

    [field: SerializeField]
    public bool IsDestroyed { get; private set; } = false;
    
    private void OnTriggerEnter(Collider other) {
        if (!IsDestroyed && other.gameObject.TryGetComponent(out Brothers player)) {
            IsDestroyed = true;
            GetComponent<MeshRenderer>().enabled = false;
            fidgetSpinner.Unstun();
        }
    }
}