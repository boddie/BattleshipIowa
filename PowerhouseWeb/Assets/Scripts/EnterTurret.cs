using UnityEngine;
using System.Collections;

public class EnterTurret : MonoBehaviour 
{
    private GameObject player;
    private GUISkin skin;
	private Quaternion originalRotation;
    public GameObject child;
    public GameObject currentCamera;

    private void Start()
    {
        player = GameObject.Find("First Person Controller");
        skin = Resources.Load<GUISkin>("gameSkin");		
		originalRotation = this.transform.rotation;
	}

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 4)
        {
            if (Input.GetMouseButtonDown(0) && !GameController.Instance.RoomStatus[GameController.ShipRoom.WEAPON])
            {
                child.SetActive(true);
                currentCamera.SetActive(false);
                GameController.Instance.inTurret = true;
				this.gameObject.GetComponent<TurretMouseLook>().enabled = true;
            }
            if (GameController.Instance.GameOver || (GameController.Instance.inTurret && 
                (Input.GetMouseButtonDown(1) || GameController.Instance.RoomStatus[GameController.ShipRoom.WEAPON])))
            {
                currentCamera.SetActive(true);
                child.SetActive(false);
                GameController.Instance.inTurret = false;
				this.gameObject.GetComponent<TurretMouseLook>().enabled = false;
				this.transform.rotation = originalRotation;
            }
        }
    }

    private void OnGUI()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 4 && !GameController.Instance.inTurret)
        {
            GUI.skin = skin;
            GUI.color = Color.red;
            Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Left Click To Enter"));
            Rect textLocation = new Rect(Screen.width / 2 - textSize.x / 2, Screen.height / 2 - 120, textSize.x, textSize.y);
            GUI.Label(textLocation, "Left Click To Enter");
        }
        if (GameController.Instance.inTurret)
        {
            GUI.skin = skin;
            GUI.color = Color.red;
            Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Right Click To Exit"));
            Rect textLocation = new Rect(Screen.width / 2 - textSize.x / 2, Screen.height / 2 - 120, textSize.x, textSize.y);
            GUI.Label(textLocation, "Right Click To Exit");
        }
    }
}
