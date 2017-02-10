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
    public GameObject Node;
    public List<GameObject> Nodes;

    void Awake()
    {
        // Fill with nodes
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                GameObject n = GameObject.Instantiate(Node);
                n.name = "Node_" + x.ToString() + "_" + z.ToString();
                n.transform.position = new Vector3(x * 4, 0.0f, z * 4);
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
    }
}
