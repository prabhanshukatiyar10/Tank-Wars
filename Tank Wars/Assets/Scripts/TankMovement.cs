using UnityEngine.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform firepoint;
    public Bullet1 b_prefab;
    public float health, healthleft;
    public Transform head, body;
    public chasebullet cb_prefab;
    Vector3 target;
    public projectilebullet pb_prefab;
    public GameObject desteff;
    public bool tripleshot = false;
    public bool heatseeker = false;
    public bool boss = false;
    public bool projectile = false;
    public AudioManager am;
    public GameObject collisioneff;

    private void OnDestroy()
    {
        if (boss)
            am.stop("Electric SF");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(boss)
            if (collision.collider.CompareTag("Player"))
            {
                am.play("Electric SF");
            }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (boss)
        {
            if (collision.collider.CompareTag("Player"))
                collision.collider.GetComponent<MoveByTouch>().damage(5 * Time.deltaTime);
            Instantiate(collisioneff, collision.transform.position, Quaternion.identity);
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        am.stop("Electric SF");
        am.stop("Tank Hit");
    }
    public void damage(float amt)
    {
        player.GetComponent<MoveByTouch>().hit_shots += 1;
        healthleft -= amt;
        if (healthleft <= 0)
        {
            Vector3 pos = head.position;
            pos.y += 3;
            GameObject x = Instantiate(desteff, pos, Quaternion.identity);
            if (boss)
                x.transform.localScale = new Vector3(3, 3, 3);
            am.play("Tank Explode");
            Destroy(gameObject);
        }
        else
            am.play("Tank Hit");
            
    }
   
    void shoot()
    {
        if (player == null)
            return;
        if(!heatseeker && !projectile)
        {
            int lvl = SceneManager.GetActiveScene().buildIndex;
            if (lvl > 15)
                lvl -= 12;
            Vector3 dir = firepoint.position - head.position;
            dir.y = 0;
            RaycastHit hit;
            int i = Random.Range(2*lvl - 42, 42 - 2*lvl);
            if (Physics.Raycast(firepoint.position, dir, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    am.play("Tank Shot");
                    Bullet1 bullet = Instantiate(b_prefab, firepoint.position, Quaternion.Euler(0, i, 0));
                    if (boss)
                        bullet.transform.localScale = new Vector3(2, 1, 2);
                    bullet.hostile = true;
                    bullet.dir = dir.normalized;
                    if (tripleshot)
                    {
                        Bullet1 bullet1 = Instantiate(b_prefab, firepoint.position, Quaternion.Euler(0f, 30f + i, 0f));
                        bullet1.hostile = true;
                        bullet1.dir = dir.normalized;
                        if (boss)
                            bullet1.transform.localScale = new Vector3(2, 1, 2);
                    }
                    if (tripleshot)
                    {
                        Bullet1 bullet1 = Instantiate(b_prefab, firepoint.position, Quaternion.Euler(0f, -30f + i, 0f));
                        bullet1.hostile = true;
                        bullet1.dir = dir.normalized;
                        if (boss)
                            bullet1.transform.localScale = new Vector3(2, 1, 2);
                    }
                }

            }
        }
        else if(heatseeker)
        {
            am.play("Tank Shot 2", 0.5f);
            chasebullet cb = Instantiate(cb_prefab, firepoint.position, Quaternion.identity);
            cb.target = player;
            cb.hostile = true;
            if (boss)
                cb.transform.localScale = new Vector3(2, 1, 2);
        }
        else
        {
            am.play("Tank Shot 2", 1f);
            projectilebullet pb = Instantiate(pb_prefab, firepoint.position, Quaternion.identity);
            pb.destin = player.position;
            if (boss)
                pb.transform.localScale = new Vector3(2, 1, 2);

        }
    }

    
    void Start()
    {
        healthleft = health;
        transform.position = new Vector3(transform.position.x, 6, transform.position.z);
        target = new Vector3(Random.Range(10, 90), 7.5f, Random.Range(-90, -10));
        if(agent != null)
            agent.SetDestination(target);
        if(!heatseeker && !projectile && !boss)
            InvokeRepeating("shoot", 3f, 1f);
        else if(!boss)
            InvokeRepeating("shoot", 3f, 5f);
        else
            InvokeRepeating("shoot", 5f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        Vector3 enem_dir = player.position - transform.position;
        head.forward = enem_dir.normalized;

        if (agent == null)
            return;
        if(agent.remainingDistance <= 10f)
        {
            target = new Vector3(Random.Range(0, 9)*10, transform.position.y, Random.Range(0, 9) * -10);
            agent.SetDestination(target);
        }


    }
}
