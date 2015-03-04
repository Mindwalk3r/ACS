using UnityEngine;
using System.Collections;

public class MapTile {	
	public GameObject obj;
	public Color tileColor;
	public float lastTraversed;
	public int numOfPlacedPhermone;
	public float PheromoneCount { get; set; }
	public bool Blocked { get; set; }
	public bool isStart;
	public bool isGoal;
	public bool traversed;
	
	public MapTile(float pheromonecount, bool blocked, bool start = false, bool goal = false)
	{
		this.PheromoneCount = pheromonecount;
		this.Blocked = blocked;
		isStart = start;
		isGoal = goal;

		if (blocked)
			tileColor = Color.gray;
		else if (start)
			tileColor = Color.red;
		else if (goal)
			tileColor = Color.cyan;
		else 
			tileColor = Color.white;
	}

	public void resetTile() {
		this.traversed = false;
		this.numOfPlacedPhermone = 0;
		this.PheromoneCount = 0f;
		this.lastTraversed = 0f;
	}
}
