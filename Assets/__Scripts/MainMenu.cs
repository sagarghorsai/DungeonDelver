using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextAsset Hatdelver;
    public TextAsset Eagledelver;

    public void loadHat()
    {
        MapInfo.delverLevel = Hatdelver;
    }
    public void loadEagle()
    {
        MapInfo.delverLevel = Eagledelver;

    }

    public void Play()
    {
        SceneManager.LoadScene("__Scene_Dungeon");
        MapInfo.delverLevel = Hatdelver;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Pressed Quit");
    }
    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }


}
