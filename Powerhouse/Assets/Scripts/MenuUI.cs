using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour 
{
    public  GUISkin _style;

    private int _cursorSize;

    private Texture _menuBackground;
    private Texture _crossHair;
    private Texture _controls;
    private Texture _title;
    private Rect _backgroundRect;
    private Rect _crossHairRect;
    private Rect _controlsRect;
    private Rect _titleRect;

    private Vector2 mousePos;

	private void Start () 
    {
        _menuBackground = Resources.Load<Texture>(@"Textures/MenuBackground");
        _crossHair = Resources.Load<Texture>(@"Textures/crosshair");
        _controls = Resources.Load<Texture>(@"Textures/Controls");
        _title = Resources.Load<Texture>(@"Textures/title");
        _backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
        _crossHairRect = new Rect(0, 0, 0, 0);
        _controlsRect = new Rect(Screen.width / 2, Screen.height * 1/20f, Screen.width * 2/5f, Screen.height * 18/20f);
        _titleRect = new Rect(50, 50, Screen.width * 2 / 5f, Screen.height * 5 / 20f);

        _cursorSize = Screen.width / 10;
	}

    float _rotation = 0;
    bool _play = true;

    private void OnGUI()
    {
        if (Screen.showCursor == true)
            Screen.showCursor = false;

        Screen.showCursor = false;
        GUI.DrawTexture(_backgroundRect, _menuBackground, ScaleMode.StretchToFill);
        GUI.DrawTexture(_titleRect, _title, ScaleMode.StretchToFill);
        GUI.DrawTexture(_controlsRect, _controls, ScaleMode.StretchToFill);

        mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        GUISkin current = GUI.skin;
        GUI.skin = _style;

        Color prevColor = GUI.color;

        Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Start Game"));
        Rect textLocation = new Rect(50, Screen.height - textSize.y * 2.5f, textSize.x, textSize.y);

        if (textLocation.Contains(mousePos))
        {
            if (_play)
            {
                _play = false;
                audio.Play();
            }
            GUI.color = Color.red;
            GUI.Label(textLocation, "Start Game");
            if (GUI.Button(textLocation, "", "Label"))
                Application.LoadLevel("Game");
        }
        else
        {
            _play = true;
            GUI.Label(textLocation, "Start Game");
        }

        GUI.color = prevColor;
        GUI.skin = current;

        _rotation += Time.deltaTime * 100;
        GUITools.RotatedTexture(_crossHairRect, (Texture2D)_crossHair, _rotation);
    }

    void Update()
    {
        _crossHairRect = new Rect(mousePos.x - _cursorSize / 2, mousePos.y - _cursorSize / 2, _cursorSize, _cursorSize);
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
