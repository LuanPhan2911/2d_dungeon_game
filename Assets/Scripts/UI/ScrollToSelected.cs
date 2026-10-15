using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollToSelected : MonoBehaviour
{
    private ScrollRect scrollRect;
    private RectTransform viewportRect;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        viewportRect = scrollRect.viewport;
    }

    void Update()
    {
        // Get the currently selected GameObject from Unity's Event System
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null) return;

        // Verify the selected object is actually a child of our scroll content
        if (!selected.transform.IsChildOf(scrollRect.content)) return;

        RectTransform selectedRect = selected.GetComponent<RectTransform>();
        if (selectedRect == null) return;

        // Scroll to the selected item
        UpdateScrollPosition(selectedRect);
    }

    void UpdateScrollPosition(RectTransform target)
    {
        // Convert target position into the local space of the content panel
        Vector3 targetLocalPos = scrollRect.content.InverseTransformPoint(target.position);

        // Calculate the total scrollable height
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = viewportRect.rect.height;
        float scrollableHeight = contentHeight - viewportHeight;

        if (scrollableHeight <= 0) return;

        // Calculate the target Y position relative to the top of the content
        // (Assumes Content pivot Y is 1)
        float targetY = -targetLocalPos.y - (viewportHeight / 2f);

        // Clamp the position so it doesn't scroll past the bounds
        float normalizedY = 1f - Mathf.Clamp(targetY / scrollableHeight, 0f, 1f);

        // Instantly snap to the position (or use Mathf.Lerp for a smooth scroll)
        scrollRect.verticalNormalizedPosition = normalizedY;
    }
}
