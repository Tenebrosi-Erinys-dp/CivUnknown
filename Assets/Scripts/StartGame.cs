using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GameObject.Find("Button").AddComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeScene() {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
