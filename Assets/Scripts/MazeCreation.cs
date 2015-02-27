using UnityEngine;
using System.Collections;

public class MazeCreation : MonoBehaviour
{
	public GameObject tile;
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
	}

	public void FixedUpdate() {
		//Set tile color
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				//if (!tiles[i,j].Blocked && !tiles[i,j].isGoal && !tiles[i,j].isStart && tiles[i,j].lastVisited + 1 <= Time.time)
					//tiles[i, j].obj.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}
}
  