using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtTromba : Tromba {

	// FIXME
	[SerializeField] private Movement movement;
	[SerializeField] private Rigidbody rb;

	[SerializeField] private float turbopierdForce = 10000f;
	[SerializeField] private float speedClamp = 50f;
	[SerializeField] private float trombaPushForce = 200f;
	[SerializeField] private float trombaPushTerminalVelocity = 10f;
	[SerializeField] private float trombaPushDistortionSpeed = 2f;
	[SerializeField] private float trombaPushDistortionPower = -5f;

	protected override void Update() {
		base.Update();
	}

	public override void TromboneStart()
	{
		base.TromboneStart();
		if (!CanAttack)
			return;
		if (!audioSource.isPlaying)
			audioSource.Play();
	}

	public override void TromboneHold() {
		if (!CanAttack)
			return;
		trombaModel.localScale = Vector3.one * Random.Range(1f, trombaModelMaxSize);
		if (!movement.IsGrounded)
			rb.AddForce(-propCollector.transform.forward * turbopierdForce, ForceMode.Impulse);
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedClamp);
		TrombonePush();
	}

	public override void TromboneRelease() {
		if (!CanAttack)
			return;
		audioSource.Stop();
		trombaModel.localScale = Vector3.one;
	}

	private void TrombonePush() {
		foreach (Prop prop in propCollector.Props) {
			//update every material within objects
			// FIXME duplicated code
			TrombaInjector[] distorts = prop.GetComponentsInChildren<TrombaInjector>();
			foreach (TrombaInjector d in distorts)
			{
				d.succPower2 = trombaPushDistortionPower;
				d.succSpeed2 = trombaPushDistortionSpeed;
			}
			prop.rb.AddForce(Vector3.Normalize(prop.transform.position - propCollector.transform.position) * trombaPushForce);
			prop.rb.velocity = Vector3.ClampMagnitude(prop.rb.velocity, trombaPushTerminalVelocity);
		}
	}
}
