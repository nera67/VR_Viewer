using UnityEngine;

public class SaveEditsButton : ChangeLayer
{
    [SerializeField] private Creator _creator;

    public override void OnChange()
    {
        if (_creator.SaveOpenedScene()) { base.OnChange(); }
    }
}
