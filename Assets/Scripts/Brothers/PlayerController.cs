using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	private Brothers brother;
	private Movement movement;
	private Vector2 move;
	private bool trombienie;

	private void Awake() {
		movement = GetComponent<Movement>();
		brother = GetComponent<Brothers>();
	}

	private void Update() {
		movement.Move(move);
	}

	private void FixedUpdate() {
		if (trombienie)
			brother.Trombone();
	}

	public void Move(InputAction.CallbackContext context) {
		if (context.performed)
			move = context.ReadValue<Vector2>();
		if (context.canceled)
			move = Vector2.zero;
	}
	
	public void Look(InputAction.CallbackContext context) {
		if (!context.performed)
			return;
		
		movement.Look(context.ReadValue<Vector2>());
	}
	
	public void Jump(InputAction.CallbackContext context) {
		if (context.performed)
			movement.Jump();
		if (context.canceled)
			movement.CutJump();
	}
	
	public void Trombone(InputAction.CallbackContext context) {
		if (context.performed)
			trombienie = true;
		if (context.canceled) {
			trombienie = false;
			brother.TromboneRelease();
		}
	}
	
	public void GroundPound(InputAction.CallbackContext context) {
		if (context.performed)
			brother.GroundPound();
	}
}
