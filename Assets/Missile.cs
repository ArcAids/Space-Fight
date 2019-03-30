using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody=GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ITakeDamage target=collision.GetComponent<ITakeDamage>();
        if (target!=null)
        {
            //explode
            gameObject.SetActive(false);
            target.TakeDamage(damage);
        }
    }
}

internal interface ITakeDamage
{
    void TakeDamage(float damage);
}