using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Platform : Powerable {
	[SerializeField] private Transform positionA;
	[SerializeField] private Transform positionB;
	[SerializeField] private Transform model;
	[SerializeField] private AnimationCurve curve;

	private void Start() {
		model.position = positionA.position;
	}

	public override void PowerOn() {
		StopAllCoroutines();
		StartCoroutine(EPowerOn());
	}

	public override void PowerOff() {
		StopAllCoroutines();
		StartCoroutine(EPowerOff());
	}

	IEnumerator EPowerOn() {
		float lerp = 0;
		Vector3 startPosition = model.position;
		while (lerp < 1f) {
			model.position = Vector3.Lerp(startPosition, positionB.position, curve.Evaluate(lerp));
			lerp += Time.deltaTime;
			yield return null;
		}
		model.position = positionB.position;
	}

	IEnumerator EPowerOff() {
		float lerp = 0;
		Vector3 startPosition = model.position;
		while (lerp < 1f) {
			model.position = Vector3.Lerp(startPosition, positionA.position, curve.Evaluate(lerp));
			lerp += Time.deltaTime;
			yield return null;
		}
		model.position = positionA.position;
	}
}
