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
    private float exitScreenSeconds = 5.0f;

    // Struct for screen timing
    struct screenValues
    {
        public string sceneName; // Name of the scene associated with the screen
        public bool isTimed; // bool defining whether or not the screen is timed or not

        // Constructor for initializing 
        public screenValues(string sceneName, bool is_timed)
        {
            this.sceneName = sceneName;
            this.isTimed = is_timed;
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
        /*
        This function takes in a dictionary mapping strings to screen scenes and a string.
        It uses the string to set the corresponding scene from the dictionary to active
        and deactivates every other scene in the dictionary.
        Args:
            screen_dict (Dictionary<string, screenValues>) : a dictionary mapping strings to screen scenes
            screen (string) : the scene to be set active
        This function returns nothing. 
        */
        foreach (var pair in screen_dict)
        {
            if (pair.Key == screen)
            {
                SceneManager.LoadSceneAsync(pair.Value.sceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadSceneAsync(pair.Value.sceneName);
            }
        }
        timer = 0.0f; // resets the timer to be used for the next timed screen
        isCurrentScreenTimed = screen_dict[screen].isTimed; // starts timer again if the next screen is timed
    }

    screenValues MakeScreenValueStruct(string sceneName, bool is_timed)
    {
        /*
        This is a helper function which takes in a scene name and a bool to create a screenValues
        struct. This is done because it makes the objective of creating a screenValues struct 
        much easier to read and much more concise. This function returns a screenValues structure.
        Args:
            sceneName (string) : a string representing a screen scene
            is_timed (bool) : a boolean value representing whether the same screen is timed
        Returns:
            screen_values (screenValues) : a screenValues struct with fields populated using sceneName and is_timed
        */
        screenValues screen_values = new screenValues(sceneName, is_timed);
        return screen_values;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        This function is called only once by the Unity engine before the first frame update. In this
        implementation, it initializes certain variables before the start of the start of the menu-scene
        Unity Scene. This function takes no arguments for input and returns nothing.
        */
        // initializing a dictionary mapping each screen to a struct containing its values
        screenToValuesDict = new Dictionary<string, screenValues>() {
            {"splash",  MakeScreenValueStruct("SplashScreen", true)},
            {"loading", MakeScreenValueStruct("LoadingScreen", true)},
            {"main", MakeScreenValueStruct("mainScreen", true)},
            // {"options", MakeScreenValueStruct("OptionsMenuScreen", false)}, // Menu screens to be implemented later
            // {"exit", MakeScreenValueStruct("ExitScreen", true)}
        };

        currentScreen = "main";
        setCurrentScreen(screenToValuesDict, currentScreen); // starts the menu-scene scene displaying only the splash screen

        // Button implementation to follow

        // exitButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "exit"));
        // optionsButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "options"));
        // optionsBackButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "main"));
        // skipLoadingButton.onClick.AddListener(() => setCurrentScreen(screenToValuesDict, "main"));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        This function is called by the Unity engine once per frame. In this implementation,
        it is used to time the screen in the scene and change them using the display time
        variables defined within this script. This function takes no arguments and returns nothing.
        */

        if (isCurrentScreenTimed)
        {
            // only updates timer on timed screens
            timer += Time.deltaTime;
        }

        // dynamically changing the visible screen based on the timer
        switch (currentScreen)
        {
            case "main":
                if (timer >= splashScreenSeconds)
                {
                    currentScreen = "splash";
                    setCurrentScreen(screenToValuesDict, currentScreen);
                }
                break;
            case "splash":
                if (timer >= loadingScreenSeconds)
                {
                    currentScreen = "loading";
                    setCurrentScreen(screenToValuesDict, currentScreen);
                }
                break;
            case "exit":
                if (timer >= exitScreenSeconds)
                {
                    Application.Quit();
                }
                break;
        }
    }
}
