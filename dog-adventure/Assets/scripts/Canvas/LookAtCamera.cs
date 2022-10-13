using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
    public Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        LookingAtCamera();
    }

    // Update is called once per frame
    void Update()
    {
        LookingAtCamera();
    }


    void LookingAtCamera()
    {
        transform.LookAt(mainCamera.transform);
    }
}
