using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerLogic playerLogic))
        {
            Debug.Log("HERERERERE");
            playerLogic.ResetPosition();
        }

        // if (other.gameObject.CompareTag("Player"))
        // {
        //     Debug.Log("Player");
        // }

    }
}