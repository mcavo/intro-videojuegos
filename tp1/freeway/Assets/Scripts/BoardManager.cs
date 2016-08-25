﻿using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

	public int columns = 18; 										//Number of columns in our game board.
	public int rows = 14;											//Number of rows in our game board.

	// 0 = empty
	// 1 = yellow
	// 2 = both
	// 3 = white
	// 4 = end

	public static float assetsSize = 0.36f;

	public GameObject chicken;										//Player1
	public GameObject chicken2;										//Player2
	public GameObject[] floorTiles;									//Vector with different types of floors.
	public GameObject[] rightCarTiles;								//Vector with differetn types of cars.
	public GameObject[] leftCarTiles;								//Vector with differetn types of cars.

	private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.

	//Sets up the outer walls and floor (background) of the game board.
	void BoardSetup () {
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;

		for(int x = 0; x < columns; x++) {
			//Asign empty floors = 0
			//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
			GameObject toInstantiate = floorTiles[0];
			//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
			GameObject instance0 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 0f, 0f), Quaternion.identity) as GameObject;
			toInstantiate = floorTiles[1];
			GameObject instance1 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 1f*assetsSize, 0f), Quaternion.identity) as GameObject;
			toInstantiate = floorTiles[2];
			GameObject instance2 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 2f*assetsSize, 0f), Quaternion.identity) as GameObject;
			toInstantiate = floorTiles[6];
			GameObject instance12 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 12f*assetsSize, 0f), Quaternion.Euler(new Vector3 (0, 0, 180f))) as GameObject;

			toInstantiate = floorTiles[0];
			GameObject instance13 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 13f*assetsSize, 0f), Quaternion.identity) as GameObject;

			//Assign one white line = 2
			for (int y = 3; y < 6; y++) {
				toInstantiate = floorTiles[3];
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x*assetsSize, y*assetsSize, 0f), Quaternion.identity) as GameObject;	
				instance.transform.SetParent (boardHolder);	
			}

			for (int y = 8; y < 12; y++) {
				toInstantiate = floorTiles[3];
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x*assetsSize, y*assetsSize, 0f), Quaternion.identity) as GameObject;	
				instance.transform.SetParent (boardHolder);	
			}

			//Assign both lines = 3
			toInstantiate = floorTiles[4];
			GameObject instance6 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 6*assetsSize, 0f), Quaternion.identity) as GameObject;	

			//Assign both lines = 1
			toInstantiate = floorTiles[5];
			GameObject instance7 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 7*assetsSize, 0f), Quaternion.identity) as GameObject;	

			//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
			instance0.transform.SetParent (boardHolder);
			instance1.transform.SetParent (boardHolder);
			instance2.transform.SetParent (boardHolder);

			instance6.transform.SetParent (boardHolder);
			instance7.transform.SetParent (boardHolder);

			instance12.transform.SetParent (boardHolder);
			instance13.transform.SetParent (boardHolder);
		}
			
	}

	//SetupScene initializes our diffculty and calls the previous functions to lay out the game board
	public void SetupScene (int diffculty) {
		GameObject toInstantiate;
		//TODO: Ask order layers
		//TODO: modify difficulty later
		for (int y = 2; y < 7; y++) {
			toInstantiate = rightCarTiles[0];
			GameObject instance =
				Instantiate (toInstantiate, new Vector3 (0, y*assetsSize, 0f), Quaternion.Euler(new Vector3 (0, 0, 180f))) as GameObject;
			//			(instance as littleCarPink).speed = 0.001;
			instance.transform.SetParent (boardHolder);
		}

		for (int y = 7; y < 12; y++) {
			toInstantiate = leftCarTiles[0];
			GameObject instance =
				Instantiate (toInstantiate, new Vector3 (0, y*assetsSize, 0f), Quaternion.identity) as GameObject;
			//			(instance as littleCarPink).speed = -0.001;
			instance.transform.SetParent (boardHolder);
		}

		GameObject chicken_instance =
			Instantiate (chicken, new Vector3 (4f*assetsSize, 1*assetsSize, 0f), Quaternion.identity) as GameObject;
		chicken_instance.transform.SetParent (boardHolder);
		
		GameObject chicken_instance2 =
			Instantiate (chicken2, new Vector3(13f*assetsSize, 1f*assetsSize, 0), Quaternion.identity) as GameObject;
		chicken_instance.transform.SetParent (boardHolder);

		//Creates the outer walls and floor.
		BoardSetup ();
	}

}
