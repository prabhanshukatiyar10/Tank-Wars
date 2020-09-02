using UnityEngine.AI;
using UnityEngine;

public class chasebullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public GameObject destroyeff;
    public bool hostile;
    public float damage;
    public NavMeshAgent agent;

    private void OnDestroy()
    {
        Instantiate(destroyeff, transform.position, transform.rotation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy") && !hostile)
        {
            collision.gameObject.GetComponent<TankMovement>().damage(damage);
            Destroy(gameObject);
        }
        else if(collision.collider.CompareTag("Player") && hostile)
        {
            collision.gameObject.GetComponent<MoveByTouch>().damage(damage);
            Destroy(gameObject);
        }
    }

    void settarget()
    {
        Vector3 point = target.position;

            agent.SetDestination(point);
    }

    void Start()
    {
        InvokeRepeating("settarget", 0f, 1f);
        Destroy(gameObject, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            Destroy(gameObject);
        
    }
}
