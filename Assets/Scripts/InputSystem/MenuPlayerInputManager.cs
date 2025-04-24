using ToyBox.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ToyBox.InputSystem {
    public class MenuPlayerInputManager : MonoBehaviour {
        public StInputEvent OnNavigateEvent;
        public Vector2 NavigationValue;
        
        public StInputEvent OnSubmitEvent;
        public StInputEvent OnCanceledEvent;

        public bool IsLastInputMouse;
        
        public void OnNavigate(InputAction.CallbackContext ctx) {
            NavigationValue = ctx.ReadValue<Vector2>();
            InputEventSystem.InvokeInputEvent(OnNavigateEvent, ctx);
        }
        
        public void OnSubmit(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnSubmitEvent, ctx);
        
        public void OnCanceled(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnCanceledEvent, ctx);

        public void OnAny(InputAction.CallbackContext ctx) {
            IsLastInputMouse = ctx.action.activeControl.device.name == "Mouse";
        }

        public void OnMouseMove(InputAction.CallbackContext ctx) {
            IsLastInputMouse = true;
        }
        
        public void OnReleasePause(InputAction.CallbackContext ctx) => PauseManager.Instance.EndPause();
    }
}