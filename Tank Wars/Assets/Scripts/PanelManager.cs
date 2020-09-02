using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;


    public void askforconf()
    {
        panels[9].SetActive(true);
    }
    public void dontreset()
    {
        panels[9].SetActive(false);
    }
    public void resetgame()
    {
        panels[9].SetActive(false);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LevelLocker.loaded = false;
    }
    void start_transition()
    {
        foreach (GameObject panel in panels)
            panel.SetActive(false);
    }
    public void loadpanel(int i)
    {
        start_transition();
        panels[i].SetActive(true);
    }
    public void exitapp()
    {
        Application.Quit();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
