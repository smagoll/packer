using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickButtonDefault : MonoBehaviour
{
    public readonly UnityEvent endClick = new();
    
    private void Start()
    {
        Share();
    }

    
    private void Share()
    {
        var button = GetComponent<Button>();
        
        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            Object target = button.onClick.GetPersistentTarget(i);
            string methodName = button.onClick.GetPersistentMethodName(i);

            // Находим метод по имени
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Добавляем метод во второй UnityEvent
            if (method != null)
            {
                UnityAction unityAction = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, method);
                endClick.AddListener(unityAction);
            }
        }
        
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(Pressed);
    }
    
    private void Pressed()
    {
        AudioController.instance.PlaySFX(AudioController.instance.button);
        
        DOTween.Sequence()
            .Append(transform.DOScale(.8f, .3f))
            .Append(transform.DOScale(1f, .1f))
            .AppendCallback(() => endClick?.Invoke())
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}