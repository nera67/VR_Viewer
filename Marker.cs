using System.IO;
using UnityEngine;

public class Marker : MonoBehaviour, IImagePathSetter
{
    [SerializeField] private FileBrowser _browser;

    private string _imagePath;

    public void SetFileBrowser(FileBrowser browser) => _browser = browser;

    public void SetImagePath(string value) => _imagePath = value;

    public string GetImagePath() => _imagePath;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RectTransform rect = GetComponent<RectTransform>();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out Vector2 localPos) && rect.rect.Contains(localPos))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(gameObject);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    _browser.SetImagePathSetter(this);
                    if (_imagePath == null) _browser.Browse();
                    else _browser.Go(Directory.GetParent(_imagePath).FullName);
                }
            }
        }
    }
}
