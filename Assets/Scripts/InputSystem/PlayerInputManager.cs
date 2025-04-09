
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Toybox.InputSystem {
    public class PlayerInputManager : MonoBehaviour {
        public StInputEvent OnJumpEvent;
         
        public StInputEvent OnMoveEvent;
        public float MoveValue;
        
        public StInputEvent OnPlaceEvent;
        public StInputEvent OnGridMoveEvent;
        public Vector2 GridMoveDir;


        private void Start() {
            OnJumpEvent.Started += () => { Debug.Log(gameObject.name + "Jump"); };
            OnPlaceEvent.Started += () => { Debug.Log(gameObject.name + "Place"); };
        }

        public void OnJump(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnJumpEvent, ctx);

        public void OnMove(InputAction.CallbackContext ctx) {
            MoveValue = ctx.ReadValue<float>();
            InputEventSystem.InvokeInputEvent(OnMoveEvent, ctx);
        }

        public void OnPlace(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnPlaceEvent, ctx);
        
        public void OnGridMove(InputAction.CallbackContext ctx) {
            GridMoveDir = ctx.ReadValue<Vector2>();
            InputEventSystem.InvokeInputEvent(OnGridMoveEvent, ctx);
        }

    }
    
    
}

