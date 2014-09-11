using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

    //the velocity of the wooden boats
    private const int FORWARD_MOVEMENT_SPEED = 5;

	private const float CD = 2.75f;
	private float timer;
	private bool time;
	private GameObject explosion;

	// Use this for initialization
	void Start()
	{
		time = false;
		explosion = Resources.Load<GameObject>("BoatExplosion");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!time) 
		{
			float dtime = Time.deltaTime;
			Vector3 forward = this.transform.forward;
			transform.position += forward * ( FORWARD_MOVEMENT_SPEED + (int)Mathf.Min( 10f, GameController.Instance.Multiplier / 5 ) ) * dtime;

		}

		if (this.transform.childCount < 2 && !time) 
		{
			Vector3 up = new Vector3(0f,1f,0f);
			var obj = GameObject.Instantiate(explosion, this.transform.position + up, Quaternion.identity);
            ((GameObject) obj).transform.Rotate(new Vector3(-90, 0, 0));
			time = true;
			timer = 0;
		}

		if (time)
		{
			timer += Time.deltaTime;
			if (timer > CD)
			{

				GameObject.Destroy( this.gameObject );
			}
		}
	}


	private void OnCollisionEnter( Collision o )
	{
        GameController.Instance.onBoatCollision( this.gameObject );
	}
}
