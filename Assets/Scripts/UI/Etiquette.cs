using TMPro;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.UI;

public class Etiquette : MonoBehaviour
{
    internal Player PlayerRef;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _portrait;
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
        _background.color = PlayerRef.Color;
        _portrait.sprite = PlayerRef.sprite;
    }

    void UpdateUltiImage()
    {
        if (ultiImage.color.a < 1)
        {
            ultiImage.color = new Color(1f, 1f, 1f, 1f);
            return;
        }
        ultiImage.color=new Color(1, 1, 1, 0.2f);
    }
    
}
