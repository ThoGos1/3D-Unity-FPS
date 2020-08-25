using Photon.Pun;
using UnityEngine;

public class FFA : MonoBehaviour
{
    public GameObject[] spawnPoint;
    [SerializeField] private GameObject playerPrefab = null;

    private void Start()
    {
        int spawnId = Random.Range(0, spawnPoint.Length);
        var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint[spawnId].transform.position, spawnPoint[spawnId].transform.rotation, 0);
    }

    public void Update()
    {

    }

    public void StartCommitNotDie()
    {
            int spawnId = Random.Range(0, spawnPoint.Length);
            var player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint[spawnId].transform.position, spawnPoint[spawnId].transform.rotation, 0);
    }
}
