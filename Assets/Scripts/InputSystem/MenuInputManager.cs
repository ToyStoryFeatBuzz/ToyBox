using ToyBox.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ToyBox.InputSystem {
    public class MenuInputManager: MonoBehaviour {
        public static MenuInputManager Instance { get; private set; }

        public StInputEvent OnNavigateEvent;
        public Vector2 NavigationValue;
        
        public StInputEvent OnSubmitEvent;
        public StInputEvent OnCanceledEvent;

        public bool IsLastInputMouse;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

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
        
        public void ReleasePause(InputAction.CallbackContext ctx) => PauseManager.Instance.EndPause();
        
        
    }
}