using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour 
{
    private Texture _crosshair;
    private Rect _crosshairLoc;
    private ScreenPad _move;
    private ScreenPad _look;

	private void Start () 
    {
        _crosshair = Resources.Load(@"Textures/crosshair") as Texture;
        _crosshairLoc = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100);
        //_move = new ScreenPad(new Rect(20, Screen.height - (Screen.width * 0.15f + 20), Screen.width * 0.15f, Screen.width * 0.15f));
        //_look = new ScreenPad(new Rect(Screen.width - (Screen.width * 0.15f + 20), Screen.height - (Screen.width * 0.15f + 20), Screen.width * 0.15f, Screen.width * 0.15f));
	}

    private void Update()
    {
        _crosshairLoc = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100);
        Screen.showCursor = false;
        //_move.Update();
        //_look.Update();
    }

    private void OnGUI()
    {
        GUI.DrawTexture(_crosshairLoc, _crosshair);
        //_move.OnGUI();
        //_look.OnGUI();
    }
}
