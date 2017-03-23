using UnityEngine;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
  void Start() { }
  void Update() { }
}

public class Graph
{
  private Dictionary<Vector3, Dictionary<Vector3, double>> mGraph = new Dictionary<Vector3, Dictionary<Vector3, double>>();

  public Graph() { }

  public void AddNodeToGraph(Vector3 position, Dictionary<Vector3, double> neighbors)
  {
    mGraph.Add(position, neighbors);
  }
  
  public double Cost(Vector3 a, Vector3 b)
  {
    return mGraph[a][b];
  }

  public IEnumerable<Vector3> Nodes()
  {
    foreach (Vector3 node in mGraph.Keys)
    {
      yield return node;
    }
  }

  public int NodesCount()
  {
    return mGraph.Count;
  }

  public IEnumerable<Vector3> Neighbors(Vector3 node)
  {
    foreach (Vector3 neighbor in mGraph[node].Keys)
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

    public Tuple Create(Vector3 item1, double item2)
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
	public Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	public Dictionary<Vector3, double> costSoFar = new Dictionary<Vector3, double>();

	static public double Heuristic(Vector3 a, Vector3 b)
	{
		return System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);
	}

	public AStarSearch(Graph graph, Vector3 start, Vector3 goal)
	{
		PriorityQueue<Vector3> frontier = new PriorityQueue<Vector3>();
		frontier.Enqueue(start, 0);

		cameFrom[start] = start;
		costSoFar[start] = 0;

		while (frontier.Count > 0)
		{
			Vector3 current = frontier.Dequeue();

			if (current.Equals(goal))
			{
				break;
			}

			foreach (Vector3 next in graph.Neighbors(current))
			{
				double newCost = costSoFar[current] + graph.Cost(current, next);
				if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
				{
					costSoFar[next] = newCost;
					double priority = newCost + Heuristic(next, goal);
					frontier.Enqueue(next, priority);
					cameFrom[next] = current;
				}
			}
		}
	}

	public static List<Vector3> FindNewObjective(Graph graph, Vector3 start, Vector3 end)
	{
		List<Vector3> path = new List<Vector3>();
		if (graph.NodesCount() > 0)
		{
			AStarSearch AStar = new AStarSearch(graph, start, end);

			//Debug.Log("Start: " + start + " | End: " + end);
			Vector3 nextStep = end;

			while (Vector3.Distance(start, nextStep) > 0.1f)
			{
				path.Add(nextStep);
				//Debug.Log("Next " + nextStep);
				nextStep = AStar.cameFrom[nextStep];
			}
			//Debug.Log("**********************");
		}
		return path;
	}

  public static Vector3 GetNearestWaypoint(Graph graph, Vector3 position)
  {
    Vector3 nearestNode = position;
    float nearestDistance = 9999.0f;
    foreach (Vector3 node in graph.Nodes())
    {
      float distance = Vector3.Distance(node, position);
      if (distance < nearestDistance)
      {
        nearestNode = node;
        nearestDistance = distance;
      }
    }
    return nearestNode;
  }

  //TODO: Optimize this function
  public static Vector3 GetRandomWaypoint(Graph graph)
  {
    int rand = Random.Range(0, graph.NodesCount() + 1);
    int index = 0;
    foreach (var node in graph.Nodes())
    {
      index++;
      if (index == rand)
      {
        return node;
      }
    }
    return new Vector3(0, 0, 0);
  }
}