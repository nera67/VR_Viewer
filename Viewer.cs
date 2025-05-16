using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _scenePrefab;

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
        GameObject scene = Instantiate(_scenePrefab, _content);
        scene.GetComponentInChildren<TextMeshProUGUI>().text = name;

        if (!String.IsNullOrEmpty(backgroundImagePath))
        {
            try {
                Texture2D texture = new(2, 2);
                texture.LoadImage(File.ReadAllBytes(backgroundImagePath));
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                scene.GetComponentInChildren<Image>().sprite = sprite; 
            } catch { Debug.Log($"Could not found image with path {backgroundImagePath}"); }
        }
    }
}
