using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IImagePathSetter
{
    void SetImagePath(string path);
}

public class Item : MonoBehaviour
{
    private bool _isFolder;
    private string _path;
    private FileBrowser _browser;
    private IImagePathSetter _marker;

    public void Configure(FileBrowser browser, string name, string path, bool isFolder, Sprite sprite, IImagePathSetter marker)
    {
        _browser = browser;
        GetComponentInChildren<TextMeshProUGUI>().text = name;
        _path = path;
        _isFolder = isFolder;
        GetComponentInChildren<Image>().sprite = sprite;
        _marker = marker;
    }

    public void OnClick()
    {
        if (_isFolder)
        {
            _browser.Go(_path);
        } else {
            _marker.SetImagePath(_path);
            ChangeLayer changeLayer = _browser.GetComponent<ChangeLayer>();
            changeLayer.OnChange();
        }
    }
}
