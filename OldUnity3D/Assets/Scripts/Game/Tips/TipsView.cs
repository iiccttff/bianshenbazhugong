using UnityEngine;
using System.Collections;

public class TipsView : MonoBehaviour
{
    public void StartTips(string content)
    {
        TweenPosition tp = transform.GetComponent<TweenPosition>();
        TweenAlpha ta = transform.GetComponent<TweenAlpha>();
        UILabel label = transform.Find("Label").GetComponent<UILabel>();
        UISprite sprite = transform.Find("TipsSprite").GetComponent<UISprite>();
        label.text = content;
        sprite.width = label.width + 40;
        ta.onFinished.Add(new EventDelegate(ReturnPlayStop));
        tp.Play(true);
        ta.Play(true);
    }

    void ReturnPlayStop()
    {
        Destroy(gameObject);
    }
}
