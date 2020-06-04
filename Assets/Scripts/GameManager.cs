using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<EnemyController> enemies;

    private void Start()
    {
        instance = this;
        enemies = FindObjectsOfType<EnemyController>().ToList();
    }

    public void CheckGameWin()
    {
        bool isAllEmpty = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].exists)
            {
                isAllEmpty = false;
            }
        }

        if (isAllEmpty)
        {
            SceneManager.LoadSceneAsync("Game Won");
        }
    }


    public void GameOver()
    {
        SceneManager.LoadSceneAsync("Game Over");
    }
}
