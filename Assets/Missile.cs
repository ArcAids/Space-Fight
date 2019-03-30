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

    bool isActive;
    bool isMine;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    public void Initialize(float xPosition, float  lag, bool isMine)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.position = new Vector2(xPosition, rigidbody.position.y);
        rigidbody.velocity = transform.up * speed;
        rigidbody.position += rigidbody.velocity * lag;
        this.isMine = isMine;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
            return;
        ITakeDamage target=collision.GetComponent<ITakeDamage>();
        if (target!=null)
        {
            //explode 
            if (isMine)
            {
                target.TakeDamage(damage);
            }
                isActive = false;
                gameObject.SetActive(false);
                Destroy(gameObject);
        }
    }
}

internal interface ITakeDamage
{
    void TakeDamage(float damage);
}