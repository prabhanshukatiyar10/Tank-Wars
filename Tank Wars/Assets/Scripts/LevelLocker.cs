using UnityEngine.UI;
using UnityEngine;
using System;

public class LevelLocker : MonoBehaviour
{
    public World[] worlds;
    public Sprite[] worldicons;
    public World survivalworld;
    int survivalreached;
    public static bool loaded = false;
    public static GameData gamedata = new GameData();
    int[,] levelstars = new int[3,5];
    int levelreached, worldreached;
    public Sprite levelLockImage;
    public static int totalstars;
    public static int coins;
    public Text starstext;
    

    public void storedata()
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                string s = "levelstars_W" + i.ToString() + "L" + j.ToString();
                PlayerPrefs.SetInt(s, gamedata.levelstars[i - 1, j - 1]);
            }
        }
        PlayerPrefs.SetInt("SurvivalUnlocked", gamedata.survivalint);
        PlayerPrefs.SetInt("WorldReached", gamedata.worldreached);
        PlayerPrefs.SetInt("LevelReached", gamedata.levelreached);
        PlayerPrefs.SetInt("Coins", gamedata.coins);
    }

    void initializedata()
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                string s = "levelstars_W" + i.ToString() + "L" + j.ToString();
                gamedata.levelstars[i - 1, j - 1] = PlayerPrefs.GetInt(s, 0);

                totalstars += levelstars[i - 1, j - 1];
            }
        }
        gamedata.survivalint = PlayerPrefs.GetInt("SurvivalUnlocked", 0);
        gamedata.worldreached = PlayerPrefs.GetInt("WorldReached", 1);
        gamedata.levelreached = PlayerPrefs.GetInt("LevelReached", 1);
        gamedata.coins = PlayerPrefs.GetInt("Coins", 2000);
        loaded = true;
    }
    public void getdata()
    {
        totalstars = 0;
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                string s = "levelstars_W" + i.ToString() + "L" + j.ToString();
                levelstars[i - 1, j - 1] = PlayerPrefs.GetInt(s, 0);

                totalstars += levelstars[i - 1, j - 1];
            }
        }
        survivalreached = PlayerPrefs.GetInt("SurvivalUnlocked", 0);
        worldreached = PlayerPrefs.GetInt("WorldReached", 1);
        levelreached = PlayerPrefs.GetInt("LevelReached", 1);
        coins = PlayerPrefs.GetInt("Coins", 2000);
    }

    

    void lock_unlock_levels()
    {
        if (totalstars >= 20)
        {
            survivalworld.playable = true;
            starstext.text = totalstars.ToString() + "/40";
            starstext.color = Color.green;
        }

        else 
        {
            survivalworld.playable = false;
            starstext.text = totalstars.ToString() + "/20";
            starstext.color = Color.blue;
        }
            
        
        for(int i=1; i<=3; i++)
        {
            if(i < worldreached)
            {
                worlds[i - 1].playable = true;
                for(int j=0; j<5; j++)
                {
                    worlds[i - 1].levels[j].playable = true;
                }
            }
            else if (i == worldreached)
            {
                worlds[i - 1].playable = true;
                for (int j = 1; j <= levelreached; j++)
                {
                    worlds[i-1].levels[j - 1].playable = true;
                }
            }
            else
                worlds[i-1].playable = false;
        }
    }

    void setbuttonstatus(Button b, bool active, Sprite icon = null)
    {
        if (active)
        {
            b.interactable = true;
            b.image.sprite = icon;
            
        }
        else
        {
            b.interactable = false;
            b.image.sprite = levelLockImage;
        }
    }

    void setbuttonsUI()
    {
        setbuttonstatus(survivalworld.worldbutton, survivalworld.playable, worldicons[3]);
        for(int i=0; i<3; i++)
        {
            setbuttonstatus(worlds[i].worldbutton, worlds[i].playable, worldicons[i]);
            for(int j=0; j<5; j++)
            {
                setbuttonstatus(worlds[i].levels[j].button, worlds[i].levels[j].playable);
                Color staractive = Color.blue;
                Color starinactive = Color.blue;
                staractive.a = 1; starinactive.a = 0;

                for (int k = 0; k < 3; k++)
                    if (k < levelstars[i, j])
                        worlds[i].levels[j].starsimage[k].color = staractive;
                    else
                        worlds[i].levels[j].starsimage[k].color = starinactive;
            }
        }


    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (!loaded)
            initializedata();
        storedata();
        getdata();



        lock_unlock_levels();
        setbuttonsUI();

        Debug.Log("coins x " + coins);

    }
    void Update()
    {
        
    }
}
