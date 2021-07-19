using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedDeffence : MonoBehaviour
{
    public GameObject redChampion;
    private GameObject mainGameManager;
    public bool spawn;
    public List<GameObject> blueChampions;

    public TextMeshProUGUI goldTxtMesh;

    public float timeForSpawning= 2.0f;
    private float timeRemaining;

    public float timeForShooting = 2.0f;
    private float timeRemainingForShooting;

    public float goldRechargeTime = 4.0f;
    private float goldRechargeTimeRemaining;
    public int goldAmount;
    public int troopCost = 10;

    public float projectileSpeed = 60.0f;

    public bool isShoot=false;
    public GameObject projectile;


    public float movementSpeedForChampion = 5.0f;

    private void Start()
    {
        mainGameManager = GameObject.FindGameObjectWithTag("MainGameManager");
        goldTxtMesh.text = ""+goldAmount;
        timeRemaining = timeForSpawning;
        timeRemainingForShooting = timeForShooting;
        goldRechargeTimeRemaining = goldRechargeTime;
    }


    void Update()
    {

        if (!mainGameManager.GetComponent<GameManager>().isPaused)
        {

            goldTxtMesh.text = "" + goldAmount;
            if (spawn)
            {
                if (timeRemaining <= 0)
                {
                    if (goldAmount >= troopCost)
                    {
                        goldAmount -= troopCost;
                        int randomX = Random.Range(-23,23);
                        int randomZ = Random.Range(-31,-15);

                        GameObject newChamp = Instantiate(redChampion, new Vector3(randomX, 0.05f, randomZ), Quaternion.identity);
                        newChamp.GetComponent<MovementScriptRed>().setup(movementSpeedForChampion, 100);
                        timeRemaining = timeForSpawning;
                    }

                }
                else if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
            }

            if (goldRechargeTimeRemaining <= 0)
            {
                goldAmount += 1;
                goldRechargeTimeRemaining = goldRechargeTime;
            }
            else if (goldRechargeTimeRemaining > 0)
            {
                goldRechargeTimeRemaining -= Time.deltaTime;
            }

            foreach (GameObject blChamp in blueChampions)
            {


                if (!isShoot)
                {
                    GameObject bullet = Instantiate(projectile, new Vector3(0, 5, -33), Quaternion.identity);
                    Vector3 pos = bullet.transform.position;
                    Vector3 shootDir = (blChamp.transform.position - pos).normalized;
                    bullet.GetComponent<projectileSc>().Setup(shootDir, projectileSpeed, this.gameObject);

                }

                if (blChamp.Equals(blueChampions[blueChampions.Count - 1]))
                {
                    isShoot = true;
                }


            }
            if (timeRemainingForShooting <= 0)
            {
                isShoot = false;
                timeRemainingForShooting = timeForShooting;
            }
            else if (timeRemainingForShooting > 0)
            {
                timeRemainingForShooting -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlueChampion"))
            blueChampions.Remove(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueChampion"))
        {
            blueChampions.Add(other.gameObject);
            isShoot = false;
        }
    }
}
