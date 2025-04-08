
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Toybox.InputSystem {
    public class PlayerInputManager : MonoBehaviour {
        public StInputEvent JumpEvent;
         
        public StInputEvent MoveEvent;
        public float MoveValue;
        
        public StInputEvent GridMoveEvent;
        public Vector2 GridMoveDir;
        public StInputEvent PlaceEvent;


        private void Start() {
            JumpEvent.Started += () => { Debug.Log(gameObject.name + "Jump"); };
        }

        public void OnJump(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(JumpEvent, ctx);

        public void OnMove(InputAction.CallbackContext ctx) {
            MoveValue = ctx.ReadValue<float>();
            InputEventSystem.InvokeInputEvent(MoveEvent, ctx);
        }
        
        public void OnGridMove(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(GridMoveEvent, ctx);
        public void OnPlace(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(PlaceEvent, ctx);
    }
    
    
}

