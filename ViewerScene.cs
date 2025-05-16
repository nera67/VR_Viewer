using TMPro;

public class ViewerScene : ChangeScene
{
    public void OnClick()
    {
        string name = GetComponentInChildren<TextMeshProUGUI>().text;
        AssetsManager.Instance.ChooseScene(name);
        Change();
    }
}
