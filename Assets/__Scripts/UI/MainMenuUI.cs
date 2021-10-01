using DG.Tweening;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    private readonly Vector2 _showedPosition = new Vector2(-55, 0);
    private readonly Vector2 _hidePosition = new Vector2(600, 0);
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Button action.
    /// </summary>
    public void StartGame()
    {
        HideMenu();
    }

    /// <summary>
    /// Button action.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
        _rectTransform.DOAnchorPos(_showedPosition, 1f);
    }
    
    public void HideMenu()
    {
        _rectTransform.DOAnchorPos(_hidePosition, 1f)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                GameManager.S.Invoke_OnGameStart();
            });
    }
}