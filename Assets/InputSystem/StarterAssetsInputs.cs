using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		bool _mouseOneHeld;
		bool _mouseTwoPressed;
		bool _mouseTwoReleased;

		bool _escapePressed;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnSuck(InputValue value)
        {
			MouseOneInput(value.isPressed);
        }
		public void OnToggle(InputValue value)
        {
			MouseTwoPressed(value.isPressed);
		}
		public void OnPause(InputValue value)
        {
			EscapePressed(value.isPressed);
        }
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		public void MouseOneInput(bool isHolding)
		{
			_mouseOneHeld = isHolding;
			if (isHolding)
			{
				Debug.Log("Holding mouse one");
			}
		}
		private void MouseTwoPressed(bool buttonPressed)
		{
			_mouseTwoPressed = buttonPressed;
			_mouseTwoReleased = !buttonPressed;
			Debug.Log("Mouse 2 pressed");
		}
		public void RevertFireRelease()
        {
			_mouseTwoReleased = false;
        }
		void EscapePressed(bool pressed)
        {
			_escapePressed = pressed;
        }
		public void RevertEscape()
        {
			_escapePressed = false;
        }
		public bool MouseOneHeld => _mouseOneHeld;
		public bool MouseTwoReleased => _mouseTwoReleased;
		public bool IsMouseTwoPressed => _mouseTwoPressed;
		public bool EscapeIsressed => _escapePressed;
	}
	
}