using System.Collections;
using System.Collections.Generic;
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
    public GameObject earthDie;
    public GameObject gameClear;

    public GameObject memos;
    public GameObject memo1;
    public GameObject memo2;
    public GameObject memo3;

    public GameObject talk;

    public Text textComponent;
    public bool isTalk = false;

    private int talkCount = 0;

    public float closeControlUITime = 7.0f;

    private bool isPositionAtZero = false;
    private bool isOpenCrossHair = false;
    private bool istalk = false;

    // Start is called before the first frame update
    void Start()
    {
        startUI.SetActive(true);
        controlUI.SetActive(false);
        dieUI.SetActive(false);

        // 子オブジェクトにアタッチされたTextコンポーネントを取得
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

        // GameManager.instanceがnullかどうか確認
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is not set.");
        }
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

            if (Input.GetKeyDown(KeyCode.Tab) || Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                TogglePosition();
            }

            controlUI.SetActive(true);

            if(GameManager.instance.gameTime >= closeControlUITime)
            {
                controlUI.SetActive(false);
                GameManager.instance._1Talk = true;
            }

            DieReason();
            Clear();
            Memo();
            if (!istalk)
            {
                Talk();
            }
            

            if (istalk)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
                {
                    talk.SetActive(false);
                    istalk = false;
                }
                
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

        if (GameManager.instance._1Talk && talkCount ==0)
        {
            SetTalkText("今日はパパもママもいないからゲームやりほうだいだ！はやくリビングでゲームをやろう！なんだろう、メモがおいてある");
            isTalk = true;
            talkCount ++;
        }
        else if (GameManager.instance._2Talk && talkCount == 1)
        {
            SetTalkText("さいあくだ…せっかくの休みなのに！… 何としてもゲームを見つけなきゃ");
            isTalk = true;
            talkCount++;
        }
        else if (GameManager.instance._3Talk && talkCount == 2)
        {
            SetTalkText("スマホを見つけた！ママは用心ぶかいからゲームはべつの場所にかくしてるみたい");
            isTalk = true;
            talkCount++;
        }
        else if (GameManager.instance._4Talk && talkCount == 3)
        {
            SetTalkText("ゲームは見つけたけど…ママが帰ってきたら怒るかな…");
            isTalk = true;
            talkCount++;
        }
        else if (GameManager.instance._5Talk && talkCount == 4)
        {
            SetTalkText("びっくりした…とりあえずじしんはおさまったみたい");
            isTalk = true;
            talkCount++;
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
