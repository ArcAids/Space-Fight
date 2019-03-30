using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BarrierManager : MonoBehaviour
{
    [SerializeField]
    float minDelay=1;
    [SerializeField]
    float maxDelay=4;
    PhotonView view;
    Barrier[] barriers;
    List<Barrier> DeactivedBarriers;
    bool gameRunning=false;

    // Start is called before the first frame update
    void Start()
    {
        view=GetComponent<PhotonView>();
        if (!PhotonNetwork.InRoom)
            view = null;
        barriers= GetComponentsInChildren<Barrier>();
        DeactivedBarriers = new List<Barrier>();
        int i = 0;
        foreach (var barrier in barriers)
        {
            barrier.id = i;
            barrier.RegisterDestroyedCallback(OnDestroyed);
            barrier.gameObject.SetActive(false);
            DeactivedBarriers.Add(barrier);
            i++;
        }
        gameRunning = true;
        StartCoroutine(SpawnRandomBarrier());
    }

    IEnumerator SpawnRandomBarrier()
    {
        float spawnDelay;
        while (gameRunning)
        {
            spawnDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(spawnDelay);

            if (DeactivedBarriers.Count - 1 <= 0)
                continue;

            int randomId=DeactivedBarriers[Random.Range(0,DeactivedBarriers.Count)].id;
            if (view != null)
                view.RPC("EnableThisEveryWhere", RpcTarget.AllViaServer, randomId);
            else
                EnableThisEveryWhere(randomId);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnDestroyed(int id)
    {
        if (view != null)
            view.RPC("DisableThisEveryWhere", RpcTarget.AllViaServer, id);
        else
            DisableThisEveryWhere(id);
    }

    [PunRPC]
    void DisableThisEveryWhere(int id)
    {
        barriers[id].gameObject.SetActive(false);
        DeactivedBarriers.Add(barriers[id]);
    }

    [PunRPC]
    void EnableThisEveryWhere(int id)
    {
        barriers[id].gameObject.SetActive(true);
        DeactivedBarriers.Remove(barriers[id]);
    }

}
