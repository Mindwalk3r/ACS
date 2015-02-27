using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Directions
{
	bool[] allowedDirections;
	MapTile[] possibleDirections;
	float[] pDirection;
    int[] indexes;
    List<PosPoint> prevVisited;
	float powerFactorAlfa;
	float powerFactorBeta;

	public Directions(int xpos, int ypos, List<PosPoint> visited)
	{
        prevVisited = visited;
		powerFactorAlfa = 1.0f;
		powerFactorBeta = 1.0f;
		pDirection = new float[4];
        indexes = new int[8];
        indexes[0] = xpos;
        indexes[1] = xpos+1;
        indexes[2] = xpos;
        indexes[3] = xpos-1;
        indexes[4] = ypos+1;
        indexes[5] = ypos;
        indexes[6] = ypos-1;
        indexes[7] = ypos;
		possibleDirections = new MapTile[4];
		possibleDirections[0] = MazeCreation.tiles[xpos, ypos + 1]; //Element 0 means the upper element
		possibleDirections[1] = MazeCreation.tiles[xpos + 1, ypos]; //Right
		possibleDirections[2] = MazeCreation.tiles[xpos, ypos - 1]; //down
		possibleDirections[3] = MazeCreation.tiles[xpos - 1, ypos]; //left
		allowedDirections = new bool[4];
		for (int i = 0; i < 4; i++)
		{
			if (possibleDirections[i].Blocked)
			{
				allowedDirections[i] = false;
			}
			else
			{
                allowedDirections[i] = searchDirection(indexes[i], indexes[i+4]);
			}
		}
	}
	
	public float[] CalcDirection()
	{       
		for (int i = 0; i < 4; i++)
		{    
			if (allowedDirections[i] == true)
			{
				float lowerDivision = 0.0f;
				float upperDivison = Mathf.Pow(1 + possibleDirections[i].PheromoneCount, powerFactorAlfa) * Mathf.Pow(1, powerFactorBeta);

				for (int j = 0; j < 4; j++)
				{
					if (allowedDirections[j] == true)
					{
						lowerDivision += Mathf.Pow(1 + possibleDirections[j].PheromoneCount, powerFactorAlfa) * Mathf.Pow(1, powerFactorBeta);
					}
				}
				pDirection[i] = upperDivison / lowerDivision;
			}
		}
		return pDirection;
	}

    public bool searchDirection(int xpos, int ypos)
    {
        for (int i = 0; i < prevVisited.Count - 1; i++)
        {
            if (xpos == prevVisited[i].X && ypos == prevVisited[i].Y)
            {
                return false;
            }
        }
        return true;
    }
}
