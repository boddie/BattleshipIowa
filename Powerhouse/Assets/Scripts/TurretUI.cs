using UnityEngine;
using System.Collections;

public class TurretUI : MonoBehaviour 
{

    public Rigidbody rocket;

    void FireRocket()
    {
        audio.Play();
        Vector3 cross = Vector3.Cross(this.transform.parent.forward, this.transform.parent.up);
        Vector3 startPos1 = this.transform.parent.position + this.transform.parent.forward * 3 + this.transform.parent.up * 10 + cross * 1.5f;
        Vector3 startPos2 = this.transform.parent.position + this.transform.parent.forward * 3 + this.transform.parent.up * 10;
        Vector3 startPos3 = this.transform.parent.position + this.transform.parent.forward * 3 + this.transform.parent.up * 10 - cross * 1.5f;
        Instantiate(rocket, startPos1, this.transform.parent.rotation);
        Instantiate(rocket, startPos2, this.transform.parent.rotation);
        Instantiate(rocket, startPos3, this.transform.parent.rotation);
    }

    void Update()
    {
        Screen.showCursor = false;
        if (Input.GetButtonDown("Fire1"))
        {
            FireRocket();
        }
    }
}
