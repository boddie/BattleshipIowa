using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

	public enum ShipRoom
	{
		COMMUNICATIONS,
		CONTROL,
		WEAPON,
		POWER,
		ENGINE,
		STORAGE
	};

	//a boolean value which determines if the game is over
	public bool GameOver
	{
		get;
		private set;
	}

    //private variable to represent progress
    private float progress;

    //Represents the progress to the destination
    public float Progress
    {
        get
        {
            progress = Mathf.Max(0f, Mathf.Min(1f, progress));
            return progress;
        }
        //private set;
    }

    //private variable to represent ship health
    private float shipHealth;

    //Represents the health of the ship
    public float ShipHealth
    {
        get
        {
            shipHealth = Mathf.Max( 0f, Mathf.Min( 1f, shipHealth ) );
            return shipHealth;
        }
        //private set;
    }

    //private variable to represent ship velocity
    private float velocity;

    //represents the forward velocity of the ship towards the goal
    public float Velocity
    {
        get
        {
            velocity = Mathf.Max(0f, Mathf.Min(1f, velocity));
            return velocity;
        }
        //private set;
    }

    //private variable to represent ship heading
    private float heading;

    //the current heading of the ship towards the goal, represented as a float. 0 = backwards, 1 = directly towards the goal ( 0, 1 ) = percentage of heading
    public float Heading
    {
        get
        {
            heading = Mathf.Max(0f, Mathf.Min(1f, heading));
            return heading;
        }
        //private set;
    }

    //the current score multiplier
    public int Multiplier
    {
        get;
        private set;
    }

    //the current score
    public int Score
    {
        get;
        private set;
    }

    //a dictionary which represents the occupied state of each room
    public Dictionary<ShipRoom, bool> RoomStatus
    {
        get;
        private set;
    }

    //the time, in milliseconds, which will correspond to one unit of movement
    private const int UNIT_OF_MOVEMENT_TIMEFRAME = 200;

    //the time, in milliseconds, between potential 1% progress gains
    private const int UNIT_OF_PROGRESS_UPDATE_TIMEFRAME = 2;

    //the time, in milliseconds, between potential 1% heading changes
    private const int UNIT_OF_HEADING_CHANGE = 100;

    //the time, in seconds, between boat spawns when enemies are not present on the ship
    private const int SPAWN_RATE_WHEN_UNOCCUPIED = 7;

    //the time, in seconds, between boat spawns when enemies are present on the ship
    private const int SPAWN_RATE_WHEN_OCCUPIED = 12;

    //the base number of points gained when an enemy is killed
    private const int POINTS_PER_ENEMY_KILLED = 100;
	
	//the time of the last spawned enemy
	private float lastEnemySpawned;

    //the time of the last storage room hit
    private float lastStorageRoomDamage;

	//a random number generator, nothing to see here
	private System.Random randy;

	//a dictionary storing our room statuses
	private Dictionary<ShipRoom, float> roomHealth;
	
	//the GameObject which represents the enemies in the game, Unity will initialize
	public GameObject enemyGameObject;

	//the GameObject which represents the enemies in the game, Unity will initialize
	public GameObject boatGameObject;

	//an array of GameObjects, used as spawn points, that Unity will initialize
	public GameObject[] roomSpawnPoints;
	
	//an array of GameObjects, used as spawn points, that Unity will initialize
	public GameObject[] boatSpawnPoints;

    // Lets us know if user is in the turrent to switch some UI information
    public bool inTurret;

    // Objects of current active enemies
    public List<GameObject> activeEnemies;

    // Objects of current active boats
    public List<GameObject> activeBoats;

    // Creates an instance of itself
    private void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance);
        else
            Instance = this;
    }


	private void Start()
	{
		GameOver = false;
        inTurret = false;

		roomHealth = new Dictionary<ShipRoom, float>();
		foreach( ShipRoom room in (ShipRoom[]) Enum.GetValues( typeof( ShipRoom ) ) )
		{
			roomHealth.Add( room, 1f );
		}

        //we start with 0 velocity but are pointed directly towards the goal
        velocity = 0f;
        heading = 1f;

        //we have 100% of ships health, but 0% progress
        shipHealth = 1f;
        progress = 0f;

        //the Score starts at 0, with the multiplier starting at 1
        Score = 0;
        Multiplier = 1;

        //initialize the status of each room
        RoomStatus = new Dictionary<ShipRoom, bool>();
        foreach (ShipRoom room in (ShipRoom[])Enum.GetValues(typeof(ShipRoom)))
        {
            RoomStatus.Add(room, false);
        }

        //mark invalid times for events
		lastEnemySpawned = -1f;
        lastStorageRoomDamage = -1;

        //initialize objects
        activeEnemies = new List<GameObject>();
        activeBoats = new List<GameObject>();
        randy = new System.Random();
	}


    /**
     * the update function called by Unity at every frame
     */
	public void Update()
    {
        if (!GameOver)
        {
            if (ShipHealth <= 0f || Progress >= 1f)
            {
                GameOver = true;
                return;
            }

            float dT = Time.deltaTime;
            float time = Time.time;

            //update the properties of the ship
            UpdateShipVelocity(dT);
            UpdateShipHeading(dT);
            UpdateGameProgress(dT);

            //we take a percent of damage for every few seconds that an enemy is in the storage room
            if (RoomStatus[ShipRoom.STORAGE])
            {
                if (time - lastStorageRoomDamage > 3f)
                {
                    lastStorageRoomDamage = time;
                    shipHealth -= 0.01f;
                }
            }

            //determine if it's time to spawn an enemy
            int spawnRate = activeEnemies.Count > 0 ? SPAWN_RATE_WHEN_OCCUPIED : SPAWN_RATE_WHEN_UNOCCUPIED;
            if (((int)Time.time) % spawnRate == 0 && lastEnemySpawned < Math.Floor(time))
            {
                lastEnemySpawned = time;
                SpawnBoat();
            }
        }
	}



    /**
     * updates the velocity of the ship based on the status of the varying rooms
     * @param delta - the amount of time passed since the last update, in milliseconds
     */
    public void UpdateShipVelocity(float delta)
    {
        //determine the potential velocity based on the status of three rooms: Power, Engine, and Control
        //float vPotential = roomHealth[ ShipRoom.CONTROL ] * roomHealth[ ShipRoom.ENGINE ] * roomHealth[ ShipRoom.POWER ];
        float vPotential = (RoomStatus[ShipRoom.ENGINE] || RoomStatus[ShipRoom.POWER]) ? -1f : 1f;

        //the amount of change possible depends on the amount of time passed
        float dPotential = delta / UNIT_OF_MOVEMENT_TIMEFRAME;

        //the actual amount of change possible
        float potential = dPotential * vPotential;

        velocity = Mathf.Max(Mathf.Min(Velocity + potential, 1f), 0f);
    }



    /**
     * updates the velocity of the ship based on the status of the varying rooms
     * @param delta - the amount of time passed since the last update, in milliseconds
     */
    public void UpdateShipHeading(float delta)
    {
        //determine the potential velocity based on the status of three rooms: Power, Engine, and Control
        float hPotential = RoomStatus[ShipRoom.CONTROL] ? -1f : 1f;

        //the amount of change possible depends on the amount of time passed
        float dHeading = delta / UNIT_OF_HEADING_CHANGE;

        //the actual amount of change possible
        float potential = hPotential * dHeading;

        heading = Mathf.Max(Mathf.Min(Heading + potential, 1f), 0f);
    }



    /**
     * updates progress towards the goal based on the time passed since the last update
     */
    public void UpdateGameProgress(float delta)
    {
        float dPotential = delta / UNIT_OF_PROGRESS_UPDATE_TIMEFRAME;
        float potential = Heading * Velocity * dPotential;
        progress += ( potential * 0.01f );
    }



	/**
	 * updates the health of the given room
	 */
	public void UpdateRoomHealth( ShipRoom room, float health )
	{
		//clamp room health [ 0, 1.0 ]
		health = Mathf.Max( Mathf.Min ( 1f, health ), 0f );

		switch( room )
		{
		case ShipRoom.COMMUNICATIONS:

			//the communications room is either disabled or enabled
			health = Mathf.Round( health );

			if( health == 0f )
			{
				// TODO DISABLE COMMUNICATIONS ROOM
			}
			else
			{
				// TODO enable communications room
			}
			
			break;
			
			
		case ShipRoom.CONTROL:

			//nothing to do here
			
			break;


		case ShipRoom.WEAPON:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.POWER:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.ENGINE:
			
			//nothing to do here
			
			break;
			
			
		case ShipRoom.STORAGE:

			if( health == 0f ) GameOver = true;
			
			break;
		}

		//store the new health of the given room into the dictionary
		roomHealth[ room ] = health;
	}


    /**
     * changes the state of a room, given the room and the status to modify
     */
    public void SetRoomStatus(ShipRoom room, bool occupied)
    {
        RoomStatus[room] = occupied;
    }


	/**
	 * spawns an enemy boat at a random water-based spawning point
	 */
	public void SpawnBoat()
	{
        activeBoats.Add(InstantiateAtRandomSpawnPoint( boatGameObject, boatSpawnPoints ));
	}

    /**
     * spawns a single enemy into a random room
     */
    public void SpawnEnemy()
    {
        activeEnemies.Add( InstantiateAtRandomSpawnPoint( enemyGameObject, roomSpawnPoints ) );
    }


    /**
     * invoked when a rowboat hits the ship
     */
    public void onBoatCollision( GameObject gameObject )
    {
        int numberOfEnemies = gameObject.transform.childCount - 1;
        shipHealth -= ( 5 + numberOfEnemies ) * 0.01f;
        Multiplier = 1;

        activeBoats.Remove(gameObject);
        GameObject.Destroy( gameObject );

        //each enemy on the boat gives a chance of spawning an enemy onto the boat
        for ( int i = 0; i < numberOfEnemies; ++i )
        {
            double r = randy.NextDouble();
            
            //25% chance to spawn an enemy on the boat
            if ( r >= 0.75 )
            {
                SpawnEnemy();
                break;
            }
        }
    }


    /**
     * invoked when an enemy on the ship is killed
     */
    public void onEnemyKilled(GameObject gameObject)
    {
        activeEnemies.Remove(gameObject);
        GameObject.Destroy(gameObject);
        Multiplier += 1;
        Score += ( Multiplier * POINTS_PER_ENEMY_KILLED );
    }


    /**
     * duplicate code is a bad thing.
     */
    private GameObject InstantiateAtRandomSpawnPoint( GameObject gameObject, GameObject[] spawnPoints )
    {
        if ( gameObject == null || spawnPoints == null ) return null;

        //select a random room
        int size = spawnPoints.Length;
        int selected = randy.Next( size );

        //set the position equal to the spawn point
        Vector3 translation = spawnPoints[selected].transform.position;

        //set the rotation equal to the spawning point's rotation
        Quaternion rotation = spawnPoints[selected].transform.rotation;

        //create the new enemy GameObject
        GameObject obj = (GameObject)GameObject.Instantiate(gameObject, translation, rotation);
        if(gameObject.name == "Enemy")
            activeEnemies.Add(obj);

        return obj;
    }
}
