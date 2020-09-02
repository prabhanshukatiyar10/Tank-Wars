using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectilebullet : MonoBehaviour
{
    
    public Vector3 destin;
    public GameObject desteff;
    public float damage;
    Vector3 initdis;
    Rigidbody rb;
    AudioManager am;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.GetComponent<MoveByTouch>().damage(damage);
        Destroy(gameObject);

    }

    private void OnDestroy()
    {
        Instantiate(desteff, transform.position, Quaternion.identity);
        am.play("Bullet Explode");
    }

    Vector3 instvel()
    {
        Vector3 vel = new Vector3();
        Vector3 dir = destin - transform.position;
        float h = dir.y;
        dir.y = 0;

        if (dir.sqrMagnitude > initdis.sqrMagnitude / 2)
            dir.y = 20f;
        else
            dir.y = h;
        
        vel = Mathf.Sqrt(20* Physics.gravity.magnitude) * dir.normalized;
        return vel*2;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        initdis = destin - transform.position;
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Vector3 vel = instvel();
            rb.velocity = vel;
        if (rb.velocity.sqrMagnitude < 100)
        {
            //transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
            //Destroy(gameObject);
        }
            

        
        
            
    }
}
