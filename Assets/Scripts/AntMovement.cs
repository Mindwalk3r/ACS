﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntMovement : MonoBehaviour
{
    public GameObject antPrefab;
	public Ant[] ants;
	public MapTile[,] tiles = MazeCreation.tiles;
	public int mapSize = MazeCreation.mapSize;

	private float p = 0.6f;
	private float c = 1f;
	private float maxPhermone = 150f;

	private int numAntsAlive;
	private int numAnts = 50;
	private Color newColor = new Color();

    void Awake()
    {
		ants = new Ant[numAnts];
		numAntsAlive = numAnts;
		for (int i = 0; i < numAnts; i++) {
			ants[i] = new Ant(1, mapSize-2);
			ants[i].obj = (GameObject) Instantiate(antPrefab, new Vector3 (ants[i].posX, ants[i].posY, -10), Quaternion.identity);
			ants[i].recordMap.Add(new PosPoint(ants[i].posX,ants[i].posY));
		}
		this.activateAllAnts();
	}

	void FixedUpdate() {
        Directions d1;
        PosPoint p1;
		for (int i = 0; i < numAnts; i++) {
			if (!ants[i].alive)
				continue;

			Vector3 pos = ants[i].obj.transform.position;
			Vector3 originalPos = ants[i].obj.transform.position;
            var directionP = Random.Range(0.00f, 100.00f);

            float probabilitySum;
            float[] probabilityDirection; //0 = up, 1 = right, 2 = down, 3 = left
            
			pos.x = originalPos.x;
			pos.y = originalPos.y;
            d1 = new Directions((int)pos.x, (int)pos.y, ants[i].recordMap);
            
            probabilityDirection = d1.CalcDirection();

            //Probability movement
            probabilitySum = probabilityDirection[0] + probabilityDirection[1] + probabilityDirection[2] + probabilityDirection[3];
			bool reset = false;
			if (probabilitySum != 0)
            {

                if (directionP < probabilityDirection[0] * 100)
                {
                    pos.y += 1;
                }
                else if (directionP < (probabilityDirection[0] + probabilityDirection[1]) * 100)
                {
                    pos.x += 1;
                }
                else if (directionP < (probabilityDirection[0] + probabilityDirection[1] + probabilityDirection[2]) * 100)
                {
                    pos.y -= 1;
                }
                else
                {
                    pos.x -= 1;
                }          
            }
			else {
				reset = true;
			}


			if (!reset) {
				ants[i].obj.transform.position = pos;
				ants[i].posX = (int)pos.x; //Might not be needed
				ants[i].posY = (int)pos.y; //Might not be needed
	            p1 = new PosPoint(ants[i].posX, ants[i].posY);
				ants[i].recordMap.Add(p1);
			} else {
				//No valid path found, reset ant
				ants[i].resetAnt();
			}

			if (tiles[(int)pos.x, (int)pos.y].isGoal) {
				ants[i].disableAnt();
				this.updatePhermone(i);
				ants[i].resetAnt();
				numAntsAlive--;
			}
		}
		if (numAntsAlive == 0) {
			this.activateAllAnts();	
			numAntsAlive = numAnts;
		}
	}

	private void updatePhermone(int i) {
		foreach (PosPoint point in ants[i].recordMap) {
			tiles[point.X, point.Y].numOfPlacedPhermone++;
			tiles[point.X, point.Y].traversed = true;
			tiles[point.X, point.Y].lastTraversed = Time.time;


			//------------------------------------------------------------------- Implementation for elitist ant colony

			List<PosPoint> lastGlobalTour = new List<PosPoint>();
			if (MazeCreation.BestGlobalTour.Count == 0)
			{
				foreach (PosPoint temp in ants[i].recordMap) 
				{
					MazeCreation.BestGlobalTour.Add (temp);
				}
				foreach (PosPoint position in MazeCreation.BestGlobalTour)
				{
					tiles[position.X, position.Y].PheromoneCount = (1 - p) * tiles[position.X, position.Y].PheromoneCount + tiles[position.X, position.Y].numOfPlacedPhermone + (100 - MazeCreation.BestGlobalTour.Count) * c;
					//tiles[position.X, position.Y].obj.GetComponent<SpriteRenderer>().color = Color.red;
				}
				Debug.Log("New starting tour found");
				Debug.Log("TileCount: " + MazeCreation.BestGlobalTour.Count);
			}
			else if (MazeCreation.BestGlobalTour.Count > ants[i].recordMap.Count)
			{
				Debug.Log ("GlobalTourCount: " + MazeCreation.BestGlobalTour.Count.ToString () + "RecordMapCount: " + ants [i].recordMap.Count.ToString ());

				lastGlobalTour = MazeCreation.BestGlobalTour;

				//Debug.Log("New tour found");
				//Debug.Log("TileCount: " + MazeCreation.BestGlobalTour.Count + " RecordMapCount: " + ants[i].recordMap.Count);
				MazeCreation.BestGlobalTour.Clear();
				foreach (PosPoint temp in ants[i].recordMap) 
				{
					MazeCreation.BestGlobalTour.Add (temp);
				}
				//MazeCreation.BestGlobalTour = ants[i].recordMap;
				Debug.Log ("GlobalTourCount: " + MazeCreation.BestGlobalTour.Count.ToString () + "RecordMapCount: " + ants [i].recordMap.Count.ToString ());
				//Debug.Break ();
				foreach (MapTile tile in tiles)
				{
					//tile.obj.GetComponent<SpriteRenderer>().color = Color.white;
				}
				foreach (PosPoint position in MazeCreation.BestGlobalTour)
				{
					tiles[position.X, position.Y].PheromoneCount = (1 - p) * tiles[position.X, position.Y].PheromoneCount + tiles[position.X, position.Y].numOfPlacedPhermone + (100 - MazeCreation.BestGlobalTour.Count) * c;
					//tiles[position.X, position.Y].obj.GetComponent<SpriteRenderer>().color = Color.red;
				}
				Debug.Log ("GlobalTourCount: " + MazeCreation.BestGlobalTour.Count.ToString () + "RecordMapCount: " + ants [i].recordMap.Count.ToString ());
				//Debug.Break ();
			}
			if (lastGlobalTour.Count != 0)
			{
				foreach (PosPoint position in lastGlobalTour)
				{
					tiles[position.X, position.Y].PheromoneCount = (1 - p) * tiles[position.X, position.Y].PheromoneCount;
				}
			}
			//---------------------------------------------------------------------- End implementation of elitist any colony
		}
		
		foreach (MapTile tile in tiles) {
			if (tile.traversed && tile.PheromoneCount < maxPhermone) {
				tile.PheromoneCount = (1 - p) * tile.PheromoneCount + tile.numOfPlacedPhermone * c;
				tile.traversed = false;
			} else {
				tile.PheromoneCount = (1 - p) * tile.PheromoneCount;
				if (tile.numOfPlacedPhermone != 0)
					tile.numOfPlacedPhermone--;
			}
			
			newColor = new Color (1f - (0.01f * tile.PheromoneCount), 1f, 1f - (0.01f * tile.PheromoneCount), 1);
			if (!tile.isStart && !tile.isGoal && !tile.Blocked)
				tile.obj.GetComponent<SpriteRenderer> ().color = newColor;
		}
	}
	                           
	private void activateAllAnts() {
		for (int i = 0; i < numAnts; i++) {
			ants[i].alive = true;
			ants[i].obj.SetActive(true);
		}
	}
}