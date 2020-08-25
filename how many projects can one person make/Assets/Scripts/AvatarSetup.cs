using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetup : MonoBehaviour
{

    private PhotonView PV;
    public GameObject[] MyObjects;

    public float playerHealth;
    private float minHealth = 0f;
    private float maxHealth = 100f;
    public float bowDamage; //Delete later

    public bool startTimerForRespawn = false;
    private bool isDead = false;
    public float respawnTimer = 0;
    GameObject deathCam;

    FFA ffa;

    /* Stuff to disable if it does not belong to the client */
    
    public AudioListener localAudioListener;

    // Start is called before the first frame update
    void Start()
    {
        ffa = GameObject.FindObjectOfType<FFA>();
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(localAudioListener);
        }
        if(PV.IsMine)
        {
            foreach(GameObject o in MyObjects)
            {
                o.SetActive(true);
            }
            foreach(GameObject o in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if(o.tag == "deathCam")
                {
                    deathCam = o;
                }
            }
        }
    }

    /*void Awake()
    {
        deathCam.SetActive(false);
    }*/

    public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage" + Mathf.Round(damage * 100) / 100);

        if(PV.IsMine)
        {
            PV.RPC("ApplyDamage", RpcTarget.All, damage);
        }
    }

    void Update()
    {
        if(PV.IsMine)
        {
            if(playerHealth > maxHealth)
            {
                playerHealth = maxHealth;
            }
            if(playerHealth <= minHealth)
            {
                Debug.Log("Commiting Die");

                playerHealth = minHealth;
                PV.RPC("commitDie", RpcTarget.All);
                deathCam.SetActive(true);
            }
            if(isDead)
            {
                ffa.StartCommitNotDie();
                isDead = true;
                playerHealth = maxHealth;
                deathCam.SetActive(false);
            }
        }

    }


    [PunRPC]
    private void ApplyDamage(float damage)
    {
            playerHealth -= damage;
    }


    [PunRPC]
    private void commitDie()
    {
        isDead = true;
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHealth);
            stream.SendNext(isDead);
        }
        else
        {
            playerHealth = (float)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
        }
    }
}