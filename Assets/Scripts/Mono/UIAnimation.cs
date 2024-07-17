using DG.Tweening;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private enum AnimationType
    {
        SlideUp,
        SlideDown,
        SlideLeft,
        SlideRight,
        ScaleUp
    }

    [SerializeField]
    private AnimationType uiAnimationIn;
    [SerializeField]
    private AnimationType uiAnimationOut;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private Ease ease = Ease.OutElastic;
    [SerializeField]
    private float distance = 100f;    [SerializeField]
    private float delay;

    private Sequence sequenceOut;

    private bool isFirst;
    
    private void Start()
    {
        AnimationIn();
        isFirst = true;
    }

    private void OnEnable()
    {
        if (isFirst) AnimationIn();
    }

    public void AnimationIn()
    {
        //rectTransform.transform.localScale = Vector3.one;
        switch (uiAnimationIn)
        {
            case AnimationType.ScaleUp:
                DOTween.Sequence()
                    .AppendCallback(() => rectTransform.transform.localScale = Vector3.zero)
                    .AppendInterval(delay)
                    .Append(rectTransform.DOScale(1f, fadeTime).SetEase(ease)).SetUpdate(true);
                break;
            case AnimationType.SlideDown:
                DOTween.Sequence()
                    .AppendCallback(() => rectTransform.transform.localPosition += new Vector3(0f,distance,0f))
                    .AppendInterval(delay)
                    .Append(rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime).SetEase(ease)).SetUpdate(true);
                break;
            case AnimationType.SlideUp:
                DOTween.Sequence()
                    .AppendCallback(() => rectTransform.transform.localPosition += new Vector3(0f,-distance,0f))
                    .AppendInterval(delay)
                    .Append(rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime).SetEase(ease)).SetUpdate(true);
                break;
            case AnimationType.SlideLeft:
                DOTween.Sequence()
                    .AppendCallback(() => rectTransform.transform.localPosition += new Vector3(distance, 0f,0f))
                    .AppendInterval(delay)
                    .Append(rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime).SetEase(ease)).SetUpdate(true);
                break;
            case AnimationType.SlideRight:
                DOTween.Sequence()
                    .AppendCallback(() => rectTransform.transform.localPosition += new Vector3(-distance,0f,0f))
                    .AppendInterval(delay)
                    .Append(rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime).SetEase(ease)).SetUpdate(true);
                break;
        }
    }
    
    public void AnimationOut()
    {
        sequenceOut.Kill(true);
        var fadeTimeOut = fadeTime / 2;
        rectTransform.transform.localScale = Vector3.one;
        sequenceOut = DOTween.Sequence();
        switch (uiAnimationOut)
        {
            case AnimationType.ScaleUp:
                rectTransform.DOScale(0f, fadeTime / 2).SetEase(Ease.OutElastic).SetUpdate(true);
                break;
            case AnimationType.SlideDown:
                rectTransform.DOAnchorPos(new Vector2(0f, -100f), fadeTimeOut).SetEase(Ease.OutElastic).SetUpdate(true);
                break;
            case AnimationType.SlideUp:
                rectTransform.DOAnchorPos(new Vector2(0f, 100f), fadeTimeOut).SetEase(Ease.OutElastic).SetUpdate(true);
                break;
            case AnimationType.SlideLeft:
                rectTransform.DOAnchorPos(new Vector2(100f, 0f), fadeTimeOut).SetEase(Ease.OutElastic).SetUpdate(true);
                break;
            case AnimationType.SlideRight:
                rectTransform.DOAnchorPos(new Vector2(-100f, 0f), fadeTimeOut).SetEase(Ease.OutElastic).SetUpdate(true);
                break;
        }
        sequenceOut.AppendInterval(fadeTimeOut / 2);
        sequenceOut.AppendCallback(() => gameObject.SetActive(false));
    }
}

