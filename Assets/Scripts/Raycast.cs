using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Raycast : MonoBehaviour
{
    [SerializeField] private InputActionReference m_PointUnitRef;

    IMouseDraggable m_NearestDraggable;
    IDropZone m_NeareastDropZone;
    IMouseClickable m_NearestInteractable;
    IMouseHoverable m_NearestHoverrable;
    PointerEventData m_PointerEventDataNoAlloc;
    List<RaycastResult> m_RaycastResults;

    public IMouseDraggable CurrentDraggable { get; private set; }

    private void Awake()
    {
        m_PointerEventDataNoAlloc = new PointerEventData(EventSystem.current);
        m_RaycastResults = new List<RaycastResult>();
    }

    private void Update()
    {
        RaycastAll();
    }

    private void RaycastAll()
    {
        var pointerData = m_PointerEventDataNoAlloc;
        pointerData.position = m_PointUnitRef.action.ReadValue<Vector2>();

        m_RaycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, m_RaycastResults);

        m_NearestDraggable = null;
        m_NeareastDropZone = null;
        m_NearestHoverrable = null;
        m_NearestInteractable = null;

        for (int i = 0; i < m_RaycastResults.Count; i++)
        {
            var result = m_RaycastResults[i];

            if (m_NearestDraggable == null && result.gameObject.TryGetComponent(out IMouseDraggable draggable))
                m_NearestDraggable = draggable;

            // For drop zones, only consider compatible drop zones.
            if (m_NeareastDropZone == null && result.gameObject.TryGetComponent(out IDropZone dropZone))
            {
                if (CurrentDraggable != null)
                {
                    if (dropZone.CanAccept(CurrentDraggable))
                        m_NeareastDropZone = dropZone;
                }
                else
                {
                    m_NeareastDropZone = dropZone;
                }
            }

            if (m_NearestHoverrable == null && result.gameObject.TryGetComponent(out IMouseHoverable hoverrable))
                m_NearestHoverrable = hoverrable;

            if (m_NearestInteractable == null && result.gameObject.TryGetComponent(out IMouseClickable interactable))
                m_NearestInteractable = interactable;

            if (m_NearestDraggable != null &&
                m_NeareastDropZone != null &&
                m_NearestHoverrable != null &&
                m_NearestInteractable != null)
            {
                break;
            }
        }
    }
}