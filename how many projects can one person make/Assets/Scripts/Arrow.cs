using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviour
{
    Rigidbody arrowbody;
    private float lifeTime = 5f;
    private float timer;
    public GameObject trail;
    private bool hitSomething = false;
    private Transform anchor;

    Vector3 velocity;

    private PhotonView PV;
    private AvatarSetup avatarSetup;

    public float ArrowDamage = 33.35f;
    public float knockbackPower = 20;

    // Start is called before the first frame update
    void Start()
    {
        arrowbody = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitSomething)
        {
            if(this.anchor != null)
            {
                this.transform.position = anchor.transform.position;
                this.transform.rotation = anchor.transform.rotation;
            }
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                DestroyArrow();
            }
        }
        ApplyArrowGravity();
    }

    private void OnCollisionEnter(Collision collision)
    {
            if (collision.collider.tag != "Arrow")
            {
                Stick();
                this.transform.position = collision.contacts[0].point;
                GameObject anchor = new GameObject("Arrow_Anchor");
                anchor.transform.position = this.transform.position;
                anchor.transform.rotation = this.transform.rotation;
                anchor.transform.parent = collision.transform;
                this.anchor = anchor.transform;

                hitSomething = true;
            }

            if (collision.collider.tag == "Avatar")
            {
                AvatarSetup pd = collision.gameObject.GetComponent<AvatarSetup>();
                pd.TakeDamage(ArrowDamage);

                Debug.Log("KnockBack");
                Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
                Vector3 direction = collision.transform.position - this.transform.position;
                direction.y = 0;

                rb.AddForce(direction.normalized * knockbackPower, ForceMode.Impulse);
            }
    }

    private void Stick()
    {
        arrowbody.velocity = Vector3.zero;
        arrowbody.useGravity = false;
        trail.SetActive(false);
        arrowbody.detectCollisions = false;
    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }

    private void ApplyArrowGravity()
    {
        float _yVelocity = arrowbody.velocity.y;
        float _zVelocity = arrowbody.velocity.z;
        float _xVelocity = arrowbody.velocity.x;
        float _combinedVelocity = Mathf.Sqrt(_xVelocity * _xVelocity + _zVelocity * _zVelocity);
        float _fallAngle = -1*Mathf.Atan2(_yVelocity, _combinedVelocity) * 180 / Mathf.PI;

        transform.eulerAngles = new Vector3(_fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

}
