using UnityEngine;
using System.Collections;

public class ProgressInfo : MonoBehaviour 
{
    // Skin for score and multiplier
    public GUISkin Skin;

    // Border width of progress and hp bars
    private const int LINE_WIDTH = 3;

    // Textures for ui
    private Texture2D _progressFore;
    private Texture2D _progressBack;
    private Texture2D _healthFore;
    private Texture2D _healthBack;
    private Texture2D _line;
    private Texture2D _dialBack;
    private Texture2D _arrowDown;
    private Texture2D _arrowUp;
    private Texture2D _dial;

    // Locations of progress and health bars
    private Rect _progressRect;
    private Rect _healthRect;

    // Locations of heading and velocity dials
    private Rect _headingRect;
    private Rect _velocityRect;

    // Height offset for ui elements
    private float _unitHeight;

    // These get disabled when game is over
    private GameObject _main;
    private GameObject _turret1;
    private GameObject _turret2;
    private GameObject _turret3;

    private Vector2 prevScreen;

    void Start()
    {
        // Get ahold of objects for end of game state
        _main = GameObject.Find("First Person Controller");
        _turret1 = GameObject.Find("turret1");
        _turret2 = GameObject.Find("turret2");
        _turret3 = GameObject.Find("turret3");

        // Load dial image and arrows
        _dialBack = Resources.Load<Texture2D>("Textures/dial");
        _arrowDown = Resources.Load<Texture2D>("Textures/arrowDown");
        _arrowUp = Resources.Load<Texture2D>("Textures/arrowUp");

        // Initializes textures used
        _progressFore = new Texture2D(1, 1);
        _progressBack = new Texture2D(1, 1);
        _healthFore = new Texture2D(1, 1);
        _healthBack = new Texture2D(1, 1);
        _line = new Texture2D(1, 1);
        _dial = new Texture2D(1, 1);

        // Sets texture colors
        _progressFore.SetPixel(0, 0, Color.green);
        _progressBack.SetPixel(0, 0, Color.gray);
        _healthFore.SetPixel(0, 0, Color.red);
        _healthBack.SetPixel(0, 0, Color.gray);
        _line.SetPixel(0, 0, Color.black);
        _dial.SetPixel(0, 0, Color.white);

        // Applys colors to textures
        _progressFore.Apply();
        _progressBack.Apply();
        _healthFore.Apply();
        _healthBack.Apply();
        _line.Apply();
        _dial.Apply();

        _unitHeight = Screen.height / 25 * 0.5f;
        // Location of progress bar on screen
        _progressRect = new Rect(Screen.width * 0.25f, _unitHeight, Screen.width / 2, Screen.height / 25);
        // Location of health bar on screen
        _healthRect = new Rect(Screen.width * 0.25f, _unitHeight + _unitHeight * 2.4f, Screen.width / 2, Screen.height / 25);

        // Locations of heading and velocity dials
        _headingRect = new Rect(20, 20, Screen.width / 6, Screen.height / 6);
        _velocityRect = new Rect(Screen.width - (Screen.width / 6 + 20), 20, Screen.width / 6, Screen.height / 6);

        prevScreen = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.LoadLevel("Start");

        if (GameController.Instance.GameOver)
        {
            _main.SetActive(false);
            _turret1.SetActive(false);
            _turret1.SetActive(false);
            _turret1.SetActive(false);
        }

        if (prevScreen.x != Screen.width || prevScreen.y != Screen.height)
        {
            prevScreen = new Vector2(Screen.width, Screen.height);

            _unitHeight = Screen.height / 25 * 0.5f;
            // Location of progress bar on screen
            _progressRect = new Rect(Screen.width * 0.25f, _unitHeight, Screen.width / 2, Screen.height / 25);
            // Location of health bar on screen
            _healthRect = new Rect(Screen.width * 0.25f, _unitHeight + _unitHeight * 2.4f, Screen.width / 2, Screen.height / 25);

            // Locations of heading and velocity dials
            _headingRect = new Rect(20, 20, Screen.width / 6, Screen.height / 6);
            _velocityRect = new Rect(Screen.width - (Screen.width / 6 + 20), 20, Screen.width / 6, Screen.height / 6);
        }
    }

    void OnGUI()
    {
        // If comm room is down disable its viewing by blacking it out
        if (GameController.Instance.RoomStatus[GameController.ShipRoom.COMMUNICATIONS] && !GameController.Instance.GameOver)
        {
            GUI.color = Color.black;
        }
         
        // Draws progress and health bars
        GUITools.progressBar(_progressFore, _progressBack, _line, LINE_WIDTH, _progressRect, GameController.Instance.Progress);
        GUITools.progressBar(_healthFore, _healthBack, _line, LINE_WIDTH, _healthRect, GameController.Instance.ShipHealth);

        // Gets text size (width and height) of progress and health bar labels
        Vector2 progressTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Progress"));
        Vector2 healthTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Ship Health"));

        // Creates a rect centered in the progress bar rect
        Rect progressTextRect = new Rect(_progressRect.x + (_progressRect.width / 2 - progressTextSize.x / 2), 
            _progressRect.y + (_progressRect.height / 2 - progressTextSize.y / 2), progressTextSize.x, progressTextSize.y);

        // Creates a rect centered in the health bar rect
        Rect healthTextRect = new Rect(_healthRect.x + (_healthRect.width / 2 - healthTextSize.x / 2), 
            _healthRect.y + (_healthRect.height / 2 - healthTextSize.y / 2), healthTextSize.x, healthTextSize.y); 

        // Draws the labels over the progress and health bars
        GUI.Label(progressTextRect, "Progress");
        GUI.Label(healthTextRect, "Ship Health");

        // Draws the dial backgrounds for heading and velocity
        GUI.DrawTexture(_headingRect, _dialBack);
        GUI.DrawTexture(_velocityRect, _dialBack);

        // Calculates and draws labels for dials
        Vector2 headingTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Heading"));
        Vector2 velocityTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Velocity"));
        Rect headingTextRect = new Rect(_headingRect.x + (_headingRect.width / 2 - headingTextSize.x / 2),
            _headingRect.y + _headingRect.height - (headingTextSize.y + 6), headingTextSize.x, headingTextSize.y);
        Rect velocityTextRect = new Rect(_velocityRect.x + (_velocityRect.width / 2 - velocityTextSize.x / 2),
            _velocityRect.y + _velocityRect.height - (velocityTextSize.y + 6), velocityTextSize.x, velocityTextSize.y);
        GUI.Label(headingTextRect, "Heading");
        GUI.Label(velocityTextRect, "Velocity");

        // Draw dial increase/decrease notifiers
        if (!GameController.Instance.RoomStatus[GameController.ShipRoom.CONTROL])
            GUI.DrawTexture(new Rect(headingTextRect.x + headingTextRect.width + 5, _headingRect.y + _headingRect.height - (Screen.height / 40 + 8), Screen.width / 35, Screen.height / 40), _arrowUp);
        else
            GUI.DrawTexture(new Rect(headingTextRect.x - (Screen.width / 35 + 5), _headingRect.y + _headingRect.height - (Screen.height / 40 + 8), Screen.width / 35, Screen.height / 40), _arrowDown);
        if (!GameController.Instance.RoomStatus[GameController.ShipRoom.ENGINE] && !GameController.Instance.RoomStatus[GameController.ShipRoom.POWER])
            GUI.DrawTexture(new Rect(velocityTextRect.x + velocityTextRect.width + 5, _headingRect.y + _headingRect.height - (Screen.height / 40 + 8), Screen.width / 35, Screen.height / 40), _arrowUp);
        else
            GUI.DrawTexture(new Rect(velocityTextRect.x - (Screen.width / 35 + 5), _headingRect.y + _headingRect.height - (Screen.height / 40 + 8), Screen.width / 35, Screen.height / 40), _arrowDown);

        // Draw labels for score and multiplier
        GUISkin current = GUI.skin;
        Color currentColor = GUI.color;
        GUI.skin = Skin;
        GUI.color = Color.green;
        GUI.Label(new Rect(Screen.width * 0.25f, _unitHeight + _unitHeight * 2.4f * 2, 400, 100), "Score: " + GameController.Instance.Score);
        GUI.Label(new Rect(Screen.width * 0.25f * 2, _unitHeight + _unitHeight * 2.4f * 2, 400, 100), "Multiplier: " + GameController.Instance.Multiplier + "x");
        GUI.skin = current;
        GUI.color = currentColor;

        // Calculate and draw dials
        Vector2 headingDialOrigin = new Vector2(_headingRect.x + _headingRect.width / 2 - 4, _headingRect.y + _headingRect.height * 0.76f);
        Vector2 velocityDialOrigin = new Vector2(_velocityRect.x + _velocityRect.width / 2 - 4, _velocityRect.y + _velocityRect.height * 0.76f);
        float hDegree = GameController.Instance.Heading;
        float vDegree = GameController.Instance.Velocity;
        int headingX = (int)(80 * Mathf.Cos(Mathf.PI * hDegree));
        int headingY = (int)(80 * Mathf.Sin(Mathf.PI * hDegree));
        int velocityX = (int)(80 * Mathf.Cos(Mathf.PI * vDegree));
        int velocityY = (int)(80 * Mathf.Sin(Mathf.PI * vDegree));
        Vector2 headingEnd = new Vector2(headingDialOrigin.x - headingX, headingDialOrigin.y - headingY);
        Vector2 velocityEnd = new Vector2(velocityDialOrigin.x - velocityX, velocityDialOrigin.y - velocityY);
        GUITools.DrawLine(headingDialOrigin, headingEnd, _dial, 8);
        GUITools.DrawLine(velocityDialOrigin, velocityEnd, _dial, 8);

        // Prints game over state
        if (GameController.Instance.GameOver)
        {
            current = GUI.skin;
            currentColor = GUI.color;
            GUI.skin = Skin;
            if (GameController.Instance.ShipHealth == 0)
            {
                GUI.color = Color.red;
                Vector2 gameOver = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("YOU LOSE!"));
                GUI.Label(new Rect(Screen.width / 2 - gameOver.x / 2, Screen.height / 2 - gameOver.y / 2, gameOver.x, gameOver.y), "YOU LOSE!");
            }
            else
            {
                GUI.color = Color.green;
                Vector2 gameOver = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("YOU WIN!"));
                GUI.Label(new Rect(Screen.width / 2 - gameOver.x / 2, Screen.height / 2 - gameOver.y / 2, gameOver.x, gameOver.y), "YOU WIN!");
            }
            GUI.skin = current;
            GUI.color = currentColor;

            if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
                Application.LoadLevel("Start");
        }
    }
}
