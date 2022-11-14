using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private float _minProgress = 0f;
    [SerializeField] private float _maxProgress = 1f;
    public float MaxProgress { get { return _maxProgress; } set { _maxProgress = value; } }
    [Range(0f, float.PositiveInfinity)]
    [SerializeField] private float _currentProgress = 0f;
    public float CurrentProgress { get { return _currentProgress; } set { _currentProgress = value; } }
    public Image mask;

    void Start()
    {
        
    }

    void Update()
    {
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float currentOffset = CurrentProgress - _minProgress;
        float maxOffset = MaxProgress - _minProgress;
        float fillAmount = currentOffset / maxOffset;
        mask.fillAmount = fillAmount;
    }
}
