using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroyCounter : MonoBehaviour
{
    [SerializeField]
    float timer;
    [SerializeField]
    bool deactivateInstead=false;

    private void OnEnable()
    {
        if (deactivateInstead)
            Invoke("Deactivate",timer);
        else
            Destroy(gameObject, timer);
    }

    [PunRPC]
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
