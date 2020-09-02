using UnityEngine.UI;
using UnityEngine;

public class ShopMaster : MonoBehaviour
{
    
    public Image[] fillbars;
    public static PlayerTankData tankdata;
    static int price = 10;
    public Button modifybutton;
    public Text amttext, pricetext;
    public Gradient bargrad;
    void modify()
    {
        tankdata.muzzlevelocity = Random.Range(50, 80);
        tankdata.health = Random.Range(250, 550);
        tankdata.tankvelocity = Random.Range(35, 45);
        tankdata.bulletdamage = Random.Range(50, 200);
        tankdata.SaveSettings();
        LevelLocker.gamedata.coins -= price;
        price += Random.Range(20, 40);
        LevelLocker ll = FindObjectOfType<LevelLocker>();
        ll.storedata();
        ll.getdata();
        PlayerPrefs.SetInt("price", price);
        Debug.Log("coinsleft = " + LevelLocker.gamedata.coins);
        Debug.Log("price = " + price);
        

    }
    void setUI()
    {
        float[] fill = new float[5];
        fill[0] = (PlayerPrefs.GetFloat("tankvelocity", 40) - 35) * 0.6f/10 + 0.2f;
        fill[1] = (PlayerPrefs.GetFloat("muzzlevelocity", 60) - 50) * 0.6f/30 + 0.2f;
        fill[2] = (PlayerPrefs.GetFloat("bulletdamage", 100) - 50) * 0.6f/150 + 0.2f;
        fill[3] = (PlayerPrefs.GetFloat("health", 400)-250) * 0.6f/300 + 0.2f;
        amttext.text = (LevelLocker.gamedata.coins).ToString();
        pricetext.text = "Price: " + price.ToString();
        
        for(int i=0; i<4; i++)
        {
            fillbars[i].fillAmount = fill[i];
            fill[4] += fill[i] / 4;
            fillbars[i].color = bargrad.Evaluate(fill[i]);
        }
        fillbars[4].fillAmount = fill[4];
        fillbars[4].color = bargrad.Evaluate(fill[4]);
    }

    public void buttonfn()
    {
        modify();
        setUI();
    }
    void Start()
    {
        price = PlayerPrefs.GetInt("price", 10);
        tankdata = new PlayerTankData();
        setUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelLocker.coins < price)
            modifybutton.interactable = false;
        else
            modifybutton.interactable = true;
    }
}
