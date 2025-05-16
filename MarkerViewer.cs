using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MarkerViewer : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _markerContainer;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _nextLayer;

    private void Awake()
    {
        AssetsManager manager = AssetsManager.Instance;
        SceneData data = manager.GetScene(manager.GetChoosenScene());

        foreach (SceneData.Marker markerData in data.markers)
        {
            OsmotorMarker marker = Instantiate(_markerPrefab, _markerContainer).GetComponent<OsmotorMarker>();
            Vector2 position = new(markerData.x * _markerContainer.rect.width, markerData.y * _markerContainer.rect.height);
            marker.transform.localPosition = position;
            marker.SetCurrentLayer(gameObject);
            marker.SetNextLayer(_nextLayer);
            marker.SetPath(markerData.imagePath);
        }

        if (!String.IsNullOrEmpty(data.backgroundImagePath))
        {
            try
            {
                Texture2D texture = new(2, 2);
                texture.LoadImage(File.ReadAllBytes(data.backgroundImagePath));
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                _backgroundImage.sprite = sprite;
            }
            catch { Debug.Log($"Could not found image with path {data.backgroundImagePath}"); }
        }
    }
}
