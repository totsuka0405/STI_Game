using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 画面UIの表示・非表示およびテキスト更新を担当するプレゼンテーション層の管理クラス。
/// ゲーム進行の状態（GameManager）を参照し、UIの見せ方に限定して制御する。
/// </summary>
public class UIManager : MonoBehaviour
{
    // ====== 参照（Inspector） ======
    [SerializeField] private GameObject startUI;        // タイトル（開始）UI
    [SerializeField] private RectTransform itemBoxUI;   // インベントリUI（スライド表示）
    [SerializeField] private GameObject crossHair;      // 画面中央の照準
    [SerializeField] private GameObject controlUI;      // 操作説明UI
    [SerializeField] private GameObject dieUI;          // 共通の死亡UIルート
    [SerializeField] private GameObject fireDie;        // 死因：火災
    [SerializeField] private GameObject secondEarthDie; // 死因：二度目の地震
    [SerializeField] private GameObject firstEarthDie;  // 死因：一度目の地震
    [SerializeField] private GameObject loopEnd;        // ループエンド表示
    [SerializeField] private GameObject settingPanel;   // 設定パネル
    [SerializeField] private GameObject endPanel;       // エンディングルート
    [SerializeField] private GameObject handItemPanel;  // 手持ちアイテム表示
    [SerializeField] private GameObject[] endingPanels; // 各エンディング個別パネル
    [SerializeField] private GameObject curor;          // エンディング画面のカーソル
    [SerializeField] private GameObject memos;          // メモUIルート
    [SerializeField] private GameObject memo1;          // メモ1
    [SerializeField] private GameObject memo2;          // メモ2
    [SerializeField] private GameObject memo3;          // メモ3
    [SerializeField] private GameObject talk;           // トークUIルート

    // ====== トーク表示 ======
    [SerializeField] private Text textComponent; // talk の子にある Text。未割り当て時は Start で自動取得を試行
    [SerializeField, Tooltip("単発トークを自動で閉じるまでの秒数")]
    private float talkAutoHideSeconds = 5f;

    // ---- 自己セリフ（selfSpleak_* 対応） ----
    [Header("Self Talk Messages (Inspector Editable)")]
    [SerializeField, TextArea]
    private string selfSpeak1Text =
        "今日はパパもママもいないからゲームやりほうだいだ！はやくリビングでゲームをやろう！なんだろう、メモがおいてある";
    [SerializeField, TextArea]
    private string selfSpeak2Text =
        "さいあくだ…せっかくの休みなのに！… 何としてもゲームを見つけなきゃ";
    [SerializeField, TextArea]
    private string selfSpeak3Text =
        "スマホを見つけた！ママは用心ぶかいからゲームはべつの場所にかくしてるみたい";
    [SerializeField, TextArea]
    private string selfSpeak4Text =
        "ゲームは見つけたけど…ママが帰ってきたら怒るかな…";
    [SerializeField, TextArea]
    private string selfSpeak5Text =
        "びっくりした…とりあえずじしんはおさまったみたい";

    // ---- イベントセリフ：電話 ----
    [Header("Phone Call Messages")]
    [SerializeField, TextArea]
    private string[] callPhoneMessages =
    {
        "ママに電話をかけた",
        "ママ「もしもし、どうかしたの？」",
        "ママ「え？ひなんじょの場所？急にどうしたの、青山小だけど…",
        "うちの家族の集合場所は青山小学校のようだ"
    };
    [SerializeField, Tooltip("電話イベントの各メッセージ表示時間（秒）")]
    private float callPhoneMessageDelay = 5.0f;

    // ---- イベントセリフ：地図確認 ----
    [Header("Map Watch Messages")]
    [SerializeField, TextArea]
    private string[] mapWatchMessages =
    {
        "自分の家の近くのひなんじょの場所をかくにんした",
        "このあたりだと青山小学校と赤木小学校がひなんじょになっているらしい",
    };
    [SerializeField, Tooltip("地図イベントの各メッセージ表示時間（秒）")]
    private float mapWatchMessageDelay = 3.0f;

    // ====== 操作ヘルプ ======
    [SerializeField, Tooltip("操作説明UIを自動で閉じるまでの秒数")]
    private float closeControlUITime = 7.0f;

    // ====== 内部状態 ======
    private bool isTalk = false;           // トークUIの表示状態
    private int currentTalkCount = 0;      // 自己セリフの進行管理
    private bool isMemoTalk = false;       // メモ閲覧中の「次へ」トリガー
    private bool isPositionAtZero = false; // インベントリの開閉状態
    private bool isOpenCrossHair = false;  // クロスヘアの初回表示フラグ
    private bool isCallEventEnd = false;   // 電話イベントの単発制御
    private bool isMapEventEnd = false;    // 地図イベントの単発制御
    private bool isFirstControlEvent = false;

    private void Start()
    {
        SafeSetActive(startUI, true);
        SafeSetActive(controlUI, false);
        SafeSetActive(dieUI, false);

        // トークテキストの自動取得（Inspector 未設定時の保険）
        if (talk != null && textComponent == null)
        {
            textComponent = talk.GetComponentInChildren<Text>();
            if (textComponent == null)
            {
                Debug.LogError("Text component not found on the child object of Talk.");
            }
        }
        else if (talk == null)
        {
            Debug.LogError("Talk parent object is not assigned.");
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance is not set.");
        }
    }

    private void Update()
    {
        // ゲーム進行中のみUI更新を行う
        if (GameManager.instance == null || !GameManager.instance.IsGameStarted())
            return;

        OpenCrossHairOnce();
        HandleItemBoxToggleInput();
        HandleMemoTalkTriggerInput();
        HandleSettingToggleInput();
        HandleControlUIHelp();

        DieReason();
        GameEndUI();
        Memo();

        // トーク表示が空いている時のみ新規表示を開始する
        if (!isTalk)
        {
            Talk();
            EventTalk();
        }

        // 手持ちアイテムパネルの表示切り替え
        if (handItemPanel != null)
        {
            bool show = GameManager.instance.isHandItemUse == true;
            SafeSetActive(handItemPanel, show);
        }
    }

    // ====== ゲーム開始/終了（外部UIからのイベント） ======

    /// <summary>開始ボタン：開始UIを閉じ、ゲーム進行を開始する。</summary>
    public void OnStartGameButtonClicked()
    {
        SafeSetActive(startUI, false);
        GameManager.instance?.StartGame();
    }

    /// <summary>終了ボタン：アプリケーションを終了する。</summary>
    public void OnEndGameButtonClicked()
    {
        GameManager.instance?.QuitGame();
    }

    // ====== 設定パネル ======

    /// <summary>設定パネルを開く。</summary>
    private void OpenSettingUI() => SafeSetActive(settingPanel, true);

    /// <summary>
    /// 設定パネルを閉じる。ゲーム進行中であればプレイヤー操作可否をトグルする。
    /// </summary>
    private void CloseSettingUI()
    {
        SafeSetActive(settingPanel, false);
        if (GameManager.instance != null && GameManager.instance.IsGameStarted())
        {
            if (CharacterMove.instance != null)
            {
                CharacterMove.instance.isGameStarted = !CharacterMove.instance.isGameStarted;
            }
        }
    }

    // ====== 入力ハンドリング ======

    /// <summary>インベントリ表示のトグル入力（Tab / Pad West）を処理する。</summary>
    private void HandleItemBoxToggleInput()
    {
        var pad = Gamepad.current;
        bool pressed = Input.GetKeyDown(KeyCode.Tab)
                       || (pad != null && pad.buttonWest.wasPressedThisFrame);
        if (pressed) TogglePosition();
    }

    /// <summary>メモ閲覧中のセリフ進行入力（Space / Pad East）を処理する。</summary>
    private void HandleMemoTalkTriggerInput()
    {
        var pad = Gamepad.current;
        bool pressed = Input.GetKeyDown(KeyCode.Space)
                       || (pad != null && pad.buttonEast.wasPressedThisFrame);
        if (pressed) isMemoTalk = true;
    }

    /// <summary>設定パネルの開閉入力（Esc / Pad Start）を処理する。</summary>
    private void HandleSettingToggleInput()
    {
        var pad = Gamepad.current;
        bool pressed = Input.GetKeyDown(KeyCode.Escape)
                       || (pad != null && pad.startButton.wasPressedThisFrame);
        if (!pressed) return;

        if (settingPanel != null && settingPanel.activeSelf)
        {
            CloseSettingUI();
            if (CharacterMove.instance != null)
                CharacterMove.instance.isGameStarted = true;
        }
        else
        {
            OpenSettingUI();
            if (CharacterMove.instance != null)
                CharacterMove.instance.isGameStarted = false;
        }
    }

    // ====== UI 表示ロジック ======

    /// <summary>インベントリUIの位置を切り替え、開閉を表現する。</summary>
    private void TogglePosition()
    {
        if (itemBoxUI == null) return;
        float nextY = isPositionAtZero ? -1500f : 0f;
        itemBoxUI.anchoredPosition = new Vector2(itemBoxUI.anchoredPosition.x, nextY);
        isPositionAtZero = !isPositionAtZero;
    }

    /// <summary>クロスヘアを一度だけ表示する。</summary>
    private void OpenCrossHairOnce()
    {
        if (isOpenCrossHair) return;
        SafeSetActive(crossHair, true);
        isOpenCrossHair = true;
    }

    /// <summary>死亡時のUIを表示し、死因ごとのパネルを切り替える。</summary>
    private void DieReason()
    {
        if (GameManager.instance == null || !GameManager.instance.isPlayerDead) return;

        if (CharacterMove.instance != null)
            CharacterMove.instance.isGameStarted = false;

        SafeSetActive(dieUI, true);

        if (GameManager.instance.isFireDie)
            SafeSetActive(fireDie, true);
        else if (GameManager.instance.isSecondEarthDie)
            SafeSetActive(secondEarthDie, true);
        else if (GameManager.instance.isFirstEarthDie)
            SafeSetActive(firstEarthDie, true);
    }

    /// <summary>ゲーム終了時のUIを表示し、到達エンディングに応じてパネルを出し分ける。</summary>
    private void GameEndUI()
    {
        if (GameManager.instance == null || !GameManager.instance.isGameEnd) return;
        if (endPanel != null && endPanel.activeSelf) return;

        SafeSetActive(endPanel, true);

        if (curor != null)
        {
            var curorPos = curor.GetComponent<RectTransform>();
            if (curorPos != null) curorPos.anchoredPosition = new Vector2(950f, 550f);
        }

        if (CharacterMove.instance != null)
            CharacterMove.instance.isGameStarted = false;

        if (GameManager.instance.isLoopEnd) SafeSetActive(loopEnd, true);
        else if (GameManager.instance.isEnd_1) SafeSetActiveByIndex(endingPanels, 0, true);
        else if (GameManager.instance.isEnd_2) SafeSetActiveByIndex(endingPanels, 1, true);
        else if (GameManager.instance.isEnd_3) SafeSetActiveByIndex(endingPanels, 2, true);
        else if (GameManager.instance.isEnd_4) SafeSetActiveByIndex(endingPanels, 3, true);
        else if (GameManager.instance.isEnd_5) SafeSetActiveByIndex(endingPanels, 4, true);
        else if (GameManager.instance.isEnd_6) SafeSetActiveByIndex(endingPanels, 5, true);
        else if (GameManager.instance.isEnd_7) SafeSetActiveByIndex(endingPanels, 6, true);
    }

    /// <summary>GameManager のメモ状態に応じてメモUIの表示を切り替える。</summary>
    private void Memo()
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.memo == 1)
        {
            SafeSetActive(memos, true); SafeSetActive(memo1, true);
        }
        else if (GameManager.instance.memo == 2)
        {
            SafeSetActive(memos, true); SafeSetActive(memo2, true);
        }
        else if (GameManager.instance.memo == 3)
        {
            SafeSetActive(memos, true); SafeSetActive(memo3, true);
        }
        else if (GameManager.instance.memo == 0)
        {
            SafeSetActive(memo1, false); SafeSetActive(memo2, false);
            SafeSetActive(memo3, false); SafeSetActive(memos, false);
        }
    }

    // ====== セリフ（イベント/通常） ======

    /// <summary>電話／地図の閲覧イベントに応じたメッセージシーケンスを開始する。</summary>
    private void EventTalk()
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.isCallPhone && !isCallEventEnd)
        {
            isCallEventEnd = true;
            ShowMessagesSequentially(callPhoneMessages, callPhoneMessageDelay);
        }
        else if (GameManager.instance.isMapWatch && !isMapEventEnd)
        {
            isMapEventEnd = true;
            ShowMessagesSequentially(mapWatchMessages, mapWatchMessageDelay);
        }
    }

    /// <summary>自己セリフの発話条件を評価し、該当テキストを表示する。</summary>
    private void Talk()
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.selfSpleak_1 && currentTalkCount == 0)
        {
            SetTalkText(selfSpeak1Text);
            currentTalkCount++;
        }
        else if (GameManager.instance.selfSpleak_2 && isMemoTalk)
        {
            SetTalkText(selfSpeak2Text);
            GameManager.instance.selfSpleak_2 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_3 && isMemoTalk)
        {
            SetTalkText(selfSpeak3Text);
            GameManager.instance.selfSpleak_3 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_4 && isMemoTalk)
        {
            SetTalkText(selfSpeak4Text);
            GameManager.instance.selfSpleak_4 = false;
            isMemoTalk = false;
        }
        else if (GameManager.instance.selfSpleak_5 && currentTalkCount == 1)
        {
            SetTalkText(selfSpeak5Text);
            currentTalkCount++;
        }
    }

    // ====== トーク表示ユーティリティ ======

    /// <summary>トークUIにテキストを設定し、一定時間後に自動で閉じる。</summary>
    private void SetTalkText(string text)
    {
        if (textComponent == null || talk == null) return;
        textComponent.text = text;
        talk.SetActive(true);
        isTalk = true;
        StartCoroutine(HideTalkAfterDelay(talkAutoHideSeconds));
    }

    /// <summary>指定秒数の経過後にトークUIを閉じる。</summary>
    private IEnumerator HideTalkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (talk != null) talk.SetActive(false);
        isTalk = false;
        yield break;
    }

    /// <summary>複数メッセージを連続表示する。各要素は指定秒数で切り替える。</summary>
    public void ShowMessagesSequentially(string[] messages, float delay)
    {
        StartCoroutine(ShowMessagesCoroutine(messages, delay));
    }

    /// <summary>連続表示のコルーチン実装。</summary>
    private IEnumerator ShowMessagesCoroutine(string[] messages, float delay)
    {
        if (messages == null) yield break;

        foreach (var message in messages)
        {
            SetTalkText(message);
            yield return new WaitForSeconds(delay);
        }

        if (talk != null) talk.SetActive(false);
        isTalk = false;
    }

    // ====== ヘルプ/入力補助 ======

    /// <summary>操作説明UIの表示継続・自動クローズ・再表示条件を管理する。</summary>
    private void HandleControlUIHelp()
    {
        SafeSetActive(controlUI, true);

        if (GameManager.instance.gameTime >= closeControlUITime)
        {
            if (!isFirstControlEvent)
            {
                SafeSetActive(controlUI, false);
                GameManager.instance.selfSpleak_1 = true;
                isFirstControlEvent = true;
            }
            else
            {
                if (CharacterMove.instance != null && CharacterMove.instance.isDontMove)
                    SafeSetActive(controlUI, true);
                else
                    SafeSetActive(controlUI, false);
            }
        }
    }

    // ====== 共通ユーティリティ ======

    /// <summary>GameObject の null を許容した安全な SetActive。</summary>
    private static void SafeSetActive(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }

    /// <summary>配列インデックスの安全性を担保した SetActive。</summary>
    private static void SafeSetActiveByIndex(GameObject[] list, int index, bool active)
    {
        if (list == null || index < 0 || index >= list.Length) return;
        SafeSetActive(list[index], active);
    }
}
