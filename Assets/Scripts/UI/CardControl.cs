using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardState
{
    Front,
    Back
}
public class CardControl : MonoBehaviour
{
    public GameObject mFront;
    public GameObject mBack;
    public CardState mCardState = CardState.Back;
    public float mTime = 0.3f;
    private static bool isActive = false;

    private void Awake()
    {
        if (mCardState == CardState.Front)
        {
            //如果是从正面开始，则将背面旋转90度，这样就看不见背面了
            mFront.transform.eulerAngles = Vector3.zero;
            mBack.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            //从背面开始，同理
            mFront.transform.eulerAngles = new Vector3(0, 90, 0);
            mBack.transform.eulerAngles = Vector3.zero;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartFrontAll() {
        StartFront();
    }
    private void StartFront2(GameObject go)
    {
        if (go == gameObject)
        {
            StartFront();
        } 
    }
    private void StartBack2(GameObject go)
    {
        if (go == gameObject)
        {
            StartBack();
        }
    }
    /// <summary>
    /// 留给外界调用的接口
    /// </summary>
    public void StartBack()
    {
        if (isActive)
            return;
        StartCoroutine(ToBack());
        

    }
    /// <summary>
    /// 留给外界调用的接口
    /// </summary>
    public void StartFront()
    {
        if (isActive)
            return;
        StartCoroutine(ToFront());
    }
    /// <summary>
    /// 翻转到背面
    /// </summary>
	IEnumerator ToBack()
    {
        isActive = true;
        mFront.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mBack.transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;

    }
    /// <summary>
    /// 翻转到正面
    /// </summary>
    IEnumerator ToFront()
    {
        isActive = true;
        mBack.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mFront.transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;
    }

}
