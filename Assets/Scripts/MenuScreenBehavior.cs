using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScreenBehavior : MonoBehaviour
{
    // Variable declarations and definitions
    private Random random = new Random(); // For later use if random loading time is desired
    public float timer = 0.0f; // Timer for initial screens
    private float splashScreenSeconds = 5.0f;
    private float loadingScreenSeconds = 5.0f;
    private float gameScreenSeconds = 20.0f;
    private float exitScreenSeconds = 5.0f;

    // Struct for screen timing
    struct screenValues
    {
        // break point
        

        public string sceneName; // Name of the scene associated with the screen
        public bool isTimed; // bool defining whether or not the screen is timed or not
        public bool isAdditive; // bool defining whether the screen should be loaded additively

        // Constructor for initializing 
        public screenValues(string sceneName, bool is_timed, bool is_additive)
        {
            this.sceneName = sceneName;
            this.isTimed = is_timed;
            this.isAdditive = is_additive;
            Debug.Log("screenValues Started");
        }
    }

    // dictionary that maps string values to the screen they represent
    private Dictionary<string, screenValues> screenToValuesDict;

    // true if current screen is timed, false if it is untimed
    private bool isCurrentScreenTimed;
    private string currentScreen = "null"; // initial screen value of "null"

    /* FUNCTION DEFINITIONS */

    void setCurrentScreen(Dictionary<string, screenValues> screen_dict, string screen)
    {
        foreach (var pair in screen_dict)
        {
            if (pair.Key == screen)
            {
                if (pair.Value.isAdditive)
                {
                    SceneManager.LoadSceneAsync(pair.Value.sceneName, LoadSceneMode.Additive);
                    Debug.Log("Loading scene additively: " + pair.Value.sceneName);
                }
                else
                {
                    SceneManager.LoadSceneAsync(pair.Value.sceneName, LoadSceneMode.Single);
                    Debug.Log("Loading scene singularly: " + pair.Value.sceneName);
                }
            }
        }

        timer = 0.0f;
        isCurrentScreenTimed = screen_dict[screen].isTimed;
        currentScreen = screen; // Update currentScreen here
        Debug.Log("setCurrentScreen is now: " + currentScreen);
    }

    screenValues MakeScreenValueStruct(string sceneName, bool is_timed, bool is_additive)
    {
        Debug.Log("MakeScreenValueStruct Started");
        
        /*
        This is a helper function which takes in a scene name and a bool to create a screenValues
        struct. This is done because it makes the objective of creating a screenValues struct 
        much easier to read and much more concise. This function returns a screenValues structure.
        Args:
            sceneName (string) : a string representing a screen scene
            is_timed (bool) : a boolean value representing whether the same screen is timed
            is_additive (bool) : a boolean value representing whether the screen should be loaded additively
        Returns:
            screen_values (screenValues) : a screenValues struct with fields populated using sceneName, is_timed, and is_additive
        */

        screenValues screen_values = new screenValues(sceneName, is_timed, is_additive);
        return screen_values;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Void Start");
        /*
        This function is called only once by the Unity engine before the first frame update. In this
        implementation, it initializes certain variables before the start of the start of the menu-scene
        Unity Scene. This function takes no arguments for input and returns nothing.
        */

        // initializing a dictionary mapping each screen to a struct containing its values
        screenToValuesDict = new Dictionary<string, screenValues>() {
            {"main", MakeScreenValueStruct("mainScreen", false, false)},
            {"splash",  MakeScreenValueStruct("splashScreen", true, true)},
            {"loading", MakeScreenValueStruct("loadingScreen", true, true)},
            {"game", MakeScreenValueStruct("gameScreen", true, true)},
            // {"options", MakeScreenValueStruct("optionsMenuScreen", false, false)}, // Menu screens to be implemented later
            // {"exit", MakeScreenValueStruct("exitScreen", true, false)}
        };

        currentScreen = "main";
        setCurrentScreen(screenToValuesDict, currentScreen); // starts the main scene and sets values
        Debug.Log("currentScreen = mainScreen");

        // Button implementation to follow

        // exitButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "exit"));
        // optionsButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "options"));
        // optionsBackButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "main"));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        This function is called by the Unity engine once per frame. In this implementation,
        it is used to time the screen in the scene and change them using the display time
        variables defined within this script. This function takes no arguments and returns nothing.
        */
        Debug.Log("Void Update");

        // if (isCurrentScreenTimed)
        // {
            // only updates timer on timed screens
            // timer += Time.deltaTime;
            // Debug.Log("Timer Started ");
        // }
        
        // dynamically changing the visible screen based on the timer
        switch (currentScreen)
        {
            case "main":
            Debug.Log("Switch Started");
                timer += Time.deltaTime;
                Debug.Log("Timer Started ");
                Debug.Log("Timer: " + timer);
                if (timer >= splashScreenSeconds)
                {
                    setCurrentScreen(screenToValuesDict, "splash");
                    Debug.Log("Loading splash screen");
                }
                break;
            case "splash":
                if (timer >= loadingScreenSeconds)
                {
                    setCurrentScreen(screenToValuesDict, "loading");
                    SceneManager.UnloadSceneAsync("splashScreen"); // Unload splash screen when loading screen is loaded
                    Debug.Log("Loading loading screen");
                }
                break;
            case "loading":
                if (timer >= gameScreenSeconds)
                {
                    setCurrentScreen(screenToValuesDict, "game");
                    SceneManager.UnloadSceneAsync("loadingScreen"); // Unload loading screen when game screen is loaded
                    Debug.Log("Loading game screen");
                }
                break;
            case "game":
                if (timer >= exitScreenSeconds)
                {
                    setCurrentScreen(screenToValuesDict, "exit");
                    SceneManager.UnloadSceneAsync("gameScreen"); // Unload game screen when exit screen is loaded
                    Debug.Log("Loading exit screen");
                }
                break;
            case "exit":
                if (timer >= exitScreenSeconds)
                {
                    Application.Quit();
                    Debug.Log("Quitting application");
                }
                break;
        }
    }
}

