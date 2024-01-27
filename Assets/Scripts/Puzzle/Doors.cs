using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Powerable {
	[SerializeField] private Transform model;
	[SerializeField] private Collider c;
	[SerializeField] private float lowerHeight = 2f;
	
	public override void PowerOn() {
		c.enabled = false;
		StopAllCoroutines();
		StartCoroutine(EPowerOn());
	}

	public override void PowerOff() {
		c.enabled = true;
		StopAllCoroutines();
		StartCoroutine(EPowerOff());
	}

	IEnumerator EPowerOn() {
		Vector3 startPosition = model.position;

		while (Progress < 1f) {
			model.position = Vector3.Lerp(startPosition, transform.position + Vector3.down * lowerHeight, Curve.Evaluate(Progress));
			Progress += Time.deltaTime;
			yield return null;
		}

		Progress = 1f;
	}

	IEnumerator EPowerOff() {
		Vector3 startPosition = model.position;

		while (Progress > 0f) {
			model.position = Vector3.Lerp(startPosition, transform.position, Curve.Evaluate(1f - Progress));
			Progress -= Time.deltaTime;
			yield return null;
		}

		Progress = 0f;
	}
}
