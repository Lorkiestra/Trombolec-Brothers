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

	[SerializeField, Space]
	private Transform handle;

	[SerializeField]
	private FloatRange handleRotationRange;

	private float rotation = 0;

	private bool isOn = false;

	private void Start() {
		rotation = handleRotationRange.min;

		Material topHandleMaterial = handle.GetComponent<MeshRenderer>().materials[^1];
		topHandleMaterial.color = staysActivated ? Color.red : Color.yellow;
	}

	private void OnValidate() {
		if (!Application.isPlaying) return;
		
		Material topHandleMaterial = handle.GetComponent<MeshRenderer>().materials[^1];
		topHandleMaterial.color = staysActivated ? Color.red : Color.yellow;
	}

	private void Update() {
		rotation = Mathf.Lerp(handleRotationRange.min, handleRotationRange.max, powerable.Progress);
		handle.localRotation = Quaternion.Euler(rotation, 90f, -90f);
	}

	private void OnTriggerEnter(Collider other) {
		if (powerable == null) return;

		if (isOn && staysActivated) return;

		if (!other.GetComponent<Prop>())
			return;

		powerable.PowerOn();

		if (staysActivated)
			isOn = true;
	}
	
	private void OnTriggerExit(Collider other) {
		if (powerable == null) return;

        if (staysActivated) return;

		if (!other.GetComponent<Prop>())
			return;

		powerable.PowerOff();
	}
}
