using UnityEngine;

public class CanvasGroupManager : MonoBehaviour
{
    public void ShowPanel(CanvasGroup _cg)
    {
        _cg.alpha = 1;
        _cg.interactable = true;
        _cg.blocksRaycasts = true;
    }

    public void HidePanel(CanvasGroup _cg)
    {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }
}