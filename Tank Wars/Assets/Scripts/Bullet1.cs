using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 50f;
    public float damage;
    public Vector3 dir;
    public bool hostile;
    public GameObject destroyeff;
    AudioManager am;
    
    public bool bounce = true;

    private void OnDestroy()
    {
        Instantiate(destroyeff, transform.position, transform.rotation);
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Enemy") && !hostile)
        {
            collision.gameObject.GetComponent<TankMovement>().damage(damage);
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Player") && hostile)
        {
            collision.gameObject.GetComponent<MoveByTouch>().damage(damage);
            Destroy(gameObject);
        }

        else
            Destroy(gameObject);
       
        
    }
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
