using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Powerable {
	[SerializeField] private Transform model;
	[SerializeField] private Collider c;
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private Transform closedPosition;
	[SerializeField] private Transform openPosition;
	
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
		float lerp = 0f;
		Vector3 startPosition = model.position;
		while (lerp < 1f) {
			model.position = Vector3.Lerp(startPosition, transform.position + Vector3.down * 2f, curve.Evaluate(lerp));
			lerp += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator EPowerOff() {
		float lerp = 0f;
		Vector3 startPosition = model.position;
		while (lerp < 1f) {
			model.position = Vector3.Lerp(startPosition, transform.position, curve.Evaluate(lerp));
			lerp += Time.deltaTime;
			yield return null;
		}
	}
}
