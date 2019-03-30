using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    Transform muzzle;
    [SerializeField]
    Missile bullet;

    [SerializeField]
    float fireRate;

    PhotonView view;
    IControlShooting input;
    float nextShotTime;

    private void Start()
    {
        view=GetComponent<PhotonView>();
        if (!PhotonNetwork.InRoom)
            view = null;
        input = new PlayerInput();
        nextShotTime = Time.time;
    }

    private void Update()
    {
        if(view!=null && !view.IsMine)
            return;

        input.ReadInput();
        if (Time.time > nextShotTime && input.Fire)
        {
            nextShotTime = Time.time + 1 / fireRate;
            if (view != null)
                view.RPC("Shoot", RpcTarget.All, transform.position.x);
            else
                Shoot(transform.position.x);
        }
    }

    [PunRPC]
    void Shoot(float xPosition, PhotonMessageInfo info)
    {
        float lag;
        lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        Missile missile =Instantiate(bullet, muzzle.position, muzzle.rotation);
        missile.Initialize(xPosition,lag,view.IsMine);
    }

    void Shoot(float xPosition)
    {
        Missile missile = Instantiate(bullet, muzzle.position, muzzle.rotation);
        missile.Initialize(xPosition, 0,true);
    }

}
