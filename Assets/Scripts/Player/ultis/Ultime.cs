using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToyBox.Player
{
    public class Ultime : MonoBehaviour
    {
        [SerializeField] private float _duration;

        [FormerlySerializedAs("_canUlti")] public bool CanUlti = true;
        public event Action callUltiEvent;

        public virtual void Ultimate()
        {

        }

        public virtual void RestoreDefaultState()
        {

        }

        private void OnEnable()
        {
            callUltiEvent?.Invoke();
        }

        public void UseUltimate()
        {
            if (CanUlti)
            {
                callUltiEvent?.Invoke();
                StartCoroutine(UltimateCoroutine());
            }
        }

        private IEnumerator UltimateCoroutine()
        {
            Ultimate();
            CanUlti = false;
            yield return new WaitForSeconds(_duration);
            RestoreDefaultState();
        }
    }
}
