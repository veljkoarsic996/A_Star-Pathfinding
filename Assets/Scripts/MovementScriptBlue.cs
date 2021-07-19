using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScriptBlue : MonoBehaviour
{
    //Skripta za kretanje heroja
    //ova skripta je ista kao i MovementScriptRed

    private GameObject mainGameManager;
    private GameObject gameManager;
    private GameObject redTower;
    public Animator anim;
    List<Node> putanja = new List<Node>();
    public bool isThere = false;
    public int pos = 0;
    public int lenght = 0;



    public int movementFactor = 1;

    private Vector3 curPos,lastPos;

    public float attackTime = 2.0f;
    private float timeRemaining;

    public int damageDelt = 5;

    public GameObject b_parrent;
    private float b_speed = 5.0f;
    private int b_health = 100;



    //Start funkcija koja postavlja pocetne parametre
    private void Start()
    {
        mainGameManager = GameObject.FindGameObjectWithTag("MainGameManager");
        isThere = false;
        gameManager = GameObject.FindGameObjectWithTag("BlueGameManager");
        redTower = GameObject.FindGameObjectWithTag("RedTower");
        timeRemaining = attackTime;
    }

    //Parrent objekat sluzi da zna koji objekat je stvorio Heroja (u ovom slucaju BlueTower)
    public void setParrent(GameObject parrent) {
        b_parrent = parrent;
    }
    //Funkcija koja postavlja pocetne parametre za heroja, brzinu i broj helti
    public void setup(float speed , int health) {
        b_speed = speed;
        b_health = health;
    }
    //funkcija koju poziva protivnicki toranj i sluzi da oduzme helte heroju
    public void dealDmgToHero(int dmg) {
        b_health -= dmg;
        Debug.Log("POGODJEN");
    }

    public void DestroyObject() {
        Destroy(this.gameObject);
        b_parrent.GetComponent<RedDeffence>().blueChampions.Remove(this.gameObject);
    }

    void Update()
    {
        if (!mainGameManager.GetComponent<GameManager>().isPaused)
        {
            //Proverava koliko helti ima trenutni heroj
            Debug.Log("Health : " + b_health);
            if (b_health <= 0)
            {
                //u koliko je jednak nuli unistava se objekat
                Destroy(this.gameObject);
                b_parrent.GetComponent<RedDeffence>().blueChampions.Remove(this.gameObject);
            }
            //Proverava da li je heroj na destinaciji
            if (!isThere)
            {
                
                //U koliko nije postavlja putanju kretanja 
                putanja = this.GetComponent<PathfindingBlue>().FinalPath;
                curPos = transform.position;

                lenght = putanja.Count - 1;
                //prolazi kroz svaki node u listi i proverava distancu izmedju sebe i trenutne pozicije u listi
                if (Vector3.Distance(transform.position, putanja[pos].vPosition) >= 1 && pos <= lenght)
                {
                    //u koliko je dalje od noda rotira se u smer prema tom nodu i 
                    //krece se po odredjenom faktoru do te lokacije
                    Vector3 destination = putanja[pos].vPosition;
                    Vector3 targetDir = putanja[pos].vPosition - transform.position;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, b_speed * Time.deltaTime, 0.0f);

                    float step = b_speed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    transform.position = Vector3.MoveTowards(transform.position, destination, step);

                }
                else if (Vector3.Distance(transform.position, putanja[pos].vPosition) <= 1 && pos <= lenght)
                {
                    //kad je heroj blizu ili na lokaciju noda dodaje se movementFactor koji oznacava 
                    // koliko noda preskace heroj
                    pos += movementFactor;

                }

                //Handler za animacije , ako je igrac blizu do tornja pokrece animaciju za udaranje u toranj
                if (pos != lenght)
                {
                    anim.SetInteger("condition", 1);
                }
                else if (pos == lenght)
                {
                    anim.SetInteger("condition", 2);
                    isThere = true;
                }

            }
            //Tajmer koji regulise koliko cesto igrac udari u toranj
            else if (isThere)
            {
                if (timeRemaining <= 0)
                {
                    //Funkcija iz skripte TowerManager koja sluzi da se smanje helti tornju
                    redTower.GetComponent<TowerManager>().dealDamageToTower(damageDelt);
                    timeRemaining = attackTime;
                }
                else if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }

            }

        }
    }
}
