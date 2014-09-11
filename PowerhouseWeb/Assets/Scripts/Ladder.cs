using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour 
{

	public Vector3 transportLocation;

	void OnTriggerEnter (Collider other) 
    {
		if(other.gameObject.tag == "Player")
        {
			other.gameObject.transform.position = transportLocation;
			
		}			
	}
}
