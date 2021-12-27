#nullable enable

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PaperWorks.Drawing
{
    public class DrawerInput : IPointerDownHandler, IDisposable
    {
        protected const int LeftMouseButtonIndex = 0;
        
        protected readonly Drawer Drawer;
        protected readonly CancellationTokenSource AliveCancellation;
        
        private bool _isAlive = false;
        public bool IsActive = false;

        public DrawerInput(Drawer drawer)
        {
            Drawer = drawer;
            AliveCancellation = new CancellationTokenSource();

            ListenInputAsync();
        }
        
        protected async Task ListenInputAsync()
        {
            while (!AliveCancellation.IsCancellationRequested)
            {
                if (Input.GetMouseButtonUp(DrawerInput.LeftMouseButtonIndex))
                {
                    HandleMouseButtonUp();
                }

                await Task.Yield();
            }
        }

        protected void HandleMouseButtonDown()
        {
            if (!IsActive)
            {
                return;
            }
            
            _isAlive = true;
            
            Drawing();
        }

        protected void HandleMouseButtonUp()
        {
            if (_isAlive)
            {
                Drawer.EndDrawing();
            }

            _isAlive = false;
        }

        protected async Task Drawing()
        {
            while (_isAlive)
            {
                Drawer.AddPoint(Input.mousePosition);

                await Task.Yield();
            }
        }

        public void OnPointerDown(PointerEventData eventData) => HandleMouseButtonDown();
        
        public void Dispose()
        {
            _isAlive = false;
            AliveCancellation.Cancel();
        }
    }
}
