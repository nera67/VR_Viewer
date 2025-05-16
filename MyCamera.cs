using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public GameObject screenObject;

    void Start()
    {
        screenObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            screenObject.SetActive(!screenObject.activeSelf);
        }
    }
}
