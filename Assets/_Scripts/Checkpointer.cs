using System;
using UnityEngine;

public class Checkpointer : MonoBehaviour
{
    public PlayerData PlayerData { get; } = new PlayerData();
    private Vector3 _checkPointPos;

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {

            _checkPointPos = transform.position;
            Debug.Log("SAVE POS: " + _checkPointPos);
        }

        if (Input.GetKey(KeyCode.V))
        {
            
            PlayerData.Velocity = Vector3.zero;
            PlayerData.Origin = _checkPointPos + transform.position;
            // transform.position = _checkPointPos;
            Debug.Log("LOAD POS");
        }
    }
}
