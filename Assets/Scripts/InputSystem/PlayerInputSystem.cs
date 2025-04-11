using ToyBox.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using static ToyBox.Enums.EPlayerState;

namespace ToyBox.InputSystem {
    public class PlayerInputSystem : MonoBehaviour {
        public StInputEvent OnJumpEvent;
         
        public StInputEvent OnMoveEvent;
        public float MoveValue;
        
        public StInputEvent OnPlaceEvent;
        public StInputEvent OnGridMoveEvent;
        public StInputEvent OnRotateRightEvent;
        public StInputEvent OnRotateLeftEvent;
        public Vector2 GridMoveDir;

        public StInputEvent OnPauseEvent;

        public bool IsDead;
        
        PlayerManager _playerManager => PlayerManager.Instance;

        public void OnJump(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnJumpEvent, ctx);

        public void OnMove(InputAction.CallbackContext ctx) {
            MoveValue = ctx.ReadValue<float>();
            InputEventSystem.InvokeInputEvent(OnMoveEvent, ctx);
        }
        
        public void OnPause(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnPauseEvent, ctx);

        public void OnPlace(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnPlaceEvent, ctx);
        public void OnRotateRight(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnRotateRightEvent, ctx);
        public void OnRotateLeft(InputAction.CallbackContext ctx) => InputEventSystem.InvokeInputEvent(OnRotateLeftEvent, ctx);
        
        public void OnGridMove(InputAction.CallbackContext ctx) {
            GridMoveDir = ctx.ReadValue<Vector2>();
            InputEventSystem.InvokeInputEvent(OnGridMoveEvent, ctx);
        }

        public void SetDeath() {
            _playerManager.SetPlayerState(gameObject, Dead);
            IsDead = true;
            gameObject.transform.position = new Vector2(-999, -999); //Send the dead out of the map to avoid clutter on the race
        }

    }
    
    
}

