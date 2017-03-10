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
    public List<GameObject> CarNodes;
    public List<GameObject> PedestrianNodes;
    public GameObject BuildingBasic;

    void Awake()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        // Initialize the nodes
        InitializeCarNodes();
        InitializePedestrianNodes();
        
        // Generate the city
        AddBuildings();

        // Fill the graphs
        FillGraph(ref WaypointsExample.CarsGraph,ref CarNodes);
        //FillGraph(WaypointsExample.PedestriansGraph, PedestrianNodes);
    }

    void AddBuildings()
    {
        for (int z = 0; z < Width; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 bPos = new Vector3( (x+0.5f) * NodeSeparation, 
                                            0.0f,
                                            (z + 0.5f) * NodeSeparation);
                GameObject b = GameObject.Instantiate(BuildingBasic, bPos, Quaternion.identity)as GameObject;
                float h = Random.Range(1.0f, 1.0f);
                float w = Random.Range(1.0f, 1.0f);
                Debug.Log(h + "," + w);
                b.GetComponent<MeshFilter>().mesh = GeometryGen.Instance.GenBuilding(h,w);
            }
        }
    }

    void FillGraph(ref Graph graph,ref List<GameObject> nodes)
    {
        // For each generated node
        for(int i=0;i<nodes.Count;i++)
        {
            Node n = nodes[i].GetComponent<Node>();
            if (!n) Debug.LogWarning("This object does not have a node component.");

            // Build a list of the neighbours of this node
            Dictionary<Vector3, double> nb = new Dictionary<Vector3, double>();
            for (int j = 0;j<n.Neighbors.Count;j++)
            {
                nb.Add(n.Neighbors[j].OtherNode.transform.position, n.Neighbors[j].Distance);
            }
            // Add the node and nb to the grid
            graph.AddNodeToGraph(n.transform.position, nb);
        }
    }

    void InitializeCarNodes()
    {
        // Fill with nodes
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                GameObject n = GameObject.Instantiate(Node);
                n.name = "Node_" + x.ToString() + "_" + z.ToString();
                n.transform.position = new Vector3(x * NodeSeparation, 0.0f, z * NodeSeparation);
                n.GetComponent<Node>().DebugColor = Color.red;
                CarNodes.Add(n);
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
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Bottom link
                if(z>0)
                {
                    int curI = x * Width + (z - 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Right link
                if(x<Width-1)
                {
                    int curI = (x + 1) * Width + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Left link
                if(x>0)
                {
                    int curI = (x - 1) * Width + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
            }
        }
    }

    void InitializePedestrianNodes()
    {
        // To-Do: proper building size
        Vector3 nodeOff = new Vector3(0.5f, 0.0f, 0.5f) * 1.25f;

        // Fill nodes
        for (int z = 0; z < Width; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 bPos = new Vector3((x + 0.5f) * NodeSeparation,
                                            0.0f,
                                            (z + 0.5f) * NodeSeparation);
                // Br node
                GameObject n = GameObject.Instantiate(Node);
                n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
                n.transform.position = new Vector3(bPos.x - nodeOff.x, 0.0f, bPos.z - nodeOff.z);
                n.GetComponent<Node>().DebugColor = Color.blue;
                PedestrianNodes.Add(n);

                // Br node
                n = GameObject.Instantiate(Node);
                n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
                n.transform.position = new Vector3(bPos.x + nodeOff.x, 0.0f, bPos.z - nodeOff.z);
                n.GetComponent<Node>().DebugColor = Color.blue;
                PedestrianNodes.Add(n);

                // Tr node
                n = GameObject.Instantiate(Node);
                n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
                n.transform.position = new Vector3(bPos.x + nodeOff.x, 0.0f, bPos.z + nodeOff.z);
                n.GetComponent<Node>().DebugColor = Color.blue;
                PedestrianNodes.Add(n);

                // Tl node
                n = GameObject.Instantiate(Node);
                n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
                n.transform.position = new Vector3(bPos.x - nodeOff.x, 0.0f, bPos.z + nodeOff.z);
                n.GetComponent<Node>().DebugColor = Color.blue;
                PedestrianNodes.Add(n);
            }
        }

        // Link Nodes
        for (int z = 0; z < Width * 2.0f; z++)
        {
            for (int x = 0; x < Width * 2.0f; x++)
            {
                int idx = x * (Width * 2) + z;
                // Top link 
                if (z < Width - 1)
                {
                    int curI = x * (Width * 2) + (z + 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Bottom link
                if (z > 0)
                {
                    int curI = x * (Width * 2) + (z - 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Right link
                if (x < Width - 1)
                {
                    int curI = (x + 1) * (Width * 2) + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Left link
                if (x > 0)
                {
                    int curI = (x - 1) * (Width * 2) + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
            }
        }
    }
}
