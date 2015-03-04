using UnityEngine;
using System.Collections;

public class MazeCreation : MonoBehaviour
{
	public GameObject tile;
	private GameObject pressedTile;
	public static int mapSize = 20;
	private float offset = 1f;
	public static MapTile[,] tiles = new MapTile[mapSize, mapSize];

    public void Start()
    {
		int rand;
		int startX = 1;
		int startY = mapSize - 2;
		int goalX = mapSize - 2;
		int goalY = 1; //mapSize -2;
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if (i == 0 || i == mapSize - 1 || j == 0 || j == mapSize - 1) {
					tiles[i, j] = new MapTile(0, true);
				} else if (i == startX && j == startY) {
					tiles[i, j] = new MapTile(0, false, true);
				} else if (i == goalX && j == goalY) {
					tiles[i, j] = new MapTile(0, false, false, true);
				}
				else {
					rand = Random.Range(0, 100);
					if (rand > 10)
						tiles[i, j] = new MapTile(0, false);
					else
						tiles[i, j] = new MapTile(0, true);
				}

				tiles[i,j].obj = (GameObject)Instantiate(tile, new Vector3(i*offset, j*offset, 0), Quaternion.identity);
				tiles[i,j].obj.GetComponent<SpriteRenderer>().color = tiles[i,j].tileColor;
			}
		}
		pressedTile = (GameObject)Instantiate(tile, new Vector3(0, 0, 10), Quaternion.identity);
	}

	public void FixedUpdate() {
		Color newColor;
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if (!tiles[i, j].Blocked && !tiles[i, j].isGoal && !tiles[i, j].isStart && tiles[i, j].lastTraversed != 0 && tiles[i, j].lastTraversed + 4 < Time.time) {
					tiles[i,j].PheromoneCount = (1f - 0.005f) * tiles[i,j].PheromoneCount;
					if (tiles[i,j].numOfPlacedPhermone != 0) tiles[i,j].numOfPlacedPhermone--;
					newColor = new Color (1f - (0.01f * tiles[i, j].PheromoneCount), 1f, 1f - (0.01f * tiles[i, j].PheromoneCount), 1);
					tiles[i, j].obj.GetComponent<SpriteRenderer> ().color = newColor;
				}
			}
		}
	}

	public void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 tilePos = pressedTile.transform.position;
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hitTile = Physics2D.OverlapPoint (mousePos);

			if (hitTile) {
				tilePos.x = hitTile.transform.position.x;
				tilePos.y = hitTile.transform.position.y;    
				if ((int)tilePos.x > 0 && (int)tilePos.x < mapSize-1 && (int)tilePos.y > 0 && (int)tilePos.y < mapSize-1) {
					if (!tiles[(int)tilePos.x, (int)tilePos.y].Blocked && !tiles[(int)tilePos.x, (int)tilePos.y].isGoal && !tiles[(int)tilePos.x, (int)tilePos.y].isStart) {
						tiles[(int)tilePos.x, (int)tilePos.y].obj.GetComponent<SpriteRenderer>().color = Color.gray;
						tiles[(int)tilePos.x, (int)tilePos.y].Blocked = true;
						tiles[(int)tilePos.x, (int)tilePos.y].resetTile();
					} else if (tiles[(int)tilePos.x, (int)tilePos.y].Blocked && !tiles[(int)tilePos.x, (int)tilePos.y].isGoal && !tiles[(int)tilePos.x, (int)tilePos.y].isStart) {
						tiles[(int)tilePos.x, (int)tilePos.y].obj.GetComponent<SpriteRenderer>().color = Color.white;
						tiles[(int)tilePos.x, (int)tilePos.y].Blocked = false;
						tiles[(int)tilePos.x, (int)tilePos.y].resetTile();
					}
				}
			}

		}
	}
}
  