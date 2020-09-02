using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalMaster : MonoBehaviour
{
    
    public MoveByTouch player;
    public Text health;
    public bool destroyed = false;
    public GameObject survivalover, pausemenu, playpanel;
    public GameObject spawneff;
    public Text cointext, timetext;
    public Gradient healthcolor;
    public Image healthfill;
    public Slider healthS;
    float Health, HealthT;
    bool paused = false;
    bool gamended = false;
    public AudioManager am;
    public SceneFader scenefader;
    public Transform[] spawnpoints;
    public GameObject[] enemies;
    int level = 1;
    float currtime = 0;
    float timesurvived = 0;
    bool showgameend = false;
    int coins;
    


    void spawnenemy()
    {
        
        GameObject e = enemies[Random.Range(0, Mathf.Min(level, enemies.Length))];
        Transform spoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
        GameObject enemy = Instantiate(e, spoint.position, spoint.rotation);
        Instantiate(spawneff, spoint.position, spoint.rotation);
        enemy.GetComponent<TankMovement>().player = player.transform;
        enemy.GetComponent<TankMovement>().am = am;

    }

    IEnumerator spawnlevel()
    {
        for(int i=0; i<Mathf.Min( level*Random.Range(1, 1.5f), 15); i++)
        {
            spawnenemy();
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
        level++;
    }

    void startlevel()
    {
        StartCoroutine(spawnlevel());
    }
    void checkgameover()
    {
        if (destroyed)
            gamended = true;

    }

    IEnumerator endgame()
    {
        am.stop("BG Music");
        playpanel.SetActive(false);
        yield return new WaitForSeconds(3);
        survivalover.SetActive(true);
        am.play("Defeat");
        LevelLocker.gamedata.coins += coins;
    }


    public void pause()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            pausemenu.SetActive(true);
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            pausemenu.SetActive(false);
            paused = false;
        }


    }


    
    void SetText()
    {
        if (healthS.value > Health)
            healthS.value = healthS.value - 200 * Time.deltaTime;
        else
            healthS.value = Health;
        healthfill.color = healthcolor.Evaluate(healthS.normalizedValue);

        health.text = ((int)(healthS.value / (HealthT / 100))).ToString();
        coins = (int)(timesurvived / Random.Range(30, 60));
        string timescore;
        if((((int)timesurvived) % 60) > 9)
            timescore = ((int)(timesurvived / 60)).ToString() + ":" + (((int)timesurvived) % 60).ToString();
        else
            timescore = ((int)(timesurvived / 60)).ToString() + ":0" + (((int)timesurvived) % 60).ToString();
        cointext.text = "x" + coins.ToString();
        timetext.text = timescore;

    }
    private void Awake()
    {

    }
    void Start()
    {
        scenefader.beginscene();
        
        playpanel.SetActive(true);
        am.play("BG Music");
        HealthT = player.health;
        healthS.maxValue = HealthT;
        InvokeRepeating("checkgameover", 1f, 0.5f);

        InvokeRepeating("spawnenemy", 2f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        timesurvived += Time.deltaTime;
        {
            currtime += Time.deltaTime;
            if (currtime >= level * 10)
            {
                StartCoroutine(spawnlevel());
                currtime = 0;
            }
                
        }
        if (gamended && !showgameend)
        {
            StartCoroutine(endgame());
            showgameend = true;
        }

        
        if (player != null)
            Health = player.healthleft;
        if (!gamended)
            SetText();
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamended)
                pause();
        }
        if (gamended)
            if (player != null)
                player.enabled = false;
    }
}
