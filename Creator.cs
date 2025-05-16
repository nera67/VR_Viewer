using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using static SimpleFileBrowser.FileBrowser;
using System.IO;
using UnityEngine.UI;

public class Creator : MonoBehaviour, IImagePathSetter
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private EditorViewer _viewer;
    [SerializeField] private CreateScenePanel _panel;
    [SerializeField] private RectTransform _markerContainer;
    [SerializeField] private FileBrowser _fileBrowser;
    [SerializeField] private Image _backgroundImage;

    private EditorScene _openedScene;
    private string _backgroundImagePath;

    public void OpenScene(EditorScene scene)
    {
        _openedScene = scene;
        _inputField.text = scene.GetName();

        SceneData data = AssetsManager.Instance.GetScene(scene.GetName());
        foreach (SceneData.Marker markerData in data.markers)
        {
            Vector2 position = new(markerData.x * _markerContainer.rect.width, markerData.y * _markerContainer.rect.height);
            GameObject markerObject = _panel.CreateMarker(position);
            markerObject.GetComponent<Marker>().SetImagePath(markerData.imagePath);
        }
        _backgroundImagePath = data.backgroundImagePath;

        if (!String.IsNullOrEmpty(_backgroundImagePath))
        {
            Texture2D texture = new(2, 2);
            texture.LoadImage(File.ReadAllBytes(_backgroundImagePath));
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            _backgroundImage.sprite = sprite;
        }
    }

    public bool SaveOpenedScene()
    {
        if (string.IsNullOrEmpty(_inputField.text)) return false;

        bool isSuccess;
        AssetsManager manager = AssetsManager.Instance;
        List<SceneData.Marker> markersData = new();

        foreach (Marker marker in _panel.GetMarkers())
        {
            Vector2 position = marker.GetComponent<RectTransform>().anchoredPosition;
            markersData.Add(new SceneData.Marker(position.x / _markerContainer.rect.width, position.y / _markerContainer.rect.height, marker.GetImagePath()));
        }

        if (_openedScene == null)
        {
            isSuccess = manager.CreateScene(_inputField.text, markersData, _backgroundImagePath);
            Debug.Log("CreateScene: " + _openedScene);
        }
        else
        {
            isSuccess = manager.EditScene(_openedScene.GetName(), _inputField.text, markersData, _backgroundImagePath);
            Debug.Log("EditScene: " + _openedScene);
        }

        if (isSuccess)
        {
            if (_openedScene == null) { _viewer.AddScene(_inputField.text, _backgroundImagePath); }
            else 
            { 
                _openedScene.GetComponentInChildren<TextMeshProUGUI>().text = _inputField.text;
                if (!String.IsNullOrEmpty(_backgroundImagePath))
                {
                    Texture2D texture = new(2, 2);
                    texture.LoadImage(File.ReadAllBytes(_backgroundImagePath));
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    _openedScene.GetComponentInChildren<Image>().sprite = sprite;
                }
            }
            _inputField.text = "";
        }

        _openedScene = null;
        _panel.ClearMarkerContainer();
        _backgroundImage.sprite = null;
        return isSuccess;
    }

    public void DeleteOpenedScene()
    {
        if (_openedScene != null)
        {
            AssetsManager.Instance.DeleteScene(_openedScene.GetName());
            Destroy(_openedScene.gameObject);
        }
        _inputField.text = "";
        _panel.ClearMarkerContainer();
        _openedScene = null;
        _backgroundImage.sprite = null;
    }

    public void SetBackgoundImage()
    {
        _fileBrowser.SetImagePathSetter(this);
    }

    public void UndoAllEdits()
    {
        _inputField.text = "";
        _panel.ClearMarkerContainer();
        _openedScene = null;
        _backgroundImage.sprite = null;
    }

    public void SetImagePath(string value) {
        _backgroundImagePath = value;

        if (!String.IsNullOrEmpty(_backgroundImagePath))
        {
            Texture2D texture = new(2, 2);
            texture.LoadImage(File.ReadAllBytes(_backgroundImagePath));
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            _backgroundImage.sprite = sprite;
        }
    }
}
