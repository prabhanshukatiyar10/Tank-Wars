using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image loadscreen;
    public AnimationCurve fadein;


    public void beginscene()
    {
        StartCoroutine(startscene());
    }
    public void endsceneto(int index)
    {
        Time.timeScale = 1;
        StartCoroutine(fadeintoscene(index));
    }
    IEnumerator startscene()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return 0;
            loadscreen.fillAmount = fadein.Evaluate(t);
        }
    }
    IEnumerator fadeintoscene(int index)
    {
        
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            yield return 0;
            loadscreen.fillAmount = fadein.Evaluate(t);
        }
        SceneManager.LoadScene(index);
        
    }
    
}
