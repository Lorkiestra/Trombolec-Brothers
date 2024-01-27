using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawelTrombolec : Brothers {
    [SerializeField] private float turbopierdForce = 10000f;
    [SerializeField] private float speedClamp = 50f;
    [SerializeField] private float trombaPushForce = 200f;
    [SerializeField] private float trombaPushTerminalVelocity = 10f;
    [SerializeField] private float trombapushDistortionSpeed = 2f;
    [SerializeField] private float trombaPushDistortionPower = -5f;

    protected override void Awake()
    {
        base.Awake();
        brotherType = 2;
    }

    public override void Update() {
        base.Update();
        Debug.DrawRay(tromba.transform.position, tromba.transform.forward, Color.yellow);
    }

    public override void Trombone() {
        if (stunnedTime > 0f)
            return;
        audioSource.PlayOneShot(trombaPierdzenie);
        trombaModel.localScale = Vector3.one * UnityEngine.Random.Range(.3f, powiekszSwojaTrombe);
        if (!movement.grounded)
            rb.AddForce(-tromba.transform.forward * turbopierdForce, ForceMode.Impulse);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedClamp);
        TrombonePush();
    }

    public override void TromboneRelease() {
        if (stunnedTime > 0f)
            return;
        audioSource.Stop();
        trombaModel.localScale = Vector3.one * .3f;
    }

    void TrombonePush() {
        foreach (Prop prop in tromba.props) {
            //update every material within objects
            TrombaInjector[] distorts = prop.GetComponentsInChildren<TrombaInjector>();
            for (int i = 0; i < distorts.Length; i++)
			{
                distorts[i].succPower2 = trombaPushDistortionPower;
                distorts[i].succSpeed2 = trombapushDistortionSpeed;
            }
            prop.rb.AddForce(Vector3.Normalize(prop.transform.position - tromba.transform.position) * trombaPushForce);
            prop.rb.velocity = Vector3.ClampMagnitude(prop.rb.velocity, trombaPushTerminalVelocity);
        }
    }
}
