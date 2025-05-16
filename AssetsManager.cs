using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
    using UnityEditor;
#endif


public class AssetsManager : Singleton<AssetsManager>
{
    [SerializeField] private string _scenesPath;
    [SerializeField] private List<string> _scenesNames = new();

    private string _choosenSceneName;

    protected override void Awake()
    {
        if (!Directory.Exists(_scenesPath))
        {
            Debug.LogError($"Directory not found: <{_scenesPath}>");
            return;
        }

        foreach (string path in Directory.GetFiles(_scenesPath, "*.json"))
        {
            _scenesNames.Add(Path.GetFileNameWithoutExtension(path));
        }
        Debug.Log($"Scenes Path: <{_scenesPath}>");
        Debug.Log("Scenes names: " + string.Join(", ", _scenesNames));

        base.Awake();
    }

    public void ChooseScene(string sceneName) => _choosenSceneName = sceneName;

    public string GetChoosenScene() => _choosenSceneName;

    public bool SceneExists(string name) => _scenesNames.Contains(name);

    public bool CreateScene(string name, List<SceneData.Marker> markers, string backgroundImagePath)
    {
        if (SceneExists(name)) {  return false; }

        SceneData sceneData = new();
        sceneData.SetMarkers(markers);
        sceneData.backgroundImagePath = backgroundImagePath;
        File.WriteAllText(Path.Combine(_scenesPath, name + ".json"), sceneData.ToJson());
        _scenesNames.Add(name);

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif

        return true;
    }

    public SceneData GetScene(string name)
    {
        try
        {
            string json = File.ReadAllText(Path.Combine(_scenesPath, name + ".json"));
            return SceneData.FromJson(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load scene {name}: {ex.Message}");
            return null;
        }
    }

    public List<string> GetScenesNames() { return _scenesNames; }

    public bool EditScene(string name, string newName, List<SceneData.Marker> markers, string backgroundImagePath)
    {
        if (!SceneExists(name) || (name != newName && SceneExists(newName))) return false;

        SceneData sceneData = SceneData.FromJson(File.ReadAllText(Path.Combine(_scenesPath, name + ".json")));
        bool isRenaming = name != newName;

        if (isRenaming) DeleteScene(name);

        sceneData.SetMarkers(markers);
        sceneData.backgroundImagePath = backgroundImagePath;
        File.WriteAllText(Path.Combine(_scenesPath, newName + ".json"), sceneData.ToJson());

        if (isRenaming) _scenesNames.Add(newName);

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif

        return true;
    }

    public bool DeleteScene(string name)
    {
        if (!SceneExists(name)) return false;

        File.Delete(Path.Combine(_scenesPath, name + ".json"));
        _scenesNames.Remove(name);

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif

        return true;
    }
}
