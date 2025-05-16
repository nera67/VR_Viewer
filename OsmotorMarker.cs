using UnityEngine;

public class OsmotorMarker : ChangeLayer
{
    private string _path;

    public void SetPath(string path) => _path = path;

    public override void OnChange()
    {
        if (!string.IsNullOrEmpty(_path))
        {
            SkyboxLoader.Instance.LoadSkybox(_path);
            base.OnChange();
        }
    }
}
