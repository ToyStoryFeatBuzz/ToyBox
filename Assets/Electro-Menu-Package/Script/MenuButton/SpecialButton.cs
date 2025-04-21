using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class SpecialButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Images to modify")]
    public ImageCustom[] _imageList;

    [Header("Texts to modify")]
    public TextCustom[] _textList;

    [Header("Events")]
    [FormerlySerializedAs("onClick")]
    [SerializeField] private ButtonClickedEvent _onClick = new ButtonClickedEvent();

    [FormerlySerializedAs("onEnter")]
    [SerializeField] private ButtonClickedEvent _onEnter = new ButtonClickedEvent();

    [FormerlySerializedAs("onExit")]
    [SerializeField] private ButtonClickedEvent _onExit = new ButtonClickedEvent();

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonInteraction("Enter");
        _onEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonInteraction("Exit");
        _onExit.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonInteraction("Click");
        _onClick.Invoke();
    }

    private void ButtonInteraction(string interactionType)
    {
        ModifyImages(interactionType);
        ModifyTexts(interactionType);

    }

    #region ImageModification
    private void ModifyImages(string interactionType)
    {
        if (_imageList != null)
        {
            for (int i = 0; i < _imageList.Length; i++)
            {
                if (_imageList[i] != null)
                {
                    switch (interactionType)
                    {
                        case "Enter":
                            ApplyImageModification(_imageList[i]._image, _imageList[i]._enterColor, _imageList[i]._enterSprite);
                            break;
                        case "Exit":
                            ApplyImageModification(_imageList[i]._image, _imageList[i]._exitColor, _imageList[i]._exitSprite);
                            break;
                        case "Click":
                            ApplyImageModification(_imageList[i]._image, _imageList[i]._clickColor, _imageList[i]._clickSprite);
                            AudioManager.Instance.PlaySFX("Button");
                            break;
                    }
                }
            }
        }
    }
    private void ApplyImageModification(Image image, List<Color> colors, Sprite sprite)
    {
        Multicolor multicolorImage = image.GetComponent<Multicolor>();
        if (colors != null && colors.Count > 0)
        {
            multicolorImage.ResetColorIndex();
            multicolorImage._colors = colors;
        }
        if (sprite != null)
        {
            image.sprite = sprite;
        }
    }
#endregion

    #region TextModification
    private void ModifyTexts(string interactionType)
    {
        if (_textList != null)
        {
            for (int t = 0; t < _textList.Length; t++)
            {
                if (_textList[t] != null)
                {
                    switch (interactionType)
                    {
                        case "Enter":
                            ApplyTextModification(_textList[t]._text, _textList[t]._enterColor);
                            break;
                        case "Exit":
                            ApplyTextModification(_textList[t]._text, _textList[t]._exitColor);
                            break;
                        case "Click":
                            ApplyTextModification(_textList[t]._text, _textList[t]._clickColor);
                            break;
                    }
                }
            }
        }
    }

    private void ApplyTextModification(TMP_Text text, List<Color> colors)
    {
        Multicolor multicolorText = text.GetComponent<Multicolor>();
        if (colors != null && colors.Count > 0)
        {
            multicolorText.ResetColorIndex();
            multicolorText._colors = colors;
        }
    }
    #endregion
}