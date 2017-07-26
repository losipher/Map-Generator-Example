using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator instance = null;
	[Range(0,100)]
	public int randomFillPercent;
	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	int[,] map;

	public GameObject[] groundTiles;

	private Transform mapHolder;

	void Aeake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start(){
		GenerateMap ();
		LevelSetup ();
	}

	void LevelSetup()
	{
		mapHolder = new GameObject ("Map").transform;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (map [x, y] == 1) {
					
					GameObject toInstantiate = groundTiles [Random.RandomRange (0, groundTiles.Length)];

					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

					instance.transform.SetParent (mapHolder);
				}

			}
		}
	}

	void GenerateMap(){
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < 5; i++) {
			SmoothMap ();
		}
	}

	void RandomFillMap(){
		if (useRandomSeed)
			seed = Time.time.ToString ();

		System.Random psuedoRandom = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				/*if (x == 0 || x == width - 1 || y == 0 || y == (height - 1) / 2) {
					map [x, y] = 1;
				}
				if (y > (height - 1) / 2) {
					map [x, y] = 0;
				}
				else {
					map [x, y] = psuedoRandom.Next (0, 100) < randomFillPercent ? 1 : 0;
				}*/

				map [x, y] = psuedoRandom.Next (0, 100) < randomFillPercent ? 1 : 0;
			}
		}			
	}

	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingEallCount (x, y);

				if (neighbourWallTiles > 12)
					map [x, y] = 1;
				else if (neighbourWallTiles <= 12)
					map [x, y] = 0;

				if (y == 0 && neighbourWallTiles < 5)
					map [x, y] = 1;

				if (y == height - 1)
					map [x, y] = 0;

				if ((x == 0 || x == width - 1) && neighbourWallTiles > 10)
					map [x, y] = 0;
			}
		}
				
	}

	int GetSurroundingEallCount(int gridX, int gridY){
		int wallCount = 0;
		for (int neighbourX = gridX - 2; neighbourX <= gridX + 2; neighbourX++) {
			for (int neighbourY = gridY - 2; neighbourY <= gridY + 2; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map [neighbourX, neighbourY];
					}
				} else {//This by default cases edge walls of the map to grow. Might need to change this for the platforming idea.
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	/*void OnDrawGizmos(){
		if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3 (-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);
					Gizmos.DrawCube (pos, Vector3.one);
				}
			}
		}
	}*/
}
