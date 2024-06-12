using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public RectTransform itemBoxUI;
    public GameObject crossHair;
    private bool isPositionAtZero = false;
    private bool isOpenCrossHair = false;

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
            if (!isOpenCrossHair)
            {
                OpenCrossHair();
                isOpenCrossHair = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePosition();
            }
        }
        
    }

    /// <summary>
    /// ゲーム開始ボタン
    /// </summary>
    public void OnStartGameButtonClicked()
    {
        startUI.SetActive(false);
        GameManager.instance.StartGame();
    }

    /// <summary>
    /// ゲーム終了ボタン
    /// </summary>
    public void OnEndGameButtonClicked()
    {
        GameManager.instance.QuitGame();
    }

    /// <summary>
    /// アイテムボックスの表示非表示
    /// </summary>
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

    /// <summary>
    /// カメラの中心点の表示
    /// </summary>
    void OpenCrossHair()
    {
        crossHair.SetActive(true);
    }
}
