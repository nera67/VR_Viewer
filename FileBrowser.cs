using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;

public class FileBrowser : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private TMP_InputField _pathInput;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Sprite _directorySprite;
    [SerializeField] private string _path;
    [SerializeField] private List<string> _extensionsFilter = new();

    private IImagePathSetter _imagePathSetter;

    private void DropPath()
    {
        if (string.IsNullOrEmpty(_path))
        {
            _path = Directory.GetCurrentDirectory();
        }
    }

    private Texture2D LoadTexture(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new(2, 2);

        if (texture.LoadImage(fileData)) return texture;
        return null;
    }

    private Sprite CreateSprite(Texture2D texture)
    {
        return texture != null ?
            Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                Vector2.one * 0.5f
            ) :
            null;
    }

    private bool ShouldIgnoreEntry(string name) =>
        string.IsNullOrEmpty(name) || name.StartsWith(".");

    private bool IsExtensionAllowed(string extension) =>
        _extensionsFilter.Count == 0 ||
        _extensionsFilter.Contains(extension.ToLowerInvariant());

    private void CreateItem(string name, string path, bool isFolder, Sprite sprite)
    {
        GameObject item = Instantiate(_itemPrefab, _content);
        item.GetComponent<Item>().Configure(this, name, path, isFolder, sprite, _imagePathSetter);
    }

    private void LoadDirectories()
    {
        foreach (var dirPath in Directory.GetDirectories(_path))
        {
            var dirName = Path.GetFileName(dirPath);
            if (ShouldIgnoreEntry(dirName)) continue;

            CreateItem(dirName, dirPath, true, _directorySprite);
        }
    }

    private void LoadFiles()
    {
        foreach (var filePath in Directory.GetFiles(_path))
        {
            var fileName = Path.GetFileName(filePath);
            var extension = Path.GetExtension(filePath);

            if (ShouldIgnoreEntry(fileName) || !IsExtensionAllowed(extension)) continue;

            CreateItem(fileName, filePath, false, CreateSprite(LoadTexture(filePath)));
        }
    }

    public void SetImagePathSetter(IImagePathSetter value) => _imagePathSetter = value;

    public void Go(string path) 
    { 
        _path = path;
        Browse();
    }

    public void Back()
    {
        string newPath = Directory.GetParent(_path).FullName;
        if (Directory.Exists(newPath)) Go(newPath);
    }

    public void Browse()
    {
        if (!Directory.Exists(_path))
        {
            Debug.LogError($"Directory not found: {_path}");
            DropPath();
        }

        foreach (RectTransform item in _content)
        {
            Destroy(item.gameObject);
        }
        _pathInput.text = _path;

        try
        {
            LoadDirectories();
            LoadFiles();
        }
        catch (Exception e)
        {
            Debug.LogError($"Browse error: {e.Message}");
        }
    }
}
