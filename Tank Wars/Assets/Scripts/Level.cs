
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Level
{

    public Image[] starsimage = new Image[3];
    public Button button;
    public bool playable = false;
    public int worldno, levelno;
}
[Serializable]
public class World
{
    public Level[] levels;
    public Button worldbutton;
    public bool playable;
}

[Serializable]
public class GameData
{
    public int[,] levelstars;
    public int worldreached;
    public int coins;
    public int survivalint;
    public int levelreached;

    public GameData()
    {
        levelstars = new int[3, 5];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                levelstars[i, j] = 0;
        worldreached = 1;
        levelreached = 1;
        coins = 0;
        survivalint = 0;
        
    }

}
[Serializable]
public class PlayerTankData
{
    public float muzzlevelocity = 60, tankvelocity = 40, bulletdamage = 100, health= 400;

    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("muzzlevelocity", muzzlevelocity);
        PlayerPrefs.SetFloat("tankvelocity", tankvelocity);
        PlayerPrefs.SetFloat("bulletdamage", bulletdamage);
        PlayerPrefs.SetFloat("health", health);
    }
    

}
