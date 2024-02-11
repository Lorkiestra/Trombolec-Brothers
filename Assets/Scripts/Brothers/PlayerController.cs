using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	
	[SerializeField] private Tromba tromba;
	
	private Brother brother;
	private Movement movement;
	private Vector2 move;
	private bool trombienie; // FIXME better name

	private void Awake() {
		brother = GetComponent<Brother>();
		movement = GetComponent<Movement>();
	}

	private void FixedUpdate() {
		movement.Move(move);
		if (trombienie)
			tromba.TromboneHold();
	}

	public void Move(InputAction.CallbackContext context) {
		if (context.performed)
			move = context.ReadValue<Vector2>();
		if (context.canceled)
			move = Vector2.zero;
	}
	
	public void Look(InputAction.CallbackContext context) {
		if (context.started)
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
		switch (context.phase) {
			case InputActionPhase.Started:
				trombienie = true;
				tromba.TromboneStart();
				break;
			case InputActionPhase.Canceled:
				trombienie = false;
				tromba.TromboneRelease();
				break;
		}
	}
	
	public void GroundPound(InputAction.CallbackContext context) {
		if (context.performed)
			brother.GroundPound();
	}
	
	public void SwitchControls(InputAction.CallbackContext context) {
		if (!context.performed)
			return;
		
		PlayerInput player1 = PlayerInput.all[0];
		PlayerInput player2 = PlayerInput.all[1];
		string tempControlScheme = player2.currentControlScheme;
		InputDevice[] tempDevices = player2.devices.ToArray();
		player2.SwitchCurrentControlScheme(player1.currentControlScheme, player1.devices.ToArray());
		player1.SwitchCurrentControlScheme(tempControlScheme, tempDevices);
	}
}