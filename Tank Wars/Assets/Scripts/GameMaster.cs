using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Level levelcompo;
    public MoveByTouch player;
    public Text health, time;
    public float Timetotal;
    public bool inftime;
    float currtime;
    public bool destroyed;
    public GameObject failed, completed, pausemenu, playpanel;
    public Gradient healthcolor, timecolor;
    public Image healthfill, timefill;
    public Slider healthS;
    float Health, HealthT;
    bool paused = false;
    bool gamended = false;
    public AudioManager am;
    public SceneFader scenefader;
    public RawImage[] stars;
    float accuracy;
    bool accuracystar, healthstar, levelstar;
    int no = 0;
    int coins = 0;
    public Text coinstext;



    
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

    IEnumerator givestars()
    {
        
        if (accuracystar)
            no++;
        if (healthstar)
            no++;
        if (levelstar)
            no++;
        if(no > LevelLocker.gamedata.levelstars[levelcompo.worldno - 1, levelcompo.levelno - 1])
            LevelLocker.gamedata.levelstars[levelcompo.worldno - 1, levelcompo.levelno - 1] = no;
        int rand = (int)Mathf.Pow(levelcompo.worldno, 3) + levelcompo.levelno;
        coins = Random.Range(rand*2, rand*3);
        //Debug.Log("earned " + coins);
        LevelLocker.gamedata.coins += coins;


        coinstext.text = "x 0";
        yield return new WaitForSecondsRealtime(3);
        for(int i=0; i<no; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            stars[i].color = Color.white;
        }
        coinstext.text = "x " + coins.ToString();
    }
   
    void unlocknextlevel()
    {
        if (levelcompo.levelno == 5)
        {
            
            {
                LevelLocker.gamedata.worldreached += 1;
                LevelLocker.gamedata.levelreached = 1;
            }
            
        }
        else
        {
            if(LevelLocker.gamedata.levelreached == levelcompo.levelno)
                LevelLocker.gamedata.levelreached = levelcompo.levelno + 1;
        }
            
    }
    
    
    
     IEnumerator check_gameover()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            unlocknextlevel();
            gamended = true;
            levelstar = true;
            if (accuracy >= 0.5)
                accuracystar = true;
            if (Health / HealthT > 0.5)
                healthstar = true;
            StartCoroutine(givestars());
            am.stop("Electric SF");
            yield return new WaitForSeconds(3);
            completed.SetActive(true);
            am.play("Victory");

            //Time.timeScale = 0;
            
            


        }
        else if (time.text == "0")
        {

            player.damage(player.health);
            gamended = true;
            
            yield return new WaitForSeconds(3);
            failed.SetActive(true);
            am.play("Defeat");
            levelstar = false;
            healthstar = false;
            accuracystar = false;
           // Time.timeScale = 0;
        }

        else if (destroyed)
        {
            gamended = true;
            
            yield return new WaitForSeconds(3);
            failed.SetActive(true);
            am.play("Defeat");

            // Time.timeScale = 0;
            levelstar = false;
            healthstar = false;
            accuracystar = false;
            
        }
        
           
        
    }
    void SetText()
    {
        if (healthS.value > Health)
            healthS.value = healthS.value - 200 * Time.deltaTime;
        else
            healthS.value = Health;
        healthfill.color = healthcolor.Evaluate(healthS.normalizedValue);

        health.text = ((int)((healthS.value/HealthT)*100)).ToString();

        timefill.color = timecolor.Evaluate(currtime / Timetotal);
        timefill.fillAmount = currtime / Timetotal;
        time.text = ((int)currtime+1).ToString();

        if (inftime)
            time.text = "inf";

        if (player.no_of_shots == 0)
            accuracy = 0;
        else
            accuracy = ((float)(player.hit_shots)) / (float)(player.no_of_shots);

    }
    private void Awake()
    {
        
    }
    void Start()
    {
        scenefader.beginscene();
        if (inftime)
            Timetotal = Mathf.Infinity;
        playpanel.SetActive(true);
        currtime = Timetotal;
        am.play("BG Music");
        HealthT = player.health;
        healthS.maxValue = HealthT;
        



    }

    // Update is called once per frame
    void Update()
    {
        if (gamended)
        {
            am.stop("BG Music");
            playpanel.SetActive(false);
        }
            
        currtime -= Time.deltaTime;
        if(player != null)
            Health = player.healthleft;
        if(!gamended)
            SetText();
        if(!gamended)
            StartCoroutine(check_gameover());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!gamended)
                pause();
        }
        if (gamended)
            if(player != null)
                player.enabled = false;
    }
}
