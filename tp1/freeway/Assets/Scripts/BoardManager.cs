using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardManager : MonoBehaviour
{
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
	public GameObject blackCover;
	public GameObject[] carTiles;								//Vector with different types of cars.
	public GameObject[] chickens;									//Vector with different players

	public Text timeText;

	private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
	private Transform carsHolder;									//A variable to store a reference to the transform of our Cars objects.
	private Transform chickensHolder;								//A variable to store a reference to the transform of our Chickens objects.
	private Canvas canvas;
	private float correctionFactor = 0.10f;

	//Sets up the outer walls and floor (background) of the game board.
	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;
		GameObject toInstantiate;

		for(int x = 0; x < columns; x++)
		{
			//Asign empty floors = 0
			//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
			//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
			toInstantiate = floorTiles[0];
			GameObject instance0 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 0f, 0f), Quaternion.identity) as GameObject;
			instance0.transform.SetParent (boardHolder);

			toInstantiate = floorTiles[1];
			GameObject instance1 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 1f*assetsSize, 0f), Quaternion.identity) as GameObject;
			instance1.transform.SetParent (boardHolder);

			toInstantiate = floorTiles[2];
			GameObject instance2 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 2f*assetsSize, 0f), Quaternion.identity) as GameObject;
			instance2.transform.SetParent (boardHolder);

			toInstantiate = floorTiles[6];
			GameObject instance12 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 12f*assetsSize, 0f), Quaternion.Euler(new Vector3 (0, 0, 180f))) as GameObject;
			instance12.transform.SetParent (boardHolder);

			toInstantiate = floorTiles[0];
			GameObject instance13 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 13f*assetsSize, 0f), Quaternion.identity) as GameObject;
			instance13.transform.SetParent (boardHolder);

			//Assign one white line = 2
			for (int y = 3; y < 6; y++)
			{
				toInstantiate = floorTiles[3];
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x*assetsSize, y*assetsSize, 0f), Quaternion.identity) as GameObject;	
				instance.transform.SetParent (boardHolder);	
			}

			for (int y = 8; y < 12; y++)
			{
				toInstantiate = floorTiles[3];
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x*assetsSize, y*assetsSize, 0f), Quaternion.identity) as GameObject;	
				instance.transform.SetParent (boardHolder);	
			}

			//Assign both lines = 3
			toInstantiate = floorTiles[4];
			GameObject instance6 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 6*assetsSize, 0f), Quaternion.identity) as GameObject;	
			instance6.transform.SetParent (boardHolder);
			//Assign both lines = 1
			toInstantiate = floorTiles[5];
			GameObject instance7 =
				Instantiate (toInstantiate, new Vector3 (x*assetsSize, 7*assetsSize, 0f), Quaternion.identity) as GameObject;	
			instance7.transform.SetParent (boardHolder);
			//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
		}

		GameObject coverInstanceLeft =
			Instantiate (blackCover, new Vector3 (-3.5f * assetsSize, 6.5f * assetsSize, -1f), Quaternion.identity) as GameObject;
		coverInstanceLeft.transform.SetParent (boardHolder);

		GameObject coverInstanceRight =
			Instantiate (blackCover, new Vector3 (20.5f * assetsSize, 6.5f * assetsSize, -1f), Quaternion.identity) as GameObject;
		coverInstanceRight.transform.SetParent (boardHolder);

		initializeText("TimeText");
	}

	//SetupScene initializes our diffculty and calls the previous functions to lay out the game board
	public void SetupScene (int diffculty)
	{
		GameObject toInstantiate;
		carsHolder = new GameObject ("Cars").transform;
		for (int y = 2; y < 12; y++)
		{
			toInstantiate = carTiles[y - 2];
			GameObject instance =
				Instantiate (toInstantiate, new Vector3 (0, y*assetsSize, 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (carsHolder);
			CarManager script = instance.GetComponent<CarManager>();
			script.initialize (1);
			if (script.numberInRows > 1)
			{
				for (int j = 2; j <= script.numberInRows; j++ )
				{
					instance =
						Instantiate (toInstantiate, new Vector3 (0, y*assetsSize, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (carsHolder);
					script = instance.GetComponent<CarManager>();
					script.initialize (j);
				}	
			}
		}

		chickensHolder = new GameObject ("Chickens").transform;
		// Initialize players
		for (int i = 0; i < chickens.Length; i++)
		{
			GameObject chickenInstance = Instantiate (chickens[i], new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
			chickenInstance.transform.SetParent (chickensHolder);
		}
		GameManager.instance.scores = new int[chickens.Length];
		//Creates the outer walls and floor.
		BoardSetup ();
	}

	private void initializeText(string name)
	{
		GameObject timeGameObject = new GameObject(name);
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

		timeGameObject.transform.SetParent(canvas.transform, false);

		RectTransform trans = timeGameObject.AddComponent<RectTransform>();

		Text text = timeGameObject.AddComponent<Text>();
		timeText = text;

		settingAnchor (trans);
		addingTextStyle();
		updateTextDimensionAndPosition ();
	}

	private void updateTextDimensionAndPosition ()
	{
		Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		int newSize = Screen.height / 11;
		timeText.fontSize = newSize;
		Vector3 cameraPosition = camera.WorldToScreenPoint (new Vector3(9f*assetsSize, 13f*assetsSize + correctionFactor, 0));
		timeText.rectTransform.anchoredPosition = new Vector2 (cameraPosition.x, cameraPosition.y);
	}

	private void addingTextStyle()
	{
		//Adding fotmat style to text
		Font BitMapFont = Resources.Load("Masaaki-Regular", typeof(Font)) as Font;  
		timeText.alignment = TextAnchor.MiddleCenter;
		timeText.color = Color.white;
		timeText.font = BitMapFont;
		timeText.horizontalOverflow = HorizontalWrapMode.Overflow;
		timeText.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void settingAnchor(RectTransform trans)
	{
		//Set anchor in the bottom left corner
		trans.anchorMax = new Vector2 (0f, 0f);
		trans.anchorMin = new Vector2 (0f, 0f);
		trans.pivot = new Vector2 (0.5f, 0.5f);
	}
}
