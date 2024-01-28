using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class Lever : MonoBehaviour {
	[SerializeField]
    private Powerable powerable;

    [SerializeField]
    private bool staysActivated = true;

	[SerializeField, Min(0), Tooltip("Time after which the lever resets to it's initial state. If 0, the lever does not reset.")]
	private float resetTime = 0f;

	[SerializeField, Space]
	private Transform handle;

	[SerializeField]
	private FloatRange handleRotationRange;

	private bool ResetsItself => resetTime > 0f;

	private float rotation = 0f, resetTimer = 0f;

	private bool isOn = false;

	private void Start() {
		rotation = handleRotationRange.min;
		ChangeTopHandleColor(staysActivated && !ResetsItself ? Color.red : Color.yellow);

	}

	private void OnValidate() {
		if (!Application.isPlaying) return;
		ChangeTopHandleColor(staysActivated && !ResetsItself ? Color.red : Color.yellow);
	}

	private void Update() {
		if (ResetsItself && isOn) {
			resetTimer += Time.deltaTime;

			if (resetTimer >= resetTime) {
				powerable.PowerOff();
				isOn = false;
				resetTimer = 0f;
				ChangeTopHandleColor(Color.yellow);
			}
		}

		rotation = Mathf.Lerp(handleRotationRange.min, handleRotationRange.max, powerable.Progress);
		handle.localRotation = Quaternion.Euler(rotation, 90f, -90f);
	}

	private void ChangeTopHandleColor(Color color) {
		Material topHandleMaterial = handle.GetComponent<MeshRenderer>().materials[^1];
		topHandleMaterial.color = color;
	}

	private void OnTriggerEnter(Collider other) {
		if (powerable == null) return;

		if (isOn && staysActivated) return;

		if (!other.GetComponent<Prop>())
			return;

		powerable.PowerOn();

		if (staysActivated) {
			isOn = true;
			
			if (ResetsItself)
				ChangeTopHandleColor(Color.red);
		}
	}
	
	private void OnTriggerExit(Collider other) {
		if (powerable == null) return;

        if (staysActivated) return;

		if (!other.GetComponent<Prop>())
			return;

		powerable.PowerOff();
	}
}
