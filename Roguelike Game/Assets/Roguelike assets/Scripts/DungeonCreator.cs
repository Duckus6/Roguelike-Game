using UnityEngine;
using System.Collections;
public class DungeonCreator : MonoBehaviour
{
	//type of tiles on the board
	public enum TileType
	{
		Wall, Floor,
	}
	// dimensions for the board
	public int columns = 100;
	public int rows = 100;
	// Random values for dungeon generation (num rooms, dimensions of rooms and corridor length
	public IntRange numRooms = new IntRange (10, 20);
	public IntRange roomWidth = new IntRange (4, 12);
	public IntRange roomHeight = new IntRange (4, 12);
	public IntRange corridorLength = new IntRange(3,8);
	//Arrays for prefabs for tiles
	public GameObject[] FloorTiles;
	public GameObject[] WallTiles;
	public GameObject[] OuterWallTiles;
	//PlayerPrefab for spawning inside dungeon
	public GameObject player;
	//2d array (board) representing tile types (wall/floor)
	private TileType[][] tiles;
	//Private variables containing all rooms/corridors
	private Room[] rooms;
	private Corridor[] corridors;
	//Private gameobject containing all board tiles
	private GameObject boardHolder;
	private void Start()
	{
		boardHolder = new GameObject ("BoardHolder");
		setupTilesArray ();
		//creates the rooms/corridors of the dungeon
		CreateRoomsandCorridors ();
		//set tile values for both rooms and corridors
		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();
		InstantiateTiles ();
		InstantiateOuterWalls ();
	}


	void setupTilesArray ()
	{
		tiles = new TileType[columns][];
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[rows];
		}
	}


	void CreateRoomsandCorridors()
	{
		//Room array with random size
		rooms = new Room[numRooms.Random];
		corridors = new Corridor[rooms.Length - 1];
		//creates the first room and corridor (special case as no other rooms/corridors in existance)
		rooms [0] = new Room ();
		corridors [0] = new Corridor ();
		rooms[0].SetupRoom (roomWidth, roomHeight, columns, rows);
		corridors [0].SetupCorridor (rooms [0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		//creates remaining rooms/corridors
		for (int i = 1; i < rooms.Length; i++) 
		{
			rooms [i] = new Room ();
			rooms [i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors [i - 1]);

			if (i < corridors.Length) 
			{
				corridors [i] = new Corridor ();
				corridors [i].SetupCorridor (rooms [i], corridorLength, roomWidth, roomHeight, columns, rows, false);

			}
			if (i == rooms.Length * .5f) 
			{
				Vector3 playerPos = new Vector3 (rooms [i].xPos, rooms [i].yPos, 0);
				Instantiate (player, playerPos, Quaternion.identity);

			}
			if (i == rooms.Length * 0.5) 
			{
				Vector3 PlayerPos = new Vector3 (rooms [i].xPos, rooms [i].yPos, 0);
				Instantiate (player, PlayerPos, Quaternion.identity);
			}
		}

	}

	//Set tile values for the rooms
	void SetTilesValuesForRooms ()
	{
		for (int i = 0; i < rooms.Length; i++) 
		{
			Room currentRoom = rooms [i];
			for (int j = 0; j < currentRoom.roomWidth; j++) 
			{
				int xCoord = currentRoom.xPos + j;
				for (int k = 0; k < currentRoom.roomHeight; k++) 
				{
					int yCoord = currentRoom.yPos + k;
					tiles [xCoord] [yCoord] = TileType.Floor;
				}
			}
		}
	}

	// set tile values for the corridors
	void SetTilesValuesForCorridors()
	{
		for (int i = 0; i < corridors.Length; i++) 
		{
			Corridor currentCorridor = corridors [i];
			for (int j = 0; j < currentCorridor.corridorLength; j++) 
			{
				//start coords at the start of the corridor
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;
				//add/sub from appropriate coord depending on the direction 
				switch (currentCorridor.direction) 
				{
				case Direction.North:
					yCoord += j;
					break;
				case Direction.East:
					xCoord += j;
					break;
				case Direction.South:
					yCoord -= j;
					break;
				case Direction.West:
					xCoord -= j;
					break;
				}
				//set tiles at these coords to be a Floor tile
				tiles [xCoord] [yCoord] = TileType.Floor;
			}
		}
	}

	// create walls
	void InstantiateTiles()
	{
		for (int i = 0; i < tiles.Length; i++) 
		{
			for (int j = 0; j < tiles [i].Length; j++) 
			{
				InstantiateFromArray (FloorTiles, i, j);
				if (tiles [i] [j] == TileType.Wall) 
				{
					InstantiateFromArray (WallTiles, i, j);
				}
			}
		}
	}


	void InstantiateOuterWalls()
	{
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;
		//instantiate both sides of the vertical walls
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall (rightEdgeX, bottomEdgeY, topEdgeY);
		//instantiate both horizontal, these are ones in left and right from the outer walls
		InstantiateHorizontalOuterWall (leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall (leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);

	}
	//instantiate vertical outer walls
	void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
	{
		float currentY = startingY;
		//while y value less than the ending y value
		while (currentY <= endingY) 
		{
			//instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray (OuterWallTiles, xCoord, currentY);
			currentY++;

		}
	}
	//instantiate horizontal outer walls
	void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
	{
		float currentX = startingX;
		while (currentX <= endingX) 
		{
			InstantiateFromArray (OuterWallTiles, currentX, yCoord);
			currentX++;
		}
	}

	void InstantiateFromArray(GameObject[]prefabs,float xCoord, float yCoord)
	{
		//creates a random index from the array
		int randomIndex = Random.Range (0, prefabs.Length);

		//The position to be instantiated at is based on the coordinates
		Vector3 position = new Vector3(xCoord,yCoord,0f);
		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefabs[randomIndex],position, Quaternion.identity) as GameObject;
		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}
}