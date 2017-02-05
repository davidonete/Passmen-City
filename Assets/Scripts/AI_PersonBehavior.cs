using UnityEngine;
using System.Collections;

public class AI_PersonBehavior : MonoBehaviour {
	public struct Location {
		private int x, y;
		public int X {
			get { return x; }
			set { x = value; }
		}
		public int Y {
			get { return y; }
			set { y = value; }
		}
	}

	// Use this for initialization
	void Start () {
		//Debug.Log ("Hola");
		Location loc = new Location();
		loc.X = 22;

		//Debug.Log (loc.X);
	}

	// Update is called once per frame
	void Update () {

	}
}
