using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomExtensions;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour, IManager
{
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    //1
    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;

    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int _itemsCollected = 0;
    public int Items
    {
        //2
        get { return _itemsCollected; }
        //3
        set
        {
            _itemsCollected = value;
            //2
            if(_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                //2
                showWinScreen = true;

                //2
                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Item found, only" + (maxItems - _itemsCollected) + " more to go!";
            }
        }
    }
    private int _playerHP = 10;
    //4
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            Debug.LogFormat("Lives: {0}", _playerHP);
            if(_playerHP <= 0)
            {
                labelText = "You want another life with that?";
                showLossScreen = true;
                Time.timeScale = 0;
            }
            else
            {
                labelText = "Ouch... that's got to hurt.";
            }
        }
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;
    //3
    void Start()
    {
        Initialize();
        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);
    }
    public Stack<string> lootStack = new Stack<string>();
    public void Initialize()
    {
        _state = "Manager initialized...";
        _state.FancyDebug();
        debug(_state);
        LogWithDelegate(debug);
        GameObject player = GameObject.Find("Player");
        PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
        playerBehavior.playerJump += HandlePlayerJump;
        Debug.Log(_state);
        lootStack.Push("Sword of Doom");
        lootStack.Push("HP+");
        lootStack.Push("Golden Key");
        lootStack.Push("Winged Boot");
        lootStack.Push("Mythril Bracers");
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the debug task...");
    }

    void OnGUI()
    {
        //4
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health:" + _playerHP);
        //5
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + _itemsCollected);
        //6
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
        //3
        if (showWinScreen)
        {
            //4
            if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 100), "YOU WON!"))
            {
                //3
                SceneManager.LoadScene(0);

                //4
                Time.timeScale = 1.0f;
                Utilities.RestartLevel(0);
            }
        }
        if(showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width /2 -100, Screen.height /2 - 50, 200, 100), "You lose..."))
            {
                try
                {
                    Utilities.RestartLevel(-1);
                    debug("level restarted successfully...");
                }
                catch (System.ArgumentException e)
                {
                    Utilities.RestartLevel(0);
                    debug("Reverting to scene 0:" + e.ToString());
                }
                finally
                {
                    debug("Restart handled...");
                }
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;
            }
        }
    }
    public void PrintLootReport()
    {
        var currentItem = lootStack.Pop();
        var nextItem = lootStack.Peek();
        Debug.LogFormat("You got a {0}! You've got a good chance of finding a {1} next!", currentItem, nextItem);
        Debug.LogFormat("There are {0} random loot items waiting for you!", lootStack.Count);
    }
}
