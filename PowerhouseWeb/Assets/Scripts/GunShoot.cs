using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour 
{
    private const float CD = 0.5f;
    private float timer;
    private bool time;
    private GameObject explosion;
    private GameObject spawn;

    void Start()
    {
        time = false;
        explosion = Resources.Load<GameObject>("GunExplosion");
        spawn = GameObject.Find("ExplosionSpawn");
    }
	
	// Update is called once per frame
	void Update () 
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name.Contains("Enemy") && !time)
            {
                //inform the gamecontroller that we have killed an enemy on the boat
                GameController.Instance.onEnemyKilled( hit.collider.gameObject );

                audio.Play();
                var obj = GameObject.Instantiate(explosion, spawn.transform.position, Quaternion.identity);
                ((GameObject)obj).transform.parent = this.transform;
                time = true;
                timer = 0;
            }
        }

        if (time)
        {
            timer += Time.deltaTime;
            if (timer > CD)
            {
                time = false;
            }
        }
	}
}
