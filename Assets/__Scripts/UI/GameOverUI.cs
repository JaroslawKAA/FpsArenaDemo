using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private float showingTime = 3;

    private float _timer;

    private void OnEnable()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > showingTime)
        {
            OnGameOverUIDissappear?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public event Action OnGameOverUIDissappear;
}
