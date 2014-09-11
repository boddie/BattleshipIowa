using UnityEngine;
using System.Collections;

public class RoomStatusUI : MonoBehaviour 
{
    private string name;
    private bool inRange;
    private Texture bad;
    private Texture ok;
    private Rect drawRect;

    private void Start()
    {
        name = this.gameObject.name;
        inRange = false;
        float rectWidth = Screen.width / 20;
        float standardWidth = Screen.width * 0.017f;
        float startx = Screen.width / 2 - (rectWidth*6 + standardWidth*5) / 2;
        switch (name)
        {
            case "SpawnComm":
                bad = Resources.Load<Texture>(@"Textures/CommBAD");
                ok = Resources.Load<Texture>(@"Textures/CommOK");
                drawRect = new Rect(startx, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
            case "SpawnControl":
                bad = Resources.Load<Texture>(@"Textures/ControlBAD");
                ok = Resources.Load<Texture>(@"Textures/ControlOK");
                drawRect = new Rect(rectWidth + startx + standardWidth, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
            case "SpawnEngine":
                bad = Resources.Load<Texture>(@"Textures/EngineBAD");
                ok = Resources.Load<Texture>(@"Textures/EngineOK");
                drawRect = new Rect(rectWidth * 2 + startx + standardWidth * 2, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
            case "SpawnPower":
                bad = Resources.Load<Texture>(@"Textures/PowerBAD");
                ok = Resources.Load<Texture>(@"Textures/PowerOK");
                drawRect = new Rect(rectWidth * 3 + startx + standardWidth * 3, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
            case "SpawnStorage":
                bad = Resources.Load<Texture>(@"Textures/StorageBAD");
                ok = Resources.Load<Texture>(@"Textures/StorageOK");
                drawRect = new Rect(rectWidth * 4 + startx + standardWidth * 4, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
            case "SpawnWeapon":
                bad = Resources.Load<Texture>(@"Textures/WeaponBAD");
                ok = Resources.Load<Texture>(@"Textures/WeaponOK");
                drawRect = new Rect(rectWidth * 5 + startx + standardWidth * 5, Screen.height - (rectWidth + 20), rectWidth, rectWidth);
                break;
        }
    }

    private void Update()
    {
        bool check = false;
        foreach (GameObject v in GameController.Instance.activeEnemies)
        {
            if (v == null) continue;
            if (Vector3.Distance(this.transform.position, v.transform.position) < 0.5f)
            {
                check = true;
            }
        }
        if (check != inRange)
        {
            inRange = check;
            setRoomStatus(inRange);
        }
    }

    private void OnGUI()
    {
        if (inRange)
        {
            GUI.DrawTexture(drawRect, bad);
        }
        else
        {
            GUI.DrawTexture(drawRect, ok);
        }
    }

    private void setRoomStatus(bool occupied)
    {
        switch (name)
        {
            case "SpawnComm":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.COMMUNICATIONS, occupied);
                break;
            case "SpawnControl":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.CONTROL, occupied);
                break;
            case "SpawnEngine":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.ENGINE, occupied);
                break;
            case "SpawnPower":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.POWER, occupied);
                break;
            case "SpawnStorage":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.STORAGE, occupied);
                break;
            case "SpawnWeapon":
                GameController.Instance.SetRoomStatus(GameController.ShipRoom.WEAPON, occupied);
                break;
        }
    }
}
