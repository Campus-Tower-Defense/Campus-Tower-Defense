using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(InputData))]
public class DisplayInputData : MonoBehaviour
{

    private InputData _inputData;
    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            Debug.Log("LeftYippie");
        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            Debug.Log("RighttYippie");
        }
    }
}
