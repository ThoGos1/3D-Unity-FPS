using UnityEngine;
using Photon.Pun;

public class BowPullShoot : MonoBehaviourPun
{
    public GameObject bow;
    private PhotonView PV;


    public float projectileSpeed = 100f;

    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private GameObject arrow = null;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        bow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (PV.IsMine)
        {
            TakeInput();
        }
    }

    private void TakeInput()
    {
        if (Input.GetMouseButtonDown(1) && PV.IsMine)
        {
            bow.SetActive(true);
            PV.RPC("FireProjectile", RpcTarget.All);
        }
        if (Input.GetMouseButtonUp(1) && PV.IsMine)
        {
            PV.RPC("RemoveBow", RpcTarget.All);
        }
        

    }

    [PunRPC]
    private void FireProjectile()
    { 
        
        bow.SetActive(true);

        var arrowInstance = Instantiate(arrow, spawnPoint.position, spawnPoint.rotation);

        arrowInstance.GetComponent<Rigidbody>().velocity = arrowInstance.transform.forward * projectileSpeed;
        
    }

    [PunRPC]
    private void RemoveBow()
    {
        bow.SetActive(false);
    }
}
