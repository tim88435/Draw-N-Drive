using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private float _minProgress = 0f;
    [SerializeField] private float _maxProgress = 1f;
    [Range(0f, float.PositiveInfinity)]
    [SerializeField] private float _currentProgress = 0f;
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
        float currentOffset = _currentProgress - _minProgress;
        float maxOffset = _maxProgress - _minProgress;
        float fillAmount = currentOffset / maxOffset;
        mask.fillAmount = fillAmount;
    }
}
