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
                n.transform.position = new Vector3(x, 0.0f, z);
                Nodes.Add(n);
            }
        }
        // Link the nodes
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                int idx = x * Width + z;
                // Top link 
                if(z<Width)
                {
                    int curI = x * Width + (z+1);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(Nodes[curI]);
                }
                // Bottom link
                if(z>0)
                {
                    int curI = x * Width + (z - 1);
                    Nodes[idx].GetComponent<Node>().AddNeighbor(Nodes[curI]);
                }
                // Right link
                if(x<Width)
                {
                    int curI = (x+1) * Width + z;
                    Nodes[idx].GetComponent<Node>().AddNeighbor(Nodes[curI]);
                }
                // Left link
                if(z>0)
                {
                    int curI = (x-1) * Width + z;
                    Nodes[idx].GetComponent<Node>().AddNeighbor(Nodes[curI]);
                }
            }
        }
    }
}
