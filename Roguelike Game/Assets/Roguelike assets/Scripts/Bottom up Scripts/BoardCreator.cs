using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour
{
	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}

	//Dimensions of the board
	public int columns = 100;
	public int rows = 100;
	//Random int values used for number of rooms, (random) dimensions of rooms, length of corridors and number of Enemies
	public IntRange numRooms = new IntRange (15, 20);         
	public IntRange roomWidth = new IntRange (3, 10);         
	public IntRange roomHeight = new IntRange (3, 10);        
	public IntRange corridorLength = new IntRange (6, 10);    
	public IntRange numEnemies = new IntRange (1,6);
	//Arrays for floor,wall and outer wall prefabs respectively
	public GameObject[] floorTiles;                           
	public GameObject[] wallTiles;                            
	public GameObject[] outerWallTiles;
	//Objects to store the player and enemy prefabs in
	public GameObject player;
	public GameObject enemy;
	//Game Object for the exit tile
	public GameObject Exit;
	//Array to represent the board
	private TileType[][] tiles;
	//Arrays to store the rooms and corridors respectively
	private Room[] rooms;
	private Corridor[] corridors;
	//Game Object that contains all the tiles on the board
	private GameObject boardHolder;


	public void Setup (int level)
	{
		
		//Create the board holder.
		boardHolder = new GameObject("BoardHolder");
		//Sets up the rest of the board, tiles and instantiates the player/enemy prefabs
		SetupTilesArray ();

		CreateRoomsAndCorridors ();

		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		InstantiateTiles ();
		InstantiateOuterWalls ();
	}


	void SetupTilesArray ()
	{
		//Set the tiles array to the correct width.
		tiles = new TileType[columns][];

		//sets each tile array to the correct height
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[rows];
		}
	}


	void CreateRoomsAndCorridors ()
	{
		//Create the rooms array with a random size specified earlier.
		rooms = new Room[numRooms.Random];
		int roomlen = rooms.Length;
		//One less corridor than rooms so all rooms connecting
		corridors = new Corridor[rooms.Length - 1];

		//Creates the first room and corridor.
		rooms[0] = new Room ();
		corridors[0] = new Corridor ();

		//Setup the first room, No previous corridor so no parameter past
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		//Setup the first corridor using the first room.
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);
                //Creates all remaining rooms and corridors
		for (int i = 1; i < roomlen; i++)
		{
			rooms[i] = new Room ();

			
			rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			
			if (i < corridors.Length)
			{
				
				corridors[i] = new Corridor ();

				
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}
                        // Instantiates the player  at approx the middke board
			if (i == Mathf.Floor(roomlen *.5f))
			{
				Vector3 playerPos = new Vector3 (rooms[i].xPos, rooms[i].yPos, 0);
				Instantiate(player, playerPos, Quaternion.identity);
			}
		}
		
		
		for (int j = 0; j < numEnemies.Random; j++) 
		{
			int x = Random.Range (0, roomlen - 1);
			if (x != Mathf.Floor(roomlen*.5f))
			{
				Vector3 enemyPos = new Vector3 (rooms [x].xPos, rooms [x].yPos, 0);
				Instantiate (enemy, enemyPos, Quaternion.identity);
			}
		}
		Vector3 exit = new Vector3 (rooms [roomlen - 1].xPos, rooms [roomlen - 1].yPos, 0);
		Instantiate (Exit, exit, Quaternion.identity);

	}

        // Sets up tiles for the rooms
	void SetTilesValuesForRooms ()
	{
		
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];

			
			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				
				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;
					
					tiles[xCoord][yCoord] = TileType.Floor;
				}	//if ((i == rooms.Length-1)&&(j == currentRoom.roomHeight
			}
		}
	}


	void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
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

				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}


	void InstantiateTiles ()
	{
		// Go through all the tiles in the jagged array...
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				// ... and instantiate a floor tile for it.
				InstantiateFromArray (floorTiles, i, j);

				// If the tile type is Wall...
				if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					InstantiateFromArray (wallTiles, i, j);
				}
			}
		}
	}


	void InstantiateOuterWalls ()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}


	void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
			// ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(outerWallTiles, xCoord, currentY);

			currentY++;
		}
	}


	void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
			// ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray (outerWallTiles, currentX, yCoord);

			currentX++;
		}
	}


	void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
	}
}
