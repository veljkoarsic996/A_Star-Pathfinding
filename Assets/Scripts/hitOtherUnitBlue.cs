using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitOtherUnitBlue : MonoBehaviour
{
    private GameObject blueTower;
    private GameObject redTower;
    private void Start()
    {
        blueTower = GameObject.FindGameObjectWithTag("BlueTower");
        redTower = GameObject.FindGameObjectWithTag("RedTower");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RedChampion")
        {
            Destroy(this.gameObject);
            redTower.GetComponent<RedDeffence>().blueChampions.Remove(this.gameObject);

            Destroy(other.gameObject);
            blueTower.GetComponent<BlueDeffence>().redChampionsList.Remove(other.gameObject);
        }
    }

}
