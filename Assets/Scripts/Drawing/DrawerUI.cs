using System;
using PaperWorks.Common.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace PaperWorks.Drawing
{
    public sealed class DrawerUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Vector2Int _visiblePosition;
        [SerializeField] private Vector2Int _invisiblePosition;
        [SerializeField] private float _animationTime;

        private RectTransform rectTransform;
        
        private bool _isVisible = false;

        private void Awake() => rectTransform = GetComponent<RectTransform>();

        public void ChangeVisibility()
        {
            _button.interactable = false;
            GetStartAndEndPosition(out Vector2Int start, out Vector2Int end);
            
            void TConsumer(float t)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            }

            void EndHandler() => _button.interactable = true;

            StartCoroutine(Interpolation.Interpolate(_animationTime,
                                                     TConsumer,
                                                     NormalizationFunctions.SmoothStep,
                                                     EndHandler));

            _isVisible = !_isVisible;
        }

        public void GetStartAndEndPosition(out Vector2Int start, out Vector2Int end)
        {
            if (_isVisible)
            {
                start = _visiblePosition;
                end = _invisiblePosition;
            }
            else
            {
                start = _invisiblePosition;
                end = _visiblePosition;
            }
        }
    }
}
