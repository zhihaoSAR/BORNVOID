using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hudController : MonoBehaviour {
    public Image healthBar;
    public Image staminaBar;
    public Stats stats;
    
<<<<<<< Updated upstream
=======
    [SerializeField]
    GameObject deathPanel;
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update() {
        //Health
        healthBar.fillAmount = stats.health / stats.maxHealth;
        //Stamina
        staminaBar.fillAmount = stats.stamine;
<<<<<<< Updated upstream
=======

        if (stats.health <= 0) {
            deathPanel.SetActive(true);
        }
>>>>>>> Stashed changes
    }
}
