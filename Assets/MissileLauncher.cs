using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    float fireRate;

    IControlShooting input;
    float nextShotTime;

    private void Start()
    {
        input = new PlayerInput();
        nextShotTime = Time.time;
    }

    private void Update()
    {
        input.ReadInput();
        if (Time.time > nextShotTime && input.Fire)
        {
            nextShotTime = Time.time + 1/fireRate;
            Instantiate(bullet, transform.position, transform.rotation);
        }

    }
}
