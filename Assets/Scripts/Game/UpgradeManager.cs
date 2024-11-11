using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject laserButton;
    [SerializeField] private GameObject movementButton;
    [SerializeField] private GameObject healthButton;
    [SerializeField] private Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCombat.score == 50)
        {
            Time.timeScale = 0;
            upgradePanel.SetActive(true);
            //laserButton.SetActive(true);
            //movementButton.SetActive(true);
            //healthButton.SetActive(true);

        }
        //if (PlayerCombat.score == 101)
        //{
        //    Time.timeScale = 0;
        //    upgradePanel.SetActive(true);
        //}
        //if (PlayerCombat.score == 152)
        //{
        //    Time.timeScale = 0;
        //    upgradePanel.SetActive(true);
        //}
    }

    public void UpgradeLaser()
    {
        PlayerCombat.laserUpgrade = true;
        PlayerCombat.score = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        laserButton.SetActive(false);
    }
    public void UpgradeHealth()
    {
        playerHealth.AddMaxHealth(25);
        PlayerCombat.score = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        healthButton.SetActive(false);
    }
    public void UpgradeMovement()
    {
        PlayerMovement.mSpeed = 10f;
        PlayerCombat.score += 1;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        movementButton.SetActive(false);
    }
}
