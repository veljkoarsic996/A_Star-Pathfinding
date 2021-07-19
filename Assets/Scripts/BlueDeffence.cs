using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlueDeffence : MonoBehaviour
{

    //Skripta koja sluzi da se podese parametri za odbranu Plavog tornja kao i za spawnovanje plavih heroja.
    //skripta RedDeffence je identicna kao ova
    public LayerMask hitLayers;
    public GameObject blueChampion;
    private GameObject mainGameManager;

    public bool spawn;

    public TextMeshProUGUI goldTxtMesh;


    public float timeForSpawning = 2.0f;
    private float timeRemaining;

    public float timeForShooting = 2.0f;
    private float timeRemainingForShooting;

    public float goldRechargeTime = 4.0f;
    private float goldRechargeTimeRemaining;
    public int goldAmount;
    public int troopCost = 10;

    public float projectileSpeed = 60.0f;

    public bool isShoot = false;
    public GameObject projectile;

    public List<GameObject> redChampionsList;

    public float movementSpeedForChampion = 5.0f;


    //U start funkciji postavljam osnovne pocetne parametre 
    private void Start()
    {
        mainGameManager = GameObject.FindGameObjectWithTag("MainGameManager");
        goldTxtMesh.text = "" + goldAmount;
        timeRemaining = timeForSpawning;
        timeRemainingForShooting = timeForShooting;
        goldRechargeTimeRemaining = goldRechargeTime;
    }

    //Metoda za stvaranje heroja
    private void spawnBlueChamp(Vector3 pos)
    {
        if (!spawn)
        {
            if (goldAmount >= troopCost)
            {
                //U slucaju da je spawn bool true i da je trenutna kolicina golda veca od cene trupe 
                //Instancira se novi objekat new champ i kroz skriptu MovementScriptBlue
                //se postavljaju pocetni parametri za heroja
                goldAmount -= troopCost;
                GameObject newChamp = Instantiate(blueChampion, pos, Quaternion.identity);
                newChamp.GetComponent<MovementScriptBlue>().setup(movementSpeedForChampion, 100);
                spawn = true;
            }
        }
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))//If the player has left clicked
        {
            Vector3 mouse = Input.mousePosition;//Get the mouse Position
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);//Cast a ray to get where the mouse is pointing at
            RaycastHit hit;//Stores the position where the ray hit.
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))//If the raycast doesnt hit a wall
            {
                if(hit.point.x >= -23 && hit.point.x <= 23 && hit.point.z >= 15 && hit.point.z<= 32)
                    spawnBlueChamp(hit.point);//Move the target to the mouse position
            }
        }
        //Regulisanje pause ekrana i kretanja objekta kroz mapu
        if (!mainGameManager.GetComponent<GameManager>().isPaused)
        {
        //Tajmer za regulisanje stvaranja heroja
        //Za ovu skriptu regulise koliko brzo igrac moze da klikne dugme za stvaranje
            goldTxtMesh.text = "" + goldAmount;
        if (spawn)
            if (timeRemaining <= 0)
            {
                spawn = false;
                timeRemaining = timeForSpawning;
            }
            else if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }

        //Klasican tajmer koji dodaje golde
        if (goldRechargeTimeRemaining <= 0)
        {
            goldAmount += 1;
            goldRechargeTimeRemaining = goldRechargeTime;
        }
        else if (goldRechargeTimeRemaining > 0)
        {
            goldRechargeTimeRemaining -= Time.deltaTime;
        }

        //Petlja koja prolazi kroz svakog crvenog heroja koji se nalazi unutar
        //kolajdera koji je oko plavog tornja
        foreach (GameObject redChamp in redChampionsList)
        {

        //U koliko nije aktiviran boolean isShoot(booolean koji se aktivira kad ispali projektil) 
        //toranj ispaljuje loptu u smeru ka svim crvenim herojima
            if (!isShoot)
            {
                GameObject bullet = Instantiate(projectile, new Vector3(0, 5, 33), Quaternion.identity);
                Vector3 pos = bullet.transform.position;
                //Smer kretanja projektila racuna tako sto od
                //pozicije heroja oduzme poziviju tornja
                Vector3 shootDir = (redChamp.transform.position - pos).normalized;
                bullet.GetComponent<projectileSc>().Setup(shootDir, projectileSpeed, this.gameObject);

            }
            //Proverava prema kog heroja se ispalila zadnja raketa i ako je isti kao poslednji u nizu stavlja 
            //isShoot na true i ogranicava da ne moze vise neko vreme da ispali projektil
            if (redChamp.Equals(redChampionsList[redChampionsList.Count - 1]))
            {
                isShoot = true;
            }


        }
        //kad toranj ispali prema svim neprijateljima vraca isShoot na true i
        //restartuje tajmer
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

    //kad crveni heroj izadje iz kolajdera izbacuje tog istog heroja iz listu 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RedChampion"))
            redChampionsList.Remove(other.gameObject);
    }
    //kad crveni heroj udje u kolajder on trigger ubacuje ga u listu crvenih koji su blizu do tornja
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedChampion"))
        {
            redChampionsList.Add(other.gameObject);
            isShoot = false;
        }
    }

}
