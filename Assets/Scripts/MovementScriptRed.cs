using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScriptRed : MonoBehaviour
{
    private GameObject mainGameManager;
    private GameObject gameManager;
    private GameObject blueTower;
    public Animator anim;
    List<Node> putanja = new List<Node>();
    public bool isThere = false;
    public int pos = 0;
    public int lenght = 0;
    private float r_speed = 3.0f;



    public int movementFactor = 1;

    private Vector3 curPos,lastPos;

    public float attackTime = 2.0f;
    private float timeRemaining;

    public int damageDelt = 5;

    public GameObject b_parrent;
    private float b_speed = 5.0f;
    private int b_health = 100;

    private void Start()
    {
        mainGameManager = GameObject.FindGameObjectWithTag("MainGameManager");
        isThere = false;
        gameManager = GameObject.FindGameObjectWithTag("RedGameManager");
        blueTower = GameObject.FindGameObjectWithTag("BlueTower");
        timeRemaining = attackTime;
    }
    public void setParrent(GameObject parrent)
    {
        b_parrent = parrent;
    }

    public void setup(float speed, int health)
    {
        b_speed = speed;
        b_health = health;
    }

    public void dealDmgToHero(int dmg)
    {
        b_health -= dmg;
        Debug.Log("POGODJEN");
    }
    public void DestroyObject() { 
    
    }

    void Update()
    {
        if (!mainGameManager.GetComponent<GameManager>().isPaused)
        {
            Debug.Log("Health : " + b_health);
            if (!isThere)
            {
                putanja = this.GetComponent<PathfindingRed>().FinalPath;
                curPos = transform.position;


                //Debug.Log(pos + "|" + putanja.Count);

                lenght = putanja.Count - 1;

                if (Vector3.Distance(transform.position, putanja[pos].vPosition) >= 1 && pos <= lenght)
                {

                    Vector3 destination = putanja[pos].vPosition;
                    Vector3 targetDir = putanja[pos].vPosition - transform.position;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, r_speed * Time.deltaTime, 0.0f);

                    float step = r_speed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    transform.position = Vector3.MoveTowards(transform.position, destination, step);

                }
                else if (Vector3.Distance(transform.position, putanja[pos].vPosition) <= 1 && pos <= lenght)
                {

                    pos += movementFactor;

                }


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
            else if (isThere)
            {
                if (timeRemaining <= 0)
                {
                    blueTower.GetComponent<TowerManager>().dealDamageToTower(damageDelt);
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
