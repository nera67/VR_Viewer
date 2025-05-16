using UnityEngine;
using System.IO;

public class SkyboxLoader : Singleton<SkyboxLoader>
{
    [SerializeField] private Material _skyboxMaterial;

    public void LoadSkybox(string relativePath)
    {
        try
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            Texture2D texture = LoadTexture(fullPath);
            ConfigureMaterial(texture);
            ApplySkybox();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка загрузки: {e.GetType().Name} - {e.Message}");
        }
    }

    private Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Файл не найден: {path}");
        }

        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new(2, 2);

        if (!texture.LoadImage(fileData))
        {
            throw new UnityException("Ошибка декодирования текстуры");
        }

        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    private void ConfigureMaterial(Texture2D texture)
    {
        if (_skyboxMaterial.shader.name != "Skybox/Panoramic")
        {
            _skyboxMaterial.shader = Shader.Find("Skybox/Panoramic");
        }

        _skyboxMaterial.SetTexture("_MainTex", texture);
        _skyboxMaterial.SetFloat("_Mapping", 1);
        _skyboxMaterial.SetFloat("_ImageType", 0);
    }

    private void ApplySkybox()
    {
        RenderSettings.skybox = _skyboxMaterial;
        DynamicGI.UpdateEnvironment();

        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        }
    }
}