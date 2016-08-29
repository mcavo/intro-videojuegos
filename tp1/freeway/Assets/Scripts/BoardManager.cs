using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardManager : MonoBehaviour {

	public int columns = 18; 										//Number of columns in our game board.
	public int rows = 14;											//Number of rows in our game board.
	public static float assetsSize = 0.36f;

	// 0 = empty
	// 1 = grass -> chicken initial position
	// 2 = grey tile
	// 3 = grey with a white line
	// 4 = grey with a yellow line on top
	// 5 = grey with a yellow line on bottom
	// 6 = grass -> where the chicken has to arrive
	public GameObject[] floorTiles;									//Vector with different types of floors.
	public GameObject[] rightCarTiles;								//Vector with different types of cars.
	public GameObject[] leftCarTiles;								//Vector with different types of cars.
	public GameObject[] chickens;									//Vector with different players

	public Text timeText;

	private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
	private Canvas canvas;

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
		initializeText();
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
			instance.transform.SetParent (boardHolder);
		}

		for (int y = 7; y < 12; y++) {
			toInstantiate = leftCarTiles[0];
			GameObject instance =
				Instantiate (toInstantiate, new Vector3 (0, y*assetsSize, 0f), Quaternion.identity) as GameObject;
			//			(instance as littleCarPink).speed = -0.001;
			instance.transform.SetParent (boardHolder);
		}
			
		// Initialize players
		for (int i = 0; i < chickens.Length; i++) {
			GameObject chickenInstance = Instantiate (chickens[i], new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
			chickenInstance.transform.SetParent (boardHolder);
		}

		//Creates the outer walls and floor.
		BoardSetup ();
	}

	private void initializeText() {
		GameObject timeGameObject = new GameObject("TextScore");
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

		timeGameObject.transform.SetParent(canvas.transform, false);

		RectTransform trans = timeGameObject.AddComponent<RectTransform>();

		Text text = timeGameObject.AddComponent<Text>();
		timeText = text;

		settingAnchor (trans);
		addingTextStyle();
		updateTextDimensionAndPosition ();
	}

	private void updateTextDimensionAndPosition () {
		Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		int newSize = Screen.height / 11;
		timeText.fontSize = newSize;
		Vector3 cameraPosition = camera.WorldToScreenPoint (new Vector3(9f*assetsSize, 13f*assetsSize, 0));
		timeText.rectTransform.anchoredPosition = new Vector2 (cameraPosition.x, cameraPosition.y);
	}

	private void addingTextStyle() {
		//Adding fotmat style to text
		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		timeText.alignment = TextAnchor.MiddleCenter;
		timeText.color = Color.white;
		timeText.font = ArialFont;
		timeText.horizontalOverflow = HorizontalWrapMode.Overflow;
		timeText.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void settingAnchor(RectTransform trans) {
		//Set anchor in the bottom left corner
		trans.anchorMax = new Vector2 (0f, 0f);
		trans.anchorMin = new Vector2 (0f, 0f);
		trans.pivot = new Vector2 (0.5f, 0.5f);
	}
}
