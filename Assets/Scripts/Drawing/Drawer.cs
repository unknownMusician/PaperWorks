using System;
using System.Collections;
using System.Collections.Generic;
using PaperWorks.Common;
using PaperWorks.Common.Animations;
using PaperWorks.Workers;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PaperWorks.Drawing
{
    public sealed class Drawer : MonoBehaviour
    {
        private const int MaxPointCount = 256;

        private static readonly int ComputeBufferId = Shader.PropertyToID("Points");
        private static readonly int TextureId = Shader.PropertyToID("Result");

        [SerializeField] private ComputeShader _shader;
        [SerializeField] private RenderTexture _texture;
        [SerializeField] private WorkerHolderUI _workerHolderUI;
        [SerializeField] private WorkerHolder _workerHolder;
        [SerializeField] private GameObject _humanPrefab;
        [SerializeField] private Transform _foreGround;
        [SerializeField] private float _humanMoveTime;

        private readonly List<Vector2> _points = new List<Vector2>();
        private ComputeBuffer _computeBuffer;
        private int _mainShaderKernel;

        private void OnValidate()
            => this.AssertNotNull(_shader, _texture, _workerHolderUI, _workerHolder, _humanPrefab, _foreGround);

        private void Awake()
        {
            _mainShaderKernel = _shader.FindKernel("Draw");

            _computeBuffer = new ComputeBuffer(Drawer.MaxPointCount, sizeof(float) * 4);
            _shader.SetBuffer(_mainShaderKernel, Drawer.ComputeBufferId, _computeBuffer);

            _texture = new RenderTexture(16, 16, 1, GraphicsFormat.R32G32B32A32_SFloat)
            {
                enableRandomWrite = true
            };

            _shader.SetTexture(_mainShaderKernel, Drawer.TextureId, _texture);
        }

        public void AddPoint(Vector2 point)
        {
            _points.Add(point);

            Debug.Log(_points.Count);

            // if (_points.Count >= MaxPointCount)
            // {
            //     EndDrawing();
            // }
        }

        public void EndDrawing()
        {
            Debug.Log("End");

            // todo
            //Draw();
            CreateHuman();
            //GetComponent<RawImage>().texture = _texture;

            _points.Clear();
        }

        // todo
        // private IEnumerator Start()
        // {
        //     while (true)
        //     {
        //         CreateHuman();
        //
        //         yield return new WaitForSeconds(1.0f);
        //     }
        // }

        private void CreateHuman()
        {
            GameObject human = Object.Instantiate(_humanPrefab, _foreGround, true);

            Action<float> tConsumer = t => TConsumers.MovePosition(human.transform,
                                                                   transform.position,
                                                                   _workerHolderUI.NewElementPosition,
                                                                   t);

            StartCoroutine(Interpolation.Interpolate(_humanMoveTime,
                                                     tConsumer.Normalized(NormalizationFunctions.SmoothStep),
                                                     () => _workerHolder.Enqueue(human.transform)));
        }

        private void Draw()
        {
            _computeBuffer.SetData(_points);
            _shader.Dispatch(_mainShaderKernel, 1, 1, 1);
        }
    }
}
