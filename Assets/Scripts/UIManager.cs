using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public RectTransform itemBoxUI;
    public GameObject crossHair;
    public GameObject controlUI;
    public GameObject dieUI;
    public GameObject fireDie;
    public GameObject secondEarthDie;
    public GameObject firstEarthDie;
    public GameObject loopEnd;
    public GameObject settingPanel;
    public GameObject endPanel;
    public GameObject handItemPanel;
    public GameObject[] endingPanels;

    public GameObject memos;
    public GameObject memo1;
    public GameObject memo2;
    public GameObject memo3;
    
    public GameObject talk;

    public Text textComponent;
    bool isTalk = false;
    int currentTalkCount = 0;
    public float closeControlUITime = 7.0f;

    bool isMemoTalk = false;
    private bool isPositionAtZero = false;
    private bool isOpenCrossHair = false;

    void Start()
    {
        startUI.SetActive(true);
        controlUI.SetActive(false);
        dieUI.SetActive(false);

        if (talk != null)
        {
            textComponent = talk.GetComponentInChildren<Text>();

            if (textComponent == null)
            {
                Debug.LogError("Text component not found on the child object.");
            }
        }
        else
        {
            Debug.LogError("Talk parent object is not assigned.");
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is not set.");
        }
    }

    void Update()
    {

        if (GameManager.instance.IsGameStarted())
        {

            if (!isOpenCrossHair)
            {
                OpenCrossHair();
                isOpenCrossHair = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab) || Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                TogglePosition();
            }

            controlUI.SetActive(true);

            if(GameManager.instance.gameTime >= closeControlUITime)
            {
                controlUI.SetActive(false);
                GameManager.instance.selfSpleak_1 = true;
            }

            DieReason();
            GameEndUI();
            Memo();
            if (!isTalk)
            {
                Talk();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                talk.SetActive(false);
                isMemoTalk = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (settingPanel.activeSelf)
                {
                    CloseSettingUI();
                }
                else
                {
                    OpenSettingUI();
                }
            }

            if(GameManager.instance.isHandItemUse == true)
            {
                handItemPanel.SetActive(true);
            }
            else
            {
                handItemPanel.SetActive(false);
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

    public void OpenSettingUI()
    {
        settingPanel.SetActive(true);
        CharacterMove.instance.isGameStarted = !CharacterMove.instance.isGameStarted;
    }

    public void CloseSettingUI()
    {
        settingPanel.SetActive(false);
        CharacterMove.instance.isGameStarted = !CharacterMove.instance.isGameStarted;

    }

    /// <summary>
    /// アイテムボックスの表示非表示
    /// </summary>
    void TogglePosition()
    {
        if (isPositionAtZero)
        {
            itemBoxUI.anchoredPosition = new Vector2(itemBoxUI.anchoredPosition.x, -1500f);
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
            CharacterMove.instance.isGameStarted = false;
            dieUI.SetActive(true);
            if (GameManager.instance.isFireDie)
            {
                fireDie.SetActive(true);
            }
            else if (GameManager.instance.isSecondEarthDie)
            {
                secondEarthDie.SetActive(true);
            }
            else if (GameManager.instance.isFirstEarthDie)
            {
                firstEarthDie.SetActive(true);
            }
        }
    }


    void GameEndUI()
    {
        if(GameManager.instance.isGameEnd == true)
        {
            endPanel.SetActive(true);
            CharacterMove.instance.isGameStarted = false;
            if (GameManager.instance.isLoopEnd == true)
            {
                loopEnd.SetActive(true);
            }
            else if (GameManager.instance.isEnd_1 == true)
            {
                endingPanels[0].SetActive(true);
            }
            else if (GameManager.instance.isEnd_2 == true)
            {
                endingPanels[1].SetActive(true);
            }
            else if (GameManager.instance.isEnd_3 == true)
            {
                endingPanels[2].SetActive(true);
            }
            else if (GameManager.instance.isEnd_4 == true)
            {
                endingPanels[3].SetActive(true);
            }
            else if (GameManager.instance.isEnd_5 == true)
            {
                endingPanels[4].SetActive(true);
            }
            else if (GameManager.instance.isEnd_6 == true)
            {
                endingPanels[5].SetActive(true);
            }
            else if (GameManager.instance.isEnd_7 == true)
            {
                endingPanels[6].SetActive(true);
            }
        }
    }

    void Memo()
    {
        if (GameManager.instance.memo == 1)
        {
            memos.SetActive(true);
            memo1.SetActive(true);
        }
        else if(GameManager.instance.memo == 2)
        {
            memos.SetActive(true);
            memo2.SetActive(true);
        }
        else if (GameManager.instance.memo == 3)
        {
            memos.SetActive(true);
            memo3.SetActive(true);
        }
        else if(GameManager.instance.memo == 0)
        {

            memo1.SetActive(false);
            memo2.SetActive(false);
            memo3.SetActive(false);
            memos.SetActive(false);
        }
    }

    void Talk()
    {
        if (GameManager.instance == null)
        {
            return; // GameManager.instanceがnullなら処理を中断
        }

        if (GameManager.instance.selfSpleak_1 && currentTalkCount == 0)
        {
            SetTalkText("今日はパパもママもいないからゲームやりほうだいだ！はやくリビングでゲームをやろう！なんだろう、メモがおいてある");
            currentTalkCount++;
        }
        else if (GameManager.instance.selfSpleak_2 && isMemoTalk)
        {
            SetTalkText("さいあくだ…せっかくの休みなのに！… 何としてもゲームを見つけなきゃ");
            GameManager.instance.selfSpleak_2 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_3 && isMemoTalk)
        {
            SetTalkText("スマホを見つけた！ママは用心ぶかいからゲームはべつの場所にかくしてるみたい");
            GameManager.instance.selfSpleak_3 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_4 && isMemoTalk)
        {
            SetTalkText("ゲームは見つけたけど…ママが帰ってきたら怒るかな…");
            GameManager.instance.selfSpleak_4 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_5 && currentTalkCount == 1)
        {
            SetTalkText("びっくりした…とりあえずじしんはおさまったみたい");
            currentTalkCount++;
        }
    }


    void SetTalkText(string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
            talk.SetActive(true);
            isTalk = true;

            // 5秒後に非表示にするコルーチンを開始
            StartCoroutine(HideTalkAfterDelay(5f));
        }
    }

    IEnumerator HideTalkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        talk.SetActive(false);
        isTalk = false;
    }
}
