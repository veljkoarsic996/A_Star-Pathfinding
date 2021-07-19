using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //Objekat node koji sluzi da se inicijalizuje node i njegova cena kretanja do sledeceg

    public int iGridX;
    public int iGridY;

    public bool bIsWall;
    public Vector3 vPosition;

    public Node ParentNode;//Node koji oznacava koji node je bio prethodni

    public int igCost;//Cena kretanja do sledeceg noda
    public int ihCost;//Distanca izmedju ovog noda do cilja

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool a_bIsWall, Vector3 a_vPos, int a_igridX, int a_igridY)
    {
        bIsWall = a_bIsWall;
        vPosition = a_vPos;
        iGridX = a_igridX;
        iGridY = a_igridY;
    }

}