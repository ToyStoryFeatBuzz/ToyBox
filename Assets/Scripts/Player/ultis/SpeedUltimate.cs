using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Player {
    public class SpeedUltimate : Ultime {
        [SerializeField] private float _speedBoost;
        private PlayerMovement _movement;
        [SerializeField] private ParticleSystem _speedBoostParticle;

        private float _baseSpeed;

        void Start() {
            _movement = GetComponent<PlayerMovement>();
            GameModeManager.Instance.OnRaceStartIntern += () => { _canUlti = true;};
            _baseSpeed = _movement.MaxSpeed;
            _speedBoostParticle.Stop();
        }

        public override void Ultimate() {
            _movement.MaxSpeed += _speedBoost;
            _speedBoostParticle.Play();
        }

        public override void RestoreDefaultState() {
            _movement.MaxSpeed = _baseSpeed;
            _speedBoostParticle.Stop();
        }
    }
}