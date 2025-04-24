using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlotAnimator : MonoBehaviour
{
    public float animationDuration = 0.5f;

    private List<RectTransform> slots = new List<RectTransform>();
    private Dictionary<RectTransform, Vector3> originalPositions = new Dictionary<RectTransform, Vector3>();

    public void AnimateReorder(List<Transform> newOrder)
    {
        slots.Clear();
        originalPositions.Clear();

        foreach (Transform child in transform)
        {
            RectTransform rect = child as RectTransform;
            slots.Add(rect);
            originalPositions[rect] = rect.position;
        }

        // Réordonner dans la hiérarchie
        for (int i = 0; i < newOrder.Count; i++)
        {
            newOrder[i].SetSiblingIndex(i);
        }

        // Délai d'un frame pour que le LayoutGroup repositionne les éléments
        StartCoroutine(DelayAndAnimate());
    }

    IEnumerator DelayAndAnimate()
    {
        yield return null; // Attendre une frame

        foreach (RectTransform slot in slots)
        {
            Vector3 startPos = originalPositions[slot];
            Vector3 endPos = slot.position;
            StartCoroutine(AnimateMove(slot, startPos, endPos));
        }
    }

    IEnumerator AnimateMove(RectTransform target, Vector3 startPos, Vector3 endPos)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            target.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = endPos;
    }
}