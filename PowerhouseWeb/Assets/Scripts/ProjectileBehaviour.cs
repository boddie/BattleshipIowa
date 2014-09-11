using UnityEngine;
using System.Collections.Generic;

public class ProjectileBehaviour : MonoBehaviour 
{

    public float MissileSpeed = 30.0f;

    void Start()
    {
        rigidbody.AddRelativeForce(Vector3.up * MissileSpeed);
    }

    void OnCollisionEnter(Collision c)
    {
        foreach (GameObject obj in GameController.Instance.activeBoats)
        {
            if (obj != null && Vector3.Distance(this.transform.position, obj.transform.position) < 20)
            {
                Stack<GameObject> children = new Stack<GameObject>();
                for (int i = 0; i < obj.transform.childCount; i++)
                    children.Push(obj.transform.GetChild(i).gameObject);
                while (children.Count > 0)
                    GameController.Instance.onEnemyKilled(children.Pop());
            }
        }
        GameObject.Destroy(this.gameObject);
    }
}
