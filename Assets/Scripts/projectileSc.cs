using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSc : MonoBehaviour
{
    //Skripta za projektile

    private Vector3 b_shootDir;
    private float b_moveSpeed = 10f;
    private GameObject b_parrent;
    
    //Funkcija koja postavlja pocetne parametre za projektile
    //Ovu funkciju poziva toranj kad instancira objekat
    public void Setup(Vector3 shootDir , float moveSpeed , GameObject parrent) {
        b_shootDir = shootDir;
        b_moveSpeed = moveSpeed;
        b_parrent = parrent;
    }


    private void Update()
    {
        //Pomeranje projektila u smer kretanja sa odredjenom brzinom
     transform.position += b_shootDir * b_moveSpeed * Time.deltaTime ;

            
     if(transform.position.y <= -5.0f) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //Kad toranj udari u heroja proverava koji je tag

        //Za plavog heroja oduzme 50 helta
        //i unisti sebe
        if (other.CompareTag("BlueChampion"))
        {
            Destroy(this.gameObject);
            other.GetComponent<MovementScriptBlue>().setParrent(b_parrent);
            other.GetComponent<MovementScriptBlue>().dealDmgToHero(50);
            Debug.Log("HIT BLUE");
        }
        //za crvenog heroja unisti oba objekta
        if (other.CompareTag("RedChampion"))
        {
            Destroy(this.gameObject);
            b_parrent.GetComponent<BlueDeffence>().redChampionsList.Remove(this.gameObject);
            Debug.Log("HIT RED");
        }

        // U oba slucaja uklanja taj objekat iz liste heroja
    }
}
