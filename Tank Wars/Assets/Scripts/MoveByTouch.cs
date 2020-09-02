using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouch : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 40f;
    float horizontalmove, verticalmove = 0f;
    public Joystick jstk;
    public Joystick aimjstk;
    Vector3 jstpos;
    float time = 0;
    public bool stopcollision = false;
    public Transform firepoint;
    public Bullet1 b_prefab;
    public Transform jstkpos;
    public Transform body, head;
    public float health, healthleft;
    public GameObject desteff;
    public GameMaster gm;
    public SurvivalMaster sm;
    [HideInInspector]
    public int no_of_shots = 0, hit_shots = 0;
    public AudioManager am;
    public GameObject Collisioneff;
    public Light headlight;
    public static PlayerTankData tankdata;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BurningWall"))
        {
            am.play("Electric SF");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("BurningWall"))
        {
            damage(50 * Time.deltaTime);
            Instantiate(Collisioneff, transform.position, Quaternion.identity);
        }
            
    }
    private void OnCollisionExit(Collision collision)
    {
        am.stop("Electric SF");
        am.stop("Tank Hit");
    }

    private void OnDestroy()
    {
        am.stop("Electric SF");
        if(gm != null)
            gm.destroyed = true;
        if (sm != null)
            sm.destroyed = true;

    }
    public void damage(float amt)
    {
        healthleft -= amt;
        headlight.intensity *= healthleft / health;
        if (healthleft <= 0)
        {
            Vector3 pos = head.position;
            pos.y += 3;
            Instantiate(desteff, pos, Quaternion.identity);
            am.play("Tank Explode");
            Destroy(gameObject);

        }
        else
            am.play("Tank Hit");
        
    }

    void shoot()
    {
        Bullet1 bullet = Instantiate(b_prefab, firepoint.position, Quaternion.Euler(0, 0 , 0));
        no_of_shots++;
        bullet.dir = (firepoint.position - head.position).normalized;
        bullet.hostile = false;
        bullet.dir.y = 0;
        am.play("Tank Shot");
    }

    void Loadtank()
    {
        b_prefab.speed = PlayerPrefs.GetFloat("muzzlevelocity", 60);
        speed = PlayerPrefs.GetFloat("tankvelocity", 40);
        b_prefab.damage = PlayerPrefs.GetFloat("bulletdamage", 100);
        health = PlayerPrefs.GetFloat("health", 400);
    }
    private void Awake()
    {
        Loadtank();
    }
    private void Start()
    {
        //Loadtank();
        // Physics.gravity = new Vector3(0f, -1000f, 0f);
        jstk = GameObject.FindGameObjectWithTag("GameController").GetComponent<FixedJoystick>();
        healthleft = health;
        jstpos = new Vector3(jstkpos.position.x, head.position.y, jstkpos.position.z);
    }
    void Update()
    {
        time += Time.deltaTime;
        if (transform.position.y <=6.1)
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        if (Mathf.Abs(jstk.Horizontal) > 0.3f)
            horizontalmove = (jstk.Horizontal);
        else
            horizontalmove = 0;

        if (Mathf.Abs(jstk.Vertical) > 0.3f)
            verticalmove = (jstk.Vertical);
        else
            verticalmove = 0f;
        Vector3 dir = new Vector3(horizontalmove, 0f, verticalmove);
        body.forward = Vector3.Lerp(body.forward, dir, 0.5f);
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        if(aimjstk.Horizontal != 0 || aimjstk.Vertical !=0)
        head.forward = new Vector3(aimjstk.Horizontal, 0, aimjstk.Vertical).normalized;

        Vector2 check = new Vector2(aimjstk.Horizontal, aimjstk.Vertical);

        {
            if (check.sqrMagnitude >= 0.4)
            {
                if (time >= 0.5)
                {
                    shoot();
                    time = 0;
                }
            }
            Vector3 restorepos = transform.position;
            bool outside = false;
            if (transform.position.x < 2)
            {
                restorepos.x = 2;
                outside = true;
            }

            if (transform.position.x > 98)
            {
                restorepos.x = 98;
                outside = true;
            }
            if (transform.position.z > -2)
            {
                restorepos.z = -2;
                outside = true;
            }
            if (transform.position.z < -98)
            {
                restorepos.z = -98;
                outside = true;
            }
            if (outside)
                transform.position = restorepos;

        }
    }

}
