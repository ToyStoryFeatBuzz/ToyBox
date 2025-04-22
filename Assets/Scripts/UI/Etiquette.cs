using TMPro;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.UI;

public class Etiquette : MonoBehaviour
{
    internal Player PlayerRef;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private RawImage _portrait;
    [SerializeField] private RawImage _background;
    [SerializeField] private RawImage _nameBackground;
    [SerializeField] private RawImage ultiImage;
    void Start()
    {
        PlayerRef.PlayerObject.GetComponent<Ultime>().callUltiEvent += UpdateUltiImage;
        SetPlayerEtiquette();
    }

    internal void SetPlayerEtiquette()
    {
        _name.text = PlayerRef.Name;
    }

    void UpdateUltiImage()
    {
        Debug.Log("Etiquette ult image to change");
    }
    
}
