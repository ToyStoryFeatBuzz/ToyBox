using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowMap : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _imageMap;
    [SerializeField] private TextMeshProUGUI _text;
    public string MapName;

    private void Start()
    {
        _canvas.gameObject.SetActive(false);
        _text.text = MapName;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _canvas.gameObject.SetActive(true);
        Debug.Log("OnTriggerEnter2D");
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _canvas.gameObject.SetActive(false);
        Debug.Log("OnTriggerExit2D");
    }
}
