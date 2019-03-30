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
        StartCoroutine(DeactivateAfterTime(timer));
    }

    IEnumerator DeactivateAfterTime(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (deactivateInstead)
            Deactivate();
        else
            Destroy(gameObject);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
