using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrader : MonoBehaviour
{
    private GameManager gameManager;
    private UpgradeManager upgradeManager;
    public string text;
    public string upgrade;
    public TMP_Text textDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        upgradeManager = GameObject.Find("GameManager").GetComponent<UpgradeManager>();
        textDisplay.text = text;
    }

    public void Upgrade(){
        upgradeManager.upgradeStat(upgrade);
    }

    public void Downgrade(){
        upgradeManager.downgradeStat(upgrade);
    }
}
