using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class EditorViewer : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _scenePrefab;
    [SerializeField] private GameObject _creator;

    private void Awake()
    {
        AssetsManager manager = AssetsManager.Instance;
        foreach (string name in manager.GetScenesNames())
        {
            SceneData data = manager.GetScene(name);
            AddScene(name, data.backgroundImagePath);
        }
    }

    public void AddScene(string name, string backgroundImagePath)
    {
        EditorScene scene = Instantiate(_scenePrefab, _content).GetComponent<EditorScene>();
        scene.GetComponentInChildren<TextMeshProUGUI>().text = name;
        scene.SetCurrentLayer(gameObject);
        scene.SetNextLayer(_creator);

        if (!String.IsNullOrEmpty(backgroundImagePath))
        {
            Texture2D texture = new(2, 2);
            texture.LoadImage(File.ReadAllBytes(backgroundImagePath));
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            scene.GetComponentInChildren<Image>().sprite = sprite;
        }
    }
}
