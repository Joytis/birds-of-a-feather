using UnityEngine;
using UnityEngine.InputSystem;

namespace LWG {

    public enum ButtonState {
        Up,
        Down,
        Unknown,
    }

    public static class InputActionExtensions {
        public static ButtonState ReadButtonState(this InputAction.CallbackContext ctx) {
            float state = ctx.ReadValue<float>();
            if(Mathf.Approximately(state, 1.0f)) { return ButtonState.Down; }
            if(Mathf.Approximately(state, 0.0f)) { return ButtonState.Up; }
            return ButtonState.Unknown; 
        }
    }
}