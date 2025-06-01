using UnityEngine;
using UnityEngine.UI;

public class SelfSpeak : MonoBehaviour
{
    public GameObject talkObject;
    public Text textTalk;

    void Start()
    {
        textTalk = talkObject.GetComponent<Text>(); // Textコンポーネントを取得
    }

    void Update()
    {
        if(GameManager.instance.selfSpleak_1 == true)
        {
            UpdateText("今日はパパもママもいないからゲームやりほうだいだ！はやくリビングでゲームをやろう！なんだろう、メモがおいてある");
        }

        if(GameManager.instance.selfSpleak_2 == true)
        {
            UpdateText("さいあくだ…せっかくの休みなのに！… 何としてもゲームを見つけなきゃ！");
        }

        if (GameManager.instance.selfSpleak_3 == true)
        {
            UpdateText("スマホを見つけた！ママは用心ぶかいからゲームはべつの場所にかくしてるみたい");
        }

        if (GameManager.instance.selfSpleak_4 == true)
        {
            UpdateText("ゲームは見つけたけど…ママが帰ってきたら怒るかな…");
        }

        if (GameManager.instance.selfSpleak_5 == true)
        {
            UpdateText("びっくりした…とりあえずじしんはおさまったみたい");
        }
    }

    public void UpdateText(string newText)
    {

        if (textTalk != null)
        {
            textTalk.text = newText;
        }
    }
}
