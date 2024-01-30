using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Platform : Powerable {
	[SerializeField] private Transform positionA;
	[SerializeField] private Transform positionB;
	[SerializeField] private Transform model;
	[SerializeField] private float time = 1f;

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
		while (Progress < time) {
			model.position = Vector3.Lerp(positionA.position, positionB.position, Curve.Evaluate(Progress));
			Progress += Time.deltaTime / time;
			yield return null;
		}

		Progress = 1f;
		model.position = positionB.position;
	}

	IEnumerator EPowerOff() {
		while (Progress > 0f) {
			model.position = Vector3.Lerp(positionA.position, positionB.position, Curve.Evaluate(Progress));
			Progress -= Time.deltaTime / time;
			yield return null;
		}

		Progress = 0f;
		model.position = positionA.position;
	}
}
