using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    public GameObject talkObject;
    public Text textTalk;
    // Start is called before the first frame update
    void Start()
    {
        textTalk = talkObject.GetComponent<Text>(); // Textコンポーネントを取得
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance._1Talk == true)
        {
            UpdateText("今日はパパもママもいないからゲームやりほうだいだ！はやくリビングでゲームをやろう！なんだろう、メモがおいてある");
        }

        if(GameManager.instance._2Talk == true)
        {
            UpdateText("さいあくだ…せっかくの休みなのに！… 何としてもゲームを見つけなきゃ！");
        }

        if (GameManager.instance._3Talk == true)
        {
            UpdateText("スマホを見つけた！ママは用心ぶかいからゲームはべつの場所にかくしてるみたい");
        }

        if (GameManager.instance._4Talk == true)
        {
            UpdateText("ゲームは見つけたけど…ママが帰ってきたら怒るかな…");
        }

        if (GameManager.instance._5Talk == true)
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
