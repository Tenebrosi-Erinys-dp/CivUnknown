using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Start()
    {
        instance = this;
    }
    public void GameOver()
    {
        SceneManager.LoadSceneAsync("Game Over");
    }
}
