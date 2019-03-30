using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipThrusters : MonoBehaviour, IPunObservable
{
    [SerializeField]
    float acceleration;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float tiltFactor = 10;

    Rigidbody2D rigid;
    PhotonView view;
    IControlPlayer input;
    float currentVelocity;
    float targetVelocity;


    private void OnEnable()
    {
        if (view != null && !view.IsMine)
        {
            GetComponent<Health>().RegisterHealthLostListner(HealthLost);
        }
    }
    private void OnDisable()
    {
        if (view != null && !view.IsMine)
            GetComponent<Health>().DeRegisterHealthLostListner(HealthLost);
    }

    private void Awake()
    {
        input = new PlayerInput();
        rigid = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        if (!PhotonNetwork.InRoom)
        {
            view = null;
        }
    }

    private void Update()
    {
        if ((view != null && view.IsMine) || view == null)
        {
            input.ReadInput();

            if (input.Horizontal != 0)
                targetVelocity = input.Horizontal * maxSpeed;
            else
                targetVelocity = 0;
        }
        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
        rigid.velocity = (transform.right * currentVelocity);

        if (rigid.position.x < -3 || rigid.position.x > 3)
        {
            currentVelocity = 0;
            rigid.position = new Vector2(Mathf.Clamp(rigid.position.x, -3, 3), transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, tiltFactor * rigid.velocity.x, transform.rotation.eulerAngles.z), Time.deltaTime * 20);
    }

    void HealthLost(float health)
    {
        Debug.Log(health);
        if (health <= 0)
        {
            if (view != null)
                view.RPC("DestroySpaceShip", RpcTarget.All);
            else
                DestroySpaceShip();
        }
        Hashtable props = new Hashtable() { { "Lives", health } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    [PunRPC]
    void DestroySpaceShip()
    {
        //Explode
        gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetVelocity);
            //stream.SendNext(rigid.velocity.x);
            stream.SendNext(rigid.position.x);
        }
        else
        {
            targetVelocity = (float)stream.ReceiveNext();
            //currentVelocity = (float)stream.ReceiveNext();
            Vector2 newPos = new Vector2((float)stream.ReceiveNext() + (rigid.velocity.x * (float)(PhotonNetwork.Time - info.SentServerTime)), rigid.position.y);
            rigid.position = Vector2.Lerp(rigid.position,newPos,Time.deltaTime * 2);
        }
    }
}