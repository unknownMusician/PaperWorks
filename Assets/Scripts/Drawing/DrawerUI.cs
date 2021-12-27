#nullable enable

using System;
using PaperWorks.Common;
using PaperWorks.Common.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace PaperWorks.Drawing
{
    public class DrawerUI
    {
        protected readonly Button Button;
        protected readonly DrawerInput DrawerInput;
        protected readonly Vector2Int VisiblePosition;
        protected readonly Vector2Int InvisiblePosition;
        protected readonly float AnimationTime;

        private RectTransform _rectTransform;

        private bool _isVisible = false;

        public DrawerUI(
            Button button, DrawerInput drawerInput, Vector2Int visiblePosition, Vector2Int invisiblePosition,
            float animationTime, RectTransform rectTransform
        )
        {
            Button = button;
            DrawerInput = drawerInput;
            VisiblePosition = visiblePosition;
            InvisiblePosition = invisiblePosition;
            AnimationTime = animationTime;

            _rectTransform = rectTransform;
            DrawerInput.IsActive = false;
        }

        public void ChangeVisibility()
        {
            Button.interactable = false;
            GetStartAndEndPosition(out Vector2Int start, out Vector2Int end);

            Action<float> TConsumer = t => _rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            void EndHandler()
            {
                Button.interactable = true;
                DrawerInput.IsActive = _isVisible;
            }

            Interpolation.Interpolate(
                AnimationTime,
                TConsumer.Normalized(NormalizationFunctions.SmoothStep),
                EndHandler
            );

            _isVisible = !_isVisible;
        }

        protected void GetStartAndEndPosition(out Vector2Int start, out Vector2Int end)
        {
            if (_isVisible)
            {
                start = VisiblePosition;
                end = InvisiblePosition;
            }
            else
            {
                start = InvisiblePosition;
                end = VisiblePosition;
            }
        }
    }
}
