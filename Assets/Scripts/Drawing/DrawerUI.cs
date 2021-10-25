using System;
using PaperWorks.Common;
using PaperWorks.Common.Animations;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

namespace PaperWorks.Drawing
{
    public sealed class DrawerUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private DrawerInput _drawerInput;
        [SerializeField] private Vector2Int _visiblePosition;
        [SerializeField] private Vector2Int _invisiblePosition;
        [SerializeField] private float _animationTime;

        private RectTransform _rectTransform;

        private bool _isVisible = false;

        private void OnValidate() => this.AssertNotNull(_button, _drawerInput);

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void Start() => _drawerInput.enabled = false;

        public void ChangeVisibility()
        {
            _button.interactable = false;
            GetStartAndEndPosition(out Vector2Int start, out Vector2Int end);

            Action<float> TConsumer = t => _rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            void EndHandler()
            {
                _button.interactable = true;
                _drawerInput.enabled = _isVisible;
            }

            StartCoroutine(Interpolation.Interpolate(_animationTime,
                                                     TConsumer.Normalized(NormalizationFunctions.SmoothStep),
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
