using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public RectTransform itemBoxUI;
    private bool isPositionAtZero = true;

    // Start is called before the first frame update
    void Start()
    {
        startUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGameStarted())
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePosition();
            }
        }
        
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

    void TogglePosition()
    {
        if (isPositionAtZero)
        {
            itemBoxUI.anchoredPosition = new Vector2(itemBoxUI.anchoredPosition.x, -150f);
        }
        else
        {
            itemBoxUI.anchoredPosition = new Vector2(itemBoxUI.anchoredPosition.x, 0f);
        }

        isPositionAtZero = !isPositionAtZero;
    }
}
