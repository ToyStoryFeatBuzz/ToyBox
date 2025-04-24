using System;
using System.Collections;
using UnityEngine;

namespace ToyBox.Player
{
    public class Ultime : MonoBehaviour
    {
        [SerializeField] private float _duration;

        public bool _canUlti = true;
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
            Debug.Log("Ultimate" + _canUlti);
            if (_canUlti)
            {
                callUltiEvent?.Invoke();
                StartCoroutine(UltimateCoroutine());
            }
        }

        private IEnumerator UltimateCoroutine()
        {
            Ultimate();
            _canUlti = false;
            yield return new WaitForSeconds(_duration);
            RestoreDefaultState();
        }
    }
}
