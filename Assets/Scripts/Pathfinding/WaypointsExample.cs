using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This file has been created just to test while the waypoints are not working
public class WaypointsExample : MonoBehaviour
{
    public static SquareGrid grid = new SquareGrid(10, 10);

    // Use this for initialization
    void Start()
    {
        /*for (var x = 5; x < 7; x++)
        {
            for (var y = 5; y < 7; y++)
            {
                grid.walls.Add(new Vector3(x, y));
            }
        }*/

        /*grid.forests = new HashSet<Vector3>
        {
            new Vector3(3, 4), new Vector3(3, 5),
            new Vector3(4, 1), new Vector3(4, 2),
            new Vector3(4, 3), new Vector3(4, 4),
            new Vector3(4, 5), new Vector3(4, 6),
            new Vector3(4, 7), new Vector3(4, 8),
            new Vector3(5, 1), new Vector3(5, 2),
            new Vector3(5, 3), new Vector3(5, 4),
            new Vector3(5, 5), new Vector3(5, 6),
            new Vector3(5, 7), new Vector3(5, 8),
            new Vector3(6, 2), new Vector3(6, 3),
            new Vector3(6, 4), new Vector3(6, 5),
            new Vector3(6, 6), new Vector3(6, 7),
            new Vector3(7, 3), new Vector3(7, 4),
            new Vector3(7, 5)
        };*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}