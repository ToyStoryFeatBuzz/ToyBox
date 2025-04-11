using System;
using UnityEngine.InputSystem;

namespace ToyBox.InputSystem {
    public static class InputEventSystem {
        public static void InvokeInputEvent(StInputEvent inputEvent, InputAction.CallbackContext ctx) {
            if (ctx.started) {
                inputEvent.StartEvent();
            } else if (ctx.performed) {
                inputEvent.PerformEvent();
            } else if (ctx.canceled) {
                inputEvent.CancelEvent();
            }
        }
    }
    
    public struct StInputEvent {
        public event Action Started;
        public event Action Canceled;
        public event Action Performed;
        
        public void StartEvent() => Started?.Invoke();
        public void PerformEvent() => Performed?.Invoke();
        public void CancelEvent() => Canceled?.Invoke();
    }
}