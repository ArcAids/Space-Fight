using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusters : MonoBehaviour
{
    [SerializeField]
    float acceleration;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float tiltFactor=10;

    Rigidbody2D rigid;
    IControlPlayer input;
    float currentVelocity;
    float targetVelocity;
    private void Start()
    {
        input = new PlayerInput();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.ReadInput();
        if (input.Horizontal != 0)
            targetVelocity = input.Horizontal * maxSpeed;
        else targetVelocity = 0;
        currentVelocity = Mathf.Lerp(currentVelocity,targetVelocity,Time.deltaTime *acceleration);
        rigid.velocity=(transform.right*currentVelocity);

        if(transform.position.x<-3 ||transform.position.x>3)
        {
            currentVelocity = 0;
            transform.position = new Vector2(Mathf.Clamp(transform.position.x,-3,3),transform.position.y);
        }

        transform.rotation = Quaternion.Euler(0, tiltFactor*currentVelocity ,transform.rotation.eulerAngles.z);

    }
}