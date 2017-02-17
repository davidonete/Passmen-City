using UnityEngine;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {
    void Start () {}
	void Update () {}
}

public class NodeGraph
{
    public HashSet<Vector3> pedestrian = new HashSet<Vector3>();

    private List<GameObject> mGraph;

    public NodeGraph(List<GameObject> graph)
    {
        this.mGraph = graph;
    }
     
    //Use this as with the pedestrianCrossing
    public double Cost(Vector3 a, Vector3 b)
    {
        return pedestrian.Contains(b) ? 5 : 1;
    }
    
    public IEnumerable<NodeNeighbors> Neighbors(GameObject node)
    {
        foreach (NodeNeighbors neighbor in node.GetComponent("Node").Neighbors)
        {
            yield return neighbor;
        }
    }
}

public class PriorityQueue<Vector3>
{
    public class Tuple
    {
        Vector3 mItem1;
        double mItem2;

        public Tuple Create (Vector3 item1, double item2)
        {
            mItem1 = item1;
            mItem2 = item2;
            return this;
        }

        public Vector3 Item1()
        {
            return mItem1;
        }
        public double Item2()
        {
            return mItem2;
        }
    }
    
    private List<Tuple> elements = new List<Tuple>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(Vector3 item, double priority)
    {
        elements.Add(new Tuple().Create(item, priority));
    }

    public Vector3 Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2() < elements[bestIndex].Item2())
            {
                bestIndex = i;
            }
        }

        Vector3 bestItem = elements[bestIndex].Item1();
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}

public class AStarSearch
{
    public Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, double> costSoFar = new Dictionary<GameObject, double>();

    static public double Heuristic(Vector3 a, Vector3 b)
    {
        return System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);
    }
    
    public AStarSearch(NodeGraph graph, NodeNeighbors start, NodeNeighbors goal)
    {
        var frontier = new PriorityQueue<NodeNeighbors>();
        frontier.Enqueue(start, 0);

        cameFrom[start.OtherNode] = start.OtherNode;
        costSoFar[start.OtherNode] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (var next in graph.Neighbors(current.OtherNode))
            {
                double newCost = costSoFar[current.OtherNode] + next.Distance; //graph.Cost(current.OtherNode, next.OtherNode);
                if (!costSoFar.ContainsKey(next.OtherNode) || newCost < costSoFar[next.OtherNode])
                {
                    costSoFar[next.OtherNode] = newCost;
                    double priority = newCost + Heuristic(next.OtherNode.transform.position, goal.OtherNode.transform.position);
                    frontier.Enqueue(next, priority);
                    cameFrom[next.OtherNode] = current.OtherNode;
                    //Debug.Log("next:                  [X: " + next.OtherNode.x + "] [Y: " + next.OtherNode.y + "]");
                    //Debug.Log("cameFrom[next.OtherNode]: [X: " + cameFrom[next.OtherNode].x + "] [Y: " + cameFrom[next.OtherNode].y + "]");
                }
            }
        }
    }
}