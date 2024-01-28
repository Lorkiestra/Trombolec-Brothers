using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class Lever : MonoBehaviour {
	[field: SerializeField]
    public Powerable Powerable { get; set; }

    [field: SerializeField]
    public bool StaysActivated { get; set; } = true;

	[SerializeField, Min(0), Tooltip("Time after which the lever resets to it's initial state. If 0, the lever does not reset.")]
	private float resetTime = 0f;

	[SerializeField, Space]
	private Transform handle;

	[SerializeField]
	private FloatRange handleRotationRange;

	public bool IsOn { get; set; } = false;

	public bool IsDisabled => Powerable == null;

	private bool ResetsItself => resetTime > 0f;

	private float rotation = 0f, resetTimer = 0f;

	[SerializeField] private Renderer topHandleRenderer;

	private void Awake() {
		topHandleRenderer = transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
	}

	private void Start() {
		rotation = handleRotationRange.min;
		ChangeTopHandleColor((StaysActivated && IsOn) || IsDisabled ? Color.red : Color.yellow);

	}

	/*
	private void OnValidate() {
		if (!Application.isPlaying) return;
		ChangeTopHandleColor((StaysActivated && IsOn) || IsDisabled ? Color.red : Color.yellow);
	}
	*/

	private void Update() {
		if (ResetsItself && IsOn) {
			resetTimer += Time.deltaTime;

			if (resetTimer >= resetTime) {
				Powerable.PowerOff();
				IsOn = false;
				resetTimer = 0f;
				ChangeTopHandleColor(Color.yellow);
			}
		}

		if (IsDisabled) return;

		rotation = Mathf.Lerp(handleRotationRange.min, handleRotationRange.max, Powerable.Progress);
		handle.localRotation = Quaternion.Euler(rotation, 90f, -90f);
	}

	private void ChangeTopHandleColor(Color color) {
		topHandleRenderer.materials[^1].color = color;
	}

	private void OnTriggerEnter(Collider other) {
		if (IsDisabled) return;

		if (IsOn && StaysActivated) return;

		if (!other.GetComponent<Brothers>())
			return;

		Powerable.PowerOn();
		IsOn = true;

		if (StaysActivated)
			ChangeTopHandleColor(Color.red);
	}
	
	private void OnTriggerExit(Collider other) {
		if (IsDisabled) return;

        if (StaysActivated) return;

		if (!other.GetComponent<Brothers>())
			return;

		Powerable.PowerOff();

		if (ResetsItself)
			ChangeTopHandleColor(Color.yellow);
	}
}
