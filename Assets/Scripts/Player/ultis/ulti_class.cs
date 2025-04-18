using System.Collections;
using ToyBox.Player;
using UnityEngine;

public class ulti : MonoBehaviour
{
    [SerializeField] private float _duration;

    private bool _canUlti = true;
    public virtual void Ultimate()
    {
        
    }

    public virtual void RestoreDefaultState()
    {
        
    }

    public void UseUltimate()
    {
        if (_canUlti)
        {
            Debug.Log("Ultimate");
            StartCoroutine(UltimateCoroutine());
        }
    }

    private IEnumerator UltimateCoroutine()
    {
        Ultimate();
        _canUlti = false;
        yield return new WaitForSeconds(_duration);
        RestoreDefaultState();
        _canUlti = true;
    }
}
