
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Toybox.InputSystem {
    public class PlayerInputManager : MonoBehaviour {
        public StInputEvent JumpEvent;
         
        public StInputEvent MoveEvent;
        public float MoveValue;
        
        public StInputEvent PlaceEvent;
        public StInputEvent GridMoveEvent;
        public Vector2 GridMoveDir;


        private void Start() {
            JumpEvent.Started += () => { Debug.Log(gameObject.name + "Jump"); };
            PlaceEvent.Started += () => { Debug.Log(gameObject.name + "Place"); };
        }

        public void OnJump(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(JumpEvent, ctx);

        public void OnMove(InputAction.CallbackContext ctx) {
            MoveValue = ctx.ReadValue<float>();
            InputEventSystem.InvokeInputEvent(MoveEvent, ctx);
        }

        public void OnPlace(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(PlaceEvent, ctx);
        
        public void OnGridMove(InputAction.CallbackContext ctx) {
            GridMoveDir = ctx.ReadValue<Vector2>();
            InputEventSystem.InvokeInputEvent(GridMoveEvent, ctx);
        }

    }
    
    
}

