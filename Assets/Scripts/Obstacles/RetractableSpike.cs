using System.Collections;
using ToyBox.Player;
using UnityEngine;

public class RetractableSpike : MonoBehaviour
{
    [SerializeField] float _retractTime;
    [SerializeField] bool _spikeUp;
    [SerializeField] GameObject _spike;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position+Vector3.up, new Vector3(3, 2, 0));
    }

    void Update()
    {
        if (!_spikeUp)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + Vector3.up, new Vector3(3, 2, 0),0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.GetComponent<PlayerMovement>())
                {
                    Debug.Log("Raising spikes");
                    StartCoroutine(ResettingSpiking());
                }
            }
        } 
    }

    IEnumerator ResettingSpiking()
    {
        float timer = 0;
        _spikeUp = true;
        _spike.SetActive(true);
        
        while (timer < _retractTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _spike.SetActive(false);
        _spikeUp = false;
    }
}
