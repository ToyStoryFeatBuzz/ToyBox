using ToyBox.Managers;
using ToyBox.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

namespace ToyBox.InputSystem {
    public class MenuInputManager: MonoBehaviour {
        public static MenuInputManager Instance { get; private set; }

        public StInputEvent OnNavigateEvent;

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
        
        public void ReleasePause(InputAction.CallbackContext ctx) => PauseManager.Instance.EndPause();
        
        
    }
}