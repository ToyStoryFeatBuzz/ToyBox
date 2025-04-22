using TMPro;
using UnityEngine;

public class GraphElement : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _LengthLine = 0.5f;
    [SerializeField] private float _WidthLine = 0.1f;
    
    public void Init(int value, Vector2 offSetText, Vector2 lineDir)
    {
        _line.startWidth = _WidthLine;
        _line.positionCount = 2;
        _text.transform.localPosition = offSetText;
        _text.text = value.ToString();

        var v = new Vector3[] { transform.position - (Vector3)(lineDir * _LengthLine), transform.position + (Vector3)(lineDir * _LengthLine) };
        
        _line.SetPositions(v);
    }
}
