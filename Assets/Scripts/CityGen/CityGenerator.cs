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
    private bool mInitialized = false;
    // Should the city be initialized at the start?
    public bool InitOnStart = false;
    public int Width = 10;
    public float NodeSeparation = 5.0f;
    // The width of the buildings, this value will be clamped checking 
    // the node separation
    public float BuildingWidth = 25.0f;
    public float BuildingHeight = 200.0f;
    public GameObject Node;
    public List<GameObject> CarNodes;
    public List<GameObject> PedestrianNodes;
    public GameObject BuildingBasic;
    public GameObject CrossWalkHorizontal;
    public GameObject CrossWalkVertical;

    public float StreetSize = 32.0f;
    public float CrossWalkHeight = 0.4f;
    public float CrossWalkWidth = 5.0f;
    public float CrossWalkLenghtMod = 0.85f;

    private GameObject mPedestrianRoot;
    private GameObject mCarRoot;
    private GameObject mBuildingsRoot;

    void Start()
    {
        if(!mInitialized && InitOnStart)
        {
            //InitializeCity();
        }

        // Clamp building width
        if(BuildingWidth >= NodeSeparation)
        {
            ////Debug..LogWarning("The provided building width is too big, clamping it down.");
            BuildingWidth = NodeSeparation / 1.5f;
        }
    }

    /// <summary>
    /// Initializes the city. Generates the buildings and the graphs.
    /// </summary>
    public void InitializeCity()
    {
        ////Debug..Log("Initializing city...");

        // Initialzed check
        if(mInitialized)
        {
            ////Debug..LogWarning("You are trying to initialize the city while it is already generated nub.");
            return;
        }
        mInitialized = true;

        Random.InitState(System.DateTime.Now.Millisecond);

        // Create root nodes
        mPedestrianRoot = new GameObject();
        mPedestrianRoot.name = "PedestrianRoot";
        mPedestrianRoot.transform.position = Vector3.zero;

        mCarRoot = new GameObject();
        mCarRoot.name = "CarRoot";
        mCarRoot.transform.position = Vector3.zero;

        mBuildingsRoot = new GameObject();
        mBuildingsRoot.name = "BuildingsRoot";
        mBuildingsRoot.transform.position = Vector3.zero;

        // Initialize the nodes
        InitializeCarNodes();
        InitializePedestrianNodes();

        // Generate the city
        AddBuildings();

        // Fill the graphs
        FillGraph(ref WaypointsExample.CarsGraph, ref CarNodes);
        FillGraph(ref WaypointsExample.PedestriansGraph, ref PedestrianNodes);
    }

    void AddBuildings()
    {
        float separationHalf = NodeSeparation * 0.5f;
        for (int z = -1; z < Width+1; z++)
        {
            for (int x = -1; x < Width+1; x++)
            {
                Vector3 bPos = new Vector3( (x * NodeSeparation) + separationHalf,
                                            0.0f,
                                            (z * NodeSeparation)+ separationHalf);
                GameObject b = GameObject.Instantiate(BuildingBasic, bPos, Quaternion.identity) as GameObject;
                b.transform.parent = mBuildingsRoot.transform;
                float h = (Mathf.PerlinNoise(z * 10.5f, x * 10.5f) * BuildingHeight);
                float w = Random.Range(BuildingWidth, BuildingWidth);
                //////Debug..Log(h + "," + w);
                b.GetComponent<MeshFilter>().mesh = GeometryGen.Instance.GenBuilding(h, w);

                GameObject baseStreet = GameObject.CreatePrimitive(PrimitiveType.Cube);
                baseStreet.transform.position = bPos;
                baseStreet.transform.localScale = new Vector3(StreetSize, 0.1f, StreetSize);
            }
        }
    }

    void FillGraph(ref Graph graph, ref List<GameObject> nodes)
    {
        // For each generated node
        for (int i = 0; i < nodes.Count; i++)
        {
            Node n = nodes[i].GetComponent<Node>();
            if (!n) Debug.LogWarning("This object does not have a node component.");

            // Build a list of the neighbours of this node
            Dictionary<Vector3, double> nb = new Dictionary<Vector3, double>();
            for (int j = 0; j < n.Neighbors.Count; j++)
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
        // We add one to the width so the road goes also
        // outside of the city on the top/top left borders
        int carWidth = Width + 1;
        CarNodes.Clear();
        for (int x = 0; x < carWidth; x++) 
        {
            for (int z = 0; z < carWidth; z++) 
            {
                GameObject n = GameObject.Instantiate(Node);
                n.transform.parent = mCarRoot.transform;
                n.name = "Node_" + x.ToString() + "_" + z.ToString();
                n.transform.position = new Vector3(x * NodeSeparation, 0.0f, z * NodeSeparation);
                n.GetComponent<Node>().DebugColor = Color.red;
                CarNodes.Add(n);
            }
        }

        // Link the nodes
        for (int z = 0; z < carWidth; z++)
        {
            for (int x = 0; x < carWidth; x++)
            {
                int idx = x * carWidth + z;
                // Top link 
                if (z < carWidth - 1)
                {
                    int curI = x * carWidth + (z + 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Bottom link
                if (z > 0)
                {
                    int curI = x * carWidth + (z - 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Right link
                if (x < carWidth - 1)
                {
                    int curI = (x + 1) * carWidth + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Left link
                if (x > 0)
                {
                    int curI = (x - 1) * carWidth + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = CarNodes[curI];
                    nn.Distance = Vector3.Distance(CarNodes[idx].transform.position,
                                                    CarNodes[curI].transform.position);
                    CarNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
            }
        }
    }

    /// <summary>
    /// We iterate through the grid nodes and generate pedestrian links. For each
    /// iteration we generate bottom row and then we generate the top row
    /// </summary>
    void InitializePedestrianNodes()
    {
        // Fill nodes
        PedestrianNodes.Clear();
        for (int z = 0; z < Width; z++)
        {
            FillPedestrianBottomNodes(z);
            FillPedestrianTopNodes(z);
            
        }
        for (int z = 0; z < Width; z++)
        {
            AddCrossWalks(z);
        }

        // Link Nodes
        int width2 = Width * 2;
        for (int z = 0; z < width2; z++)
        {
            for (int x = 0; x < width2; x++)
            {
                int idx = x * width2 + z;
                // Top link 
                if (z < width2 - 1)
                {
                    int curI = x * width2 + (z + 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Bottom link
                if (z > 0)
                {
                    int curI = x * width2 + (z - 1);
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Right link
                if (x < width2 - 1)
                {
                    int curI = (x + 1) * width2 + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
                // Left link
                if (x > 0)
                {
                    int curI = (x - 1) * width2 + z;
                    NodeNeighbors nn = new NodeNeighbors();
                    nn.OtherNode = PedestrianNodes[curI];
                    nn.Distance = Vector3.Distance(PedestrianNodes[idx].transform.position,
                                                    PedestrianNodes[curI].transform.position);
                    PedestrianNodes[idx].GetComponent<Node>().AddNeighbor(nn);
                }
            }
        }
    }

    void FillPedestrianBottomNodes(int z)
    {
        Vector3 nodeOff = new Vector3(BuildingWidth, 0.0f, BuildingWidth) * 1.25f;
        float separationHalf = NodeSeparation * 0.5f;

        for (int x = 0; x < Width; x++)
        {
            Vector3 bPos = new Vector3( (x * NodeSeparation) + separationHalf,
                                        0.0f,
                                        (z * NodeSeparation) + separationHalf);
            GameObject n;

            // Bl node
            n = GameObject.Instantiate(Node);
            n.transform.parent = mPedestrianRoot.transform;
            n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
            n.transform.position = new Vector3(bPos.x - nodeOff.x, 0.0f, bPos.z - nodeOff.z);
            n.GetComponent<Node>().DebugColor = Color.blue;
            n.GetComponent<Node>().DebugSize = 0.25f;
            PedestrianNodes.Add(n);

            // Br node
            n = GameObject.Instantiate(Node);
            n.transform.parent = mPedestrianRoot.transform;
            n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
            n.transform.position = new Vector3(bPos.x + nodeOff.x, 0.0f, bPos.z - nodeOff.z);
            n.GetComponent<Node>().DebugColor = Color.blue;
            n.GetComponent<Node>().DebugSize = 0.25f;
            PedestrianNodes.Add(n);
        }
    }

    void FillPedestrianTopNodes(int z)
    {
        Vector3 nodeOff = new Vector3(BuildingWidth, 0.0f, BuildingWidth) * 1.25f;
        float separationHalf = NodeSeparation * 0.5f;

        for (int x = 0; x < Width; x++)
        {
            Vector3 bPos = new Vector3((x * NodeSeparation) + separationHalf,
                            0.0f,
                            (z * NodeSeparation) + separationHalf);
            GameObject n;

            // Tl node
            n = GameObject.Instantiate(Node);
            n.transform.parent = mPedestrianRoot.transform;
            n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
            n.transform.position = new Vector3(bPos.x - nodeOff.x, 0.0f, bPos.z + nodeOff.z);
            n.GetComponent<Node>().DebugColor = Color.blue;
            n.GetComponent<Node>().DebugSize = 0.25f;
            PedestrianNodes.Add(n);

            // Tr node
            n = GameObject.Instantiate(Node);
            n.transform.parent = mPedestrianRoot.transform;
            n.name = "PedestrianNode_" + bPos.x.ToString() + "_" + bPos.z.ToString();
            n.transform.position = new Vector3(bPos.x + nodeOff.x, 0.0f, bPos.z + nodeOff.z);
            n.GetComponent<Node>().DebugColor = Color.blue;
            n.GetComponent<Node>().DebugSize = 0.25f;
            PedestrianNodes.Add(n);
        }
    }

    void AddCrossWalks(int z)
    {
        float separationHalf = NodeSeparation * 0.5f;
        float separationQuart = NodeSeparation * 0.25f;

        for (int x = 0; x < Width; x++)
        {
            Vector3 cwPos  = new Vector3((x * NodeSeparation),
                                        0.0f,
                                        (z * NodeSeparation) + separationQuart);
            Vector3 cwPos2 = new Vector3((x * NodeSeparation),
                                        0.0f,
                                        (z * NodeSeparation) + separationHalf + separationQuart);

            int vertX = x;

            Vector3 cwPosTop = new Vector3((vertX * NodeSeparation) + separationQuart + separationHalf,
                                            0.0f,
                                            (z * NodeSeparation) + separationHalf + separationHalf);
            Vector3 cwPosTop2 = new Vector3((vertX * NodeSeparation) + separationQuart,
                                            0.0f,
                                            (z * NodeSeparation) + separationHalf + separationHalf);
            GameObject cw;
            Vector3 cwCurScale;

            // Horizontal crosswalks
            if (x != 0)
            {
                // Horizontal bottom
                cw = GameObject.Instantiate(CrossWalkHorizontal, cwPos, Quaternion.identity) as GameObject;
                cwCurScale = cw.transform.localScale;
                cwCurScale.x = separationHalf * CrossWalkLenghtMod;
                cwCurScale.y = CrossWalkHeight;
                cwCurScale.z = CrossWalkWidth;
                cw.transform.localScale = cwCurScale;

                // Horizontal top
                cw = GameObject.Instantiate(CrossWalkHorizontal, cwPos2, Quaternion.identity) as GameObject;
                cwCurScale = cw.transform.localScale;
                cwCurScale.x = separationHalf * CrossWalkLenghtMod;
                cwCurScale.y = CrossWalkHeight;
                cwCurScale.z = CrossWalkWidth;
                cw.transform.localScale = cwCurScale;
            }

            // Vertical Crosswalk
            if (z < Width - 1)
            {
                // Vertical cw left
                cw = GameObject.Instantiate(CrossWalkVertical, cwPosTop2, Quaternion.identity) as GameObject;
                cwCurScale = cw.transform.localScale;
                cwCurScale.x = CrossWalkWidth;
                cwCurScale.y = CrossWalkHeight;
                cwCurScale.z = separationHalf * CrossWalkLenghtMod;
                cw.transform.localScale = cwCurScale;
                
                // Vertical cw right
                cw = GameObject.Instantiate(CrossWalkVertical, cwPosTop, Quaternion.identity) as GameObject;
                cwCurScale = cw.transform.localScale;
                cwCurScale.x = CrossWalkWidth;
                cwCurScale.y = CrossWalkHeight;
                cwCurScale.z = separationHalf * CrossWalkLenghtMod;
                cw.transform.localScale = cwCurScale;
                
            }
        }
    }
}

