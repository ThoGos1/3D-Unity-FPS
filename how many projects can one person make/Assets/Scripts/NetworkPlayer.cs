using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{

    private PhotonView PV;

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    Quaternion realCamRotation = Quaternion.identity;

    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, realCamRotation, 0.1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
            stream.SendNext(cam.transform.rotation);
        }
        else
        {
            realRotation = (Quaternion)stream.ReceiveNext();
            realPosition = (Vector3)stream.ReceiveNext();
            realCamRotation = (Quaternion)stream.ReceiveNext();
        }

    }
}
