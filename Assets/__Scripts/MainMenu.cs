using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    MapInfo map = new MapInfo();
    public TextAsset Hatdelver;
    public TextAsset Eagledelver;

    public void loadHat()
    {
       map.delverLevel = Hatdelver;
    }
    public void loadEagle()
    {
        map.delverLevel = Eagledelver;

    }

    public void Play()
    {
        map.delverLevel = Hatdelver;
        SceneManager.LoadScene("__Scene_Dungeon");
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

    public void easyDifficulty()
    {
        Skeletos.maxHealth = 2;
        Slime.maxHealth = 1;
        Bat.maxHealth = 1;
        Spiker.maxHealth = 1;
    }
    public void mediumDifficulty()
    {
        Skeletos.maxHealth = 4;
        Slime.maxHealth = 2;
        Bat.maxHealth = 2;
        Spiker.maxHealth = 2;
    }
    public void hardDifficulty()
    {
        Skeletos.maxHealth = 6;
        Slime.maxHealth = 4;
        Bat.maxHealth = 4;
        Spiker.maxHealth = 4;
    }


}
