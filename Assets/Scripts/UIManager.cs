using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;

    // Start is called before the first frame update
    void Start()
    {
        startUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGameButtonClicked()
    {
        startUI.SetActive(false);
        GameManager.instance.StartGame();
    }

    public void OnEndGameButtonClicked()
    {
        GameManager.instance.QuitGame();
    }
}
