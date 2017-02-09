using UnityEngine;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

public interface WeightedGraph<L>
{
    double Cost(Vector3 a, Vector3 b);

    IEnumerable<Vector3> Neighbors(Vector3 id);
}

public class SquareGrid : WeightedGraph<Vector3>
{
    // Implementation notes: I made the fields public for convenience,
    // but in a real project you'll probably want to follow standard
    // style and make them private.

    public static readonly Vector3[] DIRS = new[]
    {
        new Vector3(1, 0),
        new Vector3(0, -1),
        new Vector3(-1, 0),
        new Vector3(0, 1)
    };

    public int width, height;
    public HashSet<Vector3> walls = new HashSet<Vector3>();
    public HashSet<Vector3> forests = new HashSet<Vector3>();

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool InBounds(Vector3 id)
    {
        return 0 <= id.x && id.x < width && 0 <= id.y && id.y < height;
    }

    public bool Passable(Vector3 id)
    {
        return !walls.Contains(id);
    }

    //Use this as with the pedestrianCrossing
    public double Cost(Vector3 a, Vector3 b)
    {
        return forests.Contains(b) ? 5 : 1;
    }

    public IEnumerable<Vector3> Neighbors(Vector3 id)
    {
        foreach (var dir in DIRS)
        {
            Vector3 next = new Vector3(id.x + dir.x, id.y + dir.y);
            if (InBounds(next) && Passable(next))
            {
                yield return next;
            }
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
    public Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
    public Dictionary<Vector3, double> costSoFar = new Dictionary<Vector3, double>();
    
    static public double Heuristic(Vector3 a, Vector3 b)
    {
        return System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y);
    }

    public AStarSearch(WeightedGraph<Vector3> graph, Vector3 start, Vector3 goal)
    {
        var frontier = new PriorityQueue<Vector3>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (var next in graph.Neighbors(current))
            {
                double newCost = costSoFar[current] + graph.Cost(current, next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    //Debug.Log("next:                  [X: " + next.x + "] [Y: " + next.y + "]");
                    //Debug.Log("cameFrom[next]: [X: " + cameFrom[next].x + "] [Y: " + cameFrom[next].y + "]");
                }
            }
        }
    }
}