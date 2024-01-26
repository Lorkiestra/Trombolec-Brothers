using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PukaszTrombolec : Brothers {
    [SerializeField] private Prop succedObject;
    [SerializeField] private PropCollector tromba;
    [SerializeField] private float throwForce = 200f;
    [SerializeField] private float succForce = 200f;
    [SerializeField] private float succTerminalVelocity = 10f;
    [SerializeField] private float succHoldDistance = 0.7f;

    public override void Trombone() {
        if (succedObject)
            return;

        SuccObjects();
    }

    public override void TromboneRelease() {
        RestoreGravity();
        
        if (!succedObject)
            return;

        ThrowObject();
    }

    private void ThrowObject() {
        Debug.Log($"throw {succedObject.name}");
        succedObject.transform.parent = null;
        succedObject.rb.isKinematic = false;
        succedObject.rb.AddForce(tromba.transform.forward * throwForce, ForceMode.Impulse);
        Movement brotherMovement = succedObject.GetComponent<Movement>();
        if (brotherMovement) {
            brotherMovement.canMove = true;
        }
        succedObject = null;
    }

    private void SuccObjects() {
        foreach (Prop prop in tromba.props) {
            prop.rb.useGravity = false;
            prop.rb.AddForce(Vector3.Normalize(tromba.transform.position - prop.transform.position) * succForce);
            prop.rb.velocity = Vector3.ClampMagnitude(prop.rb.velocity, succTerminalVelocity);
            Debug.Log(Vector3.Distance(prop.transform.position, tromba.transform.position));
            if (Vector3.Distance(prop.transform.position, tromba.transform.position) < succHoldDistance) {
                HoldObject(prop);
                return;
            }
        }
    }

    private void RestoreGravity() {
        foreach (Prop prop in tromba.props) {
            prop.rb.useGravity = true;
        }
    }

    void HoldObject(Prop prop) {
        succedObject = prop;
        succedObject.transform.parent = tromba.transform;
        succedObject.rb.isKinematic = true;
        Movement brotherMovement = succedObject.GetComponent<Movement>();
        if (brotherMovement) {
            brotherMovement.canMove = false;
        }
        RestoreGravity();
    }
}