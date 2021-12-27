using PaperWorks.Common;
using PaperWorks.Drawing;
using UM.Assertions;
using UM.ComponentGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace Components.PaperWorks.Drawing
{
    public sealed class DrawerUIComponent : GeneratedComponent<DrawerUI>
    {
        [SerializeField] private Button _button;
        [SerializeField] private DrawerInputComponent _drawerInput;
        [SerializeField] private Vector2Int _visiblePosition;
        [SerializeField] private Vector2Int _invisiblePosition;
        [SerializeField] private float _animationTime;

        protected override DrawerUI Create()
        {
            this.AssertNotNull(_button, _drawerInput);

            return new DrawerUI(
                _button,
                _drawerInput.HeldItem,
                _visiblePosition,
                _invisiblePosition,
                _animationTime,
                GetComponent<RectTransform>()
            );
        }

        public void ChangeVisibility() => HeldItem.ChangeVisibility();
    }
}
