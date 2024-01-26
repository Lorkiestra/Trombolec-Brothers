using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukaszTrombolec : Brothers {
    [SerializeField] private Prop succedObject;
    [SerializeField] private PropCollector tromba;
    [SerializeField] private float throwForce = 100f;
    [SerializeField] private float succForce = 30f;

    public override void Trombone() {
        Debug.Log("trombone");
        if (succedObject)
            return;

        SuccObjects();
    }

    public override void TromboneRelease() {
        Debug.Log("trombone release");
        RestoreGravity();
        
        if (!succedObject)
            return;

        ThrowObject();
    }

    private void ThrowObject() {
        succedObject.transform.parent = null;
        succedObject.rb.isKinematic = false;
        succedObject.rb.AddForce(tromba.transform.forward * throwForce);
    }

    private void SuccObjects() {
        foreach (Prop prop in tromba.props) {
            prop.rb.useGravity = false;
            prop.rb.AddForce(-tromba.transform.forward * succForce);
            if (Vector3.Distance(prop.transform.position, tromba.transform.position) < 0.5f) {
                succedObject = prop;
                succedObject.transform.parent = transform;
                succedObject.rb.isKinematic = true;
                RestoreGravity();
                return;
            }
        }
    }

    private void RestoreGravity() {
        foreach (Prop prop in tromba.props) {
            prop.rb.useGravity = true;
        }
    }
}