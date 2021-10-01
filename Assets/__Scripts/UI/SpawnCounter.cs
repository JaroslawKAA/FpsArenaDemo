using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int numberToCount = 3;
    [SerializeField] private float changingTime = 1f;
    [SerializeField] private string counterMessage = "Spawning!";

    private int currentNumber;
    private float timer;

    private void Start()
    {
        Assert.IsNotNull(_text);
    }

    private void OnEnable()
    {
        currentNumber = numberToCount;
        _text.text = currentNumber.ToString();
        _text.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        _text.rectTransform.DOScale(1f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changingTime)
        {
            _text.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            _text.rectTransform.DOScale(1f, .5f);
            currentNumber--;
            if (currentNumber == 0)
            {
                _text.text = counterMessage;
                OnCountingFinish?.Invoke();
            }
            else if (currentNumber < 0)
            {
                gameObject.SetActive(false);
            }
            else
                _text.text = currentNumber.ToString();

            timer = 0f;
        }
    }

    public event Action OnCountingFinish;
}