using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    [SerializeField] protected GameObject _currentLayer;
    [SerializeField] protected GameObject _nextLayer;

    public virtual void OnChange() {
        _currentLayer.SetActive(false);
        _nextLayer.SetActive(true);
    }

    public void SetCurrentLayer(GameObject layer)
    {
        _currentLayer = layer;
    }

    public void SetNextLayer(GameObject layer)
    {
        _nextLayer = layer;
    }
    public GameObject GetCurrentLayer()
    {
        return _currentLayer;
    }

    public GameObject GetNextLayer()
    {
        return _nextLayer;
    }
}
