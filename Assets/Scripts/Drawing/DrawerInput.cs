using System.Collections;
using UnityEngine;

namespace PaperWorks.Drawing
{
    public sealed class DrawerInput : MonoBehaviour
    {
        private const int LeftMouseButtonIndex = 0;

        [SerializeField] private Drawer _drawer;

        private bool _isAlive = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(DrawerInput.LeftMouseButtonIndex))
            {
                HandleMouseButtonDown();
            }

            if (Input.GetMouseButtonUp(DrawerInput.LeftMouseButtonIndex))
            {
                HandleMouseButtonUp();
            }
        }

        private void HandleMouseButtonDown()
        {
            _isAlive = true;
            StartCoroutine(Drawing());
        }

        private void HandleMouseButtonUp()
        {
            if (_isAlive)
            {
                _drawer.EndDrawing();
            }

            _isAlive = false;
        }

        private IEnumerator Drawing()
        {
            while (_isAlive)
            {
                _drawer.AddPoint(Input.mousePosition);

                yield return null;
            }
        }
    }
}
