using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("ENTER");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("EXIT");
    }
}
