using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    GameObject mainCamera;
    GameObject subCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        subCamera = GameObject.Find("SubCamera");
        subCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("space"))
        {
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }

        else
        {
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
