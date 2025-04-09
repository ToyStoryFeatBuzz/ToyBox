using UnityEngine;
using UnityEngine.InputSystem;

namespace Toybox.InputSystem {
    public class MenuInputManager: MonoBehaviour {
        public static MenuInputManager Instance { get; private set; }

        public StInputEvent OnNavigateEvent;
        public StInputEvent OnAnyEvent;

        public bool IsLastInputMouse;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }
        
        public void OnNavigate(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnNavigateEvent, ctx);

        public void OnAny(InputAction.CallbackContext ctx) {
            IsLastInputMouse = ctx.action.activeControl.device.name == "Mouse";
        }

        public void OnMouseMove(InputAction.CallbackContext ctx) {
            IsLastInputMouse = true;
        }
    }
}