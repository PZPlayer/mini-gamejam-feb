using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class TrialTimer : MonoBehaviour
{
    [SerializeField] private float _duration = 30f;
    [SerializeField] private TMP_Text _timerText;

    [Header("Events")]
    public UnityEvent OnTimerEnd;

    private float _timeLeft;
    private bool _isRunning;
    private float _timeRight;

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        _timeLeft = _duration;
        _isRunning = true;

        StopAllCoroutines();
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isRunning && _timeLeft > 0f)
        {
            UpdateUI();
            _timeLeft -= Time.deltaTime;
            yield return null;
        }

        _timeLeft = Mathf.Max(_timeLeft, 0f);
        UpdateUI();

        _isRunning = false;
        OnTimerEnd?.Invoke();
    }

    private void UpdateUI()
    {
        if (_timerText == null) return;

        int minutes = Mathf.FloorToInt(_timeLeft / 60f);
        int seconds = Mathf.FloorToInt(_timeLeft % 60f);

        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        _isRunning = false;
    }
}
