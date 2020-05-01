using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScientistBox : MonoBehaviour
{
    private float Timer_max;
    private float Timer_count;
    private Image scientistIcon; 
    // Start is called before the first frame update
    void Start()
    {
        Timer_max = 1000;
        Timer_count = 0;
        scientistIcon = transform.Find("Icon").GetComponent<Image>();
        ImgInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer_count < Timer_max)
        {
            Timer_count++;
        }
        else
        {
            scientistIcon.DOFade(0, 2);
            ImgInit();
            Timer_count = 0;
        }
    }
    private void ImgInit()
    {
        ScientistManager.GetSciData();
        scientistIcon.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(Random.Range(0, 6)));
        scientistIcon.color = new Color(1, 1, 1, 0);
        scientistIcon.DOFade(1, 2);
    }
}
