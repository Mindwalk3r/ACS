using UnityEngine;
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

			var directionR = Random.Range (0.0f, 3.9f);
            var directionP = Random.Range(0.00f, 100.00f);
			int dir = (int)directionR;
            int amountAllowed;
            float probabilitySum;
            float[] probabilityDirection; //0 = up, 1 = right, 2 = down, 3 = left
            
			pos.x = originalPos.x;
			pos.y = originalPos.y;
            d1 = new Directions((int)pos.x, (int)pos.y, ants[i].recordMap);
            
            probabilityDirection = d1.CalcDirection();
            //Debug.Log(probabilityDirection[0].ToString());
            //Debug.Log(probabilityDirection[1].ToString());
            //Debug.Log(probabilityDirection[2].ToString());
            //Debug.Log(probabilityDirection[3].ToString());
            
            

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
				//ants[i].disableAnt();
				ants[i].resetAnt();
				//numAntsAlive--;
			}


			tiles[(int)pos.x, (int)pos.y].lastVisited = Time.time;


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
		//for (int i = 0; i < numAnts; i++) {
			foreach (PosPoint point in ants[i].recordMap) {
				tiles [point.X, point.Y].numOfPlacedPhermone++;
				tiles [point.X, point.Y].traversed = true;
			}
			
			foreach (MapTile tile in tiles) {
				if (tile.traversed) {
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


		//}
	}
	                           
	private void activateAllAnts() {
		for (int i = 0; i < numAnts; i++) {
			ants[i].alive = true;
			ants[i].obj.SetActive(true);
		}
	}
}