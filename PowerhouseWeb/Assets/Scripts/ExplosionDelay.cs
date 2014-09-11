using UnityEngine;
using System.Collections;

public class ExplosionDelay : MonoBehaviour 
{
    public float DURATION = 0.2f;
    float timer;

	void Start () 
    {
        timer = 0;
	}
	
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer > DURATION)
            GameObject.Destroy(this.gameObject);
	}
}
