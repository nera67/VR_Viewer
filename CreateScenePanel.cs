using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CreateScenePanel : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private RectTransform _markerContainer;
    [SerializeField] private GameObject _markerCurrentLayer;
    [SerializeField] private FileBrowser _fileBrowser;

    [SerializeField] private XRRayInteractor _leftControllerInteractor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }

        HandleVRInput(_leftControllerInteractor);
    }

    private void HandleVRInput(XRRayInteractor interactor)
    {
        bool hasHit = interactor.TryGetCurrentRaycast(
            out RaycastHit? raycastHit,
            out _,
            out RaycastResult? uiRaycastResult,
            out _,
            out bool isUIHitClosest);

        if (!hasHit) return;

        Vector2 screenPosition = isUIHitClosest && uiRaycastResult.HasValue
            ? uiRaycastResult.Value.screenPosition
            : Camera.main.WorldToScreenPoint(raycastHit.Value.point);

        HandleInput(screenPosition);
    }

    private void HandleInput(Vector2 inputPosition)
    {
        print(IsPointerOverUIElement(inputPosition));
        if (IsPointerOverUIElement(inputPosition)) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _markerContainer,
            inputPosition,
            null,
            out Vector2 localPos) && _markerContainer.rect.Contains(localPos))
        {
            CreateMarker(localPos);
        }
    }

    private bool IsPointerOverUIElement(Vector2 position)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = position
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Marker>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearMarkerContainer()
    {
        foreach (Marker marker in _markerContainer.GetComponentsInChildren<Marker>())
        {
            Destroy(marker.gameObject);
        }
    }

    public Marker[] GetMarkers()
    {
        return _markerContainer.GetComponentsInChildren<Marker>();
    }

    public GameObject CreateMarker(Vector2 localPos)
    {
        GameObject markerObject = Instantiate(_markerPrefab, _markerContainer);

        ChangeLayer changeLayerComponent = markerObject.GetComponent<ChangeLayer>();
        changeLayerComponent.SetCurrentLayer(_markerCurrentLayer);
        changeLayerComponent.SetNextLayer(_fileBrowser.gameObject);

        Marker markerComponent = markerObject.GetComponent<Marker>();
        markerComponent.SetFileBrowser(_fileBrowser);
        RectTransform rt = markerObject.GetComponent<RectTransform>();
        rt.anchoredPosition = localPos;

        return markerObject;
    }
}
