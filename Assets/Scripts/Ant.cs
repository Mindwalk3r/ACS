using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PosPoint
{
    public int X { get; set; }
    public int Y { get; set; }
    public PosPoint(int x, int y)
    {
    	this.X = x;
        this.Y = y;
    }
}

public class Ant
{
	public int posX;
	public int posY;
	public bool alive;
	public bool finished;
	public bool reachedFood;
	public GameObject obj;
	public List<PosPoint> recordMap;
	public int mapSize = MazeCreation.mapSize;

	public Ant(int x, int y)
	{
		posX = x;
		posY = y;
        recordMap = new List<PosPoint>();
	}

	public void resetAnt() {
		Vector3 pos = this.obj.transform.position;
		pos.x = 1;
		pos.y = mapSize - 2;
		
		this.recordMap.Clear();
		this.finished = false;
		this.reachedFood = false;
		this.posX = 1;
		this.posY = mapSize - 2;
		this.obj.transform.position = pos;
	}

	public void disableAnt() {
		this.obj.SetActive(false);
		this.alive = false;
	}
}