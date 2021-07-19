using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    //Skripta za stvaranje matrice nodova

    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 vGridWorldSize;
    public float fNodeRadius;
    public float fDistanceBetweenNodes;
    public Color color;
    public List<Node> FinalPath;
    Node[,] NodeArray;


    float fNodeDiameter;
    int iGridSizeX, iGridSizeY;

    //Postavlja velicinu grida
    private void Start()
    {
        fNodeDiameter = fNodeRadius * 2;
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);

        CreateGrid();
    }
    private void Update()
    {
    }
    //Logika za stvaranje grida
    void CreateGrid()
    {
        NodeArray = new Node[iGridSizeX, iGridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;
        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                //ugnjezdeni for loop koji prolazi kroz svaki node i proverava da li je 
                //zid u koliko je ta tacka u svetu ima layermask za zid postavlja mu boolean da je to prepreka
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                bool Wall = true;

             
                if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask))
                {
                    Wall = false;
                }
                //u Listi tipa Node dodaje taj node i njegov tip ( wall true ili false)
                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    //Skripta koja proverava i vraca sve susedne nodove i dodaje ih u array susednih nodova
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();
        int icheckX;
        int icheckY;

        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }
       
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);
            }
        }

        return NeighborList;
    }

    //Funkcija koja daje lokaciju svih nodova relativnih na scenu
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }
    //Unity funkcija za debuging koja crta sve nodove,
    
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));
        if (NodeArray != null)
        {
            foreach (Node n in NodeArray)
            {
                if (n.bIsWall && !FinalPath.Contains(n))
                {

                    //Beli nodovi su za sve moguce prolaze
                    Gizmos.color = Color.white;
                }
                else
                {
                    //Zuti nodovi oznacavaju prepreke
                    Gizmos.color = Color.yellow;
                }


                if (FinalPath != null)
                {
                    //Obojeni nodovi su nodovi koji oznacavaju putanju
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = color;
                    }

                }


                Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));
            }
        }
    }
}