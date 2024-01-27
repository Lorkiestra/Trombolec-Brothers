using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PukaszTrombolec : Brothers {
    [SerializeField] private Prop succedObject;
    [SerializeField] private float throwForce = 200f;
    [SerializeField] private float succForce = 200f;
    [SerializeField] private float succTerminalVelocity = 10f;
    [SerializeField] private float succHoldDistance = 0.7f;

    [SerializeField] private float succDistortionPower = 1.3f;
    [SerializeField] private float pushDistortionPower = 3f;
    public override void Trombone() {
        audioSource.PlayOneShot(trombaPierdzenie);
        if (succedObject)
            return;
        
        trombaModel.localScale = Vector3.one * Random.Range(.3f, powiekszSwojaTrombe);

        SuccObjects();
    }

    public override void TromboneRelease() {
        audioSource.Stop();
        trombaModel.localScale = Vector3.one * 0.3f;
        RestoreGravity();
        
        if (!succedObject)
            return;

        ThrowObject();
    }

    private void ThrowObject() {
        //update every material within objects
        TrombaInjector[] distorts = succedObject.GetComponentsInChildren<TrombaInjector>();
        for (int i = 0; i < distorts.Length; i++)
        {
            distorts[i].succPower1 += pushDistortionPower;
        }

        succedObject.transform.parent = null;
        succedObject.rb.isKinematic = false;
        if (succedObject.throwable)
            succedObject.rb.AddForce(tromba.transform.forward * throwForce, ForceMode.Impulse);
        Movement brotherMovement = succedObject.GetComponent<Movement>();
        if (brotherMovement) {
            brotherMovement.canMove = true;
        }
        succedObject = null;
    }

    private void SuccObjects() {
        foreach (Prop prop in tromba.props) {
            //update every material within objects
            TrombaInjector[] distorts = prop.GetComponentsInChildren<TrombaInjector>();
            for (int i = 0; i < distorts.Length; i++)
            {
                distorts[i].succPower1 += succDistortionPower;
            }
            prop.rb.useGravity = false;
            prop.rb.AddForce(Vector3.Normalize(tromba.transform.position - prop.transform.position) * succForce);
            prop.rb.velocity = Vector3.ClampMagnitude(prop.rb.velocity, succTerminalVelocity);
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
        //update every material within object
        TrombaInjector[] distorts = prop.GetComponentsInChildren<TrombaInjector>();
        for (int i = 0; i < distorts.Length; i++)
        {
            distorts[i].succPower1 += succDistortionPower;
        }

        succedObject = prop;
        succedObject.transform.parent = tromba.transform;
        succedObject.rb.isKinematic = true;
        Movement brotherMovement = succedObject.GetComponent<Movement>();
        if (brotherMovement) {
            brotherMovement.canMove = false;
        }
        RestoreGravity();
        trombaModel.localScale = Vector3.one * 0.3f;
    }
}