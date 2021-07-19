using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingRed : MonoBehaviour
{
    //Skripta koja nalazi putanju

    private Grid GridReference;
    private Transform StartPosition;
    private Transform TargetPosition;
    public List<Node> FinalPath = new List<Node>();


    private void Start()
    {
        GridReference = GameObject.FindGameObjectWithTag("RedGameManager").GetComponent<Grid>();
        StartPosition = transform;
        TargetPosition = GameObject.FindGameObjectWithTag("BlueTower").transform;


        FindPath(StartPosition.position, TargetPosition.position);
    }


    private void Update()
    {
    }

    //Funkcija za nalazenje putanje 

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        
        //Za pocetak postavlja pocetni i zavrsni node
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);


        //Dve liste koje oznacavaju otvorenu i zatvorenu listu nodova
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        //Dodaje pocetni node u otvorenu listu
        OpenList.Add(StartNode);
        while (OpenList.Count > 0)  {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                //Proverava cenu kretanja od jednog ka drugog noda
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)
                {
                    // U koliko je cena kretanja iz jednog ka drugog noda najmanja
                    //dodaje taj node u otvorenu listu
                    CurrentNode = OpenList[i];
                }
            }
            //Kad proveri sve uzlove izbacuej nod iz otvorenu listu i stavlja ga u zatvorenu
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);
            
            //Kad je trenutni nod jednak krajnom pokrece funkciju za trazenje konacnog puta
            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    //foreach petlja koja prolazi kroz svaki node u susednim nodovima 
                    //i proverava koja je cena za kretanje kroz nodove
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }

        }
    }


    //Funkcija za pronalazenje konacnog puta
    //prolazi kroz listu koja je napravljena za putanju unazad i okrece je da bude aktivna putanja
    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {

        Node CurrentNode = a_EndNode;

        while (CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        FinalPath.Reverse();

        GridReference.FinalPath = FinalPath;

    }

    //Funkcija koja racuna distancu izmedju nodova i vraca njihov zbir
    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);

        return (ix + iy);
    }
}