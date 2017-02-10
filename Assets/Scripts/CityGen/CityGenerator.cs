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
    public int Width = 10;
    public float NodeSeparation = 5.0f;
    public GameObject Node;
    public List<GameObject> Nodes;
    public GameObject BuildingBasic;

    void Awake()
    {
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
                b.transform.localScale = new Vector3(2.0f, Random.Range(4, 20), 2.0f);
                //int vCount = b.GetComponent<MeshFilter>().mesh.vertexCount;
                //Vector3[] v= new Vector3[vCount];
                //v = b.GetComponent<MeshFilter>().mesh.vertices;
                //v[0].x++;
            }
        }
    }
}
