using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class EditorScene : ChangeLayer
{
    public string GetName()
    {
        return GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public override void OnChange()
    {
        base.OnChange();

        Creator creator = _nextLayer.GetComponent<Creator>();
        creator.OpenScene(this);
    }
}
