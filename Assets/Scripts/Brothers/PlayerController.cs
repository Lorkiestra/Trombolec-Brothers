using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	private Movement movement;
	private Vector2 move;
	
	private void Awake() {
		movement = GetComponent<Movement>();
	}

	private void Update() {
		movement.Move(move);
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
		if (!context.performed)
			return;
		
		movement.Jump();
	}
}
