using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// CityGenerator nachocpol@gmail.com
/// This class is in charge of generating the city layout
/// and the roads.
/// </summary>
public class CityGenerator : MonoBehaviour
{
    public int Width = 500;
    public float NodeSeparation = 5.0f;
    public GameObject Node;
    public List<GameObject> Nodes;
    public GameObject BuildingBasic;

    void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        // Fill with nodes
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                GameObject n = GameObject.Instantiate(Node);
                n.name = "Node_" + x.ToString() + "_" + z.ToString();
                n.transform.position = new Vector3(x * NodeSeparation, 0.0f, z * NodeSeparation);
                Nodes.Add(n);
            }
        }
        // Link the nodes
        for (int z = 0; z < Width; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                int idx = x * Width + z;
                // Top link 
                if(z<Width-1)
                {
                    int curI = x * Width + (z + 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = Nodes[curI];
                    nn.Distance = Vector3.Distance(Nodes[idx].transform.position,
                                                    Nodes[curI].transform.position);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Bottom link
                if(z>0)
                {
                    int curI = x * Width + (z - 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = Nodes[curI];
                    nn.Distance = Vector3.Distance(Nodes[idx].transform.position,
                                                    Nodes[curI].transform.position);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Right link
                if(x<Width-1)
                {
                    int curI = (x + 1) * Width + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = Nodes[curI];
                    nn.Distance = Vector3.Distance(Nodes[idx].transform.position,
                                                    Nodes[curI].transform.position);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Left link
                if(x>0)
                {
                    int curI = (x - 1) * Width + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = Nodes[curI];
                    nn.Distance = Vector3.Distance(Nodes[idx].transform.position,
                                                    Nodes[curI].transform.position);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
            }
        }
        AddBuildings();
        FillGraph();
    }

    void AddBuildings()
    {
        for (int z = -1; z < Width; z++)
        {
            for (int x = -1; x < Width; x++)
            {
                Vector3 bPos = new Vector3( (x+0.5f) * NodeSeparation, 
                                            0.0f,
                                            (z + 0.5f) * NodeSeparation);
                GameObject b = GameObject.Instantiate(BuildingBasic, bPos, Quaternion.identity)as GameObject;
                float h = Random.Range(2.0f, 7.0f);
                float w = Random.Range(1.0f, 3.0f);
                Debug.Log(h + "," + w);
                b.GetComponent<MeshFilter>().mesh = GeometryGen.Instance.GenBuilding(h,w);
            }
        }
    }

    void FillGraph()
    {
        // For each generated node
        for(int i=0;i<Nodes.Count;i++)
        {
            Node n = Nodes[i].GetComponent<Node>();
            if (!n) Debug.LogWarning("This object does not have a node component.");

            // Build a list of the neighbours of this node
            Dictionary<Vector3, double> nb = new Dictionary<Vector3, double>();
            for (int j = 0;j<n.Neighbors.Count;j++)
            {
                nb.Add(n.Neighbors[j].OtherNode.transform.position, n.Neighbors[j].Distance);
            }
            // Add the node and nb to the grid
            WaypointsExample.CarsGraph.AddNodeToGraph(n.transform.position, nb);
        }
    }
}
