using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlotAnimator : MonoBehaviour
{
    public float animationDuration = 3f;

    private List<RectTransform> slots = new List<RectTransform>();
    private Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();
    private LayoutGroup layoutGroup;

    private void Awake()
    {
        layoutGroup = GetComponent<LayoutGroup>();
    }

    public void AnimateReorder(List<Transform> newOrder)
    {
        slots.Clear();
        originalPositions.Clear();

        foreach (Transform child in transform)
        {
            if (child is RectTransform rect)
            {
                slots.Add(rect);
                originalPositions[rect] = rect.anchoredPosition;
            }
        }
        
        for (int i = 0; i < newOrder.Count; i++)
        {
            newOrder[i].SetSiblingIndex(i);
        }

        StartCoroutine(DelayAndAnimate());
    }

    IEnumerator DelayAndAnimate()
    {
        yield return null;

        if (layoutGroup != null)
            layoutGroup.enabled = false;
        
        Dictionary<RectTransform, Vector2> newPositions = new Dictionary<RectTransform, Vector2>();
        foreach (RectTransform slot in slots)
        {
            newPositions[slot] = slot.anchoredPosition;
            slot.anchoredPosition = originalPositions[slot]; 
        }
        
        foreach (RectTransform slot in slots)
        {
            StartCoroutine(AnimateMove(slot, originalPositions[slot], newPositions[slot]));
        }
    }

    IEnumerator AnimateMove(RectTransform target, Vector2 startPos, Vector2 endPos)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = endPos;
    }
}
