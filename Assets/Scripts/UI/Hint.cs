using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    private Text hintText;
    public float hintShowTime = 3f;
    private Color color;
    private void Awake()
    {
        EventCenter.AddListener<string>(EventDefine.Hint, Show);
        hintText = GetComponent<Text>();
        color = new Color(1, 1, 1, 0);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.Hint, Show);
    }
    private void Show(string text)
    {
        hintText.text = text;
        hintText.DOColor(new Color(color.r, color.g, color.b, 1), 0.3f).OnComplete(() =>
        {
            StartCoroutine(Delay());
        });
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(hintShowTime);
        hintText.DOColor(new Color(color.r, color.g, color.b, 0), 0.3f);
    }
}
