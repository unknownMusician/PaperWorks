using PaperWorks.Drawing;
using UM.ComponentGeneration;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components.PaperWorks.Drawing
{
    public sealed class DrawerInputComponent : GeneratedComponent<DrawerInput>, IPointerDownHandler
    {
        [SerializeField] private DrawerComponent _drawer;

        protected override DrawerInput Create() => new DrawerInput(_drawer.HeldItem);
        public void OnPointerDown(PointerEventData eventData) => HeldItem.OnPointerDown(eventData);
    }
}
