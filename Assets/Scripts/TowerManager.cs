using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    // Skripta koja regulise rad tornja
    [Range(0.0f,100.0f)]
    public int health = 100;

    public Slider healthSlider;

    public GameObject redWonScreen, blueWonScreen;

    void Update()
    {
     //Proverava koliko helta je ostalo na tornju
        healthSlider.value = health;
      if (health <= 0)
        {
            GameObject mainGameManager = GameObject.FindGameObjectWithTag("MainGameManager");
            mainGameManager.GetComponent<GameManager>().isPaused = true;
            //Posto je ista skripta za oba tornja 
            //proverava koji toranj prvi izgubi helte
            //i postavlja da je suprotni pobedio
            if (this.gameObject.name == "RedTower")
            {
                blueWonScreen.SetActive(true);
            }
            else if (this.gameObject.name == "BlueTower") {
                redWonScreen.SetActive(true);
            }
        }
    }

    //Funkcija koju pozivaju heroji i diluju stetu tornju
    public void dealDamageToTower(int dmg) {
        if (health >= 0)
        {
            health -= dmg;
        }
    }
}
