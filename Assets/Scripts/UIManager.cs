using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public RectTransform itemBoxUI;
    public GameObject crossHair;
    public GameObject controlUI;
    public GameObject dieUI;
    public GameObject fireDie;
    public GameObject earthDie;
    public GameObject gameClear;

    public float closeControlUITime = 7.0f;

    private bool isPositionAtZero = false;
    private bool isOpenCrossHair = false;

    // Start is called before the first frame update
    void Start()
    {
        startUI.SetActive(true);
        controlUI.SetActive(false);
        dieUI.SetActive(false);
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

            controlUI.SetActive(true);

            if(GameManager.instance.gameTime >= closeControlUITime)
            {
                controlUI.SetActive(false);
            }

            DieReason();
            Clear();
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


    void DieReason()
    {
        if(GameManager.instance.isPlayerDead == true)
        {
            dieUI.SetActive(true);
            if (GameManager.instance.isFireDie)
            {
                fireDie.SetActive(true);
            }
            else if (GameManager.instance.isEarthDie)
            {
                earthDie.SetActive(true);
            }
        }
    }

    void Clear()
    {
        if(GameManager.instance.isGameClear == true)
        {
            gameClear.SetActive(true);
        }
    }
}
