#nullable enable

using System;
using System.Collections.Generic;
using PaperWorks.Common;
using PaperWorks.Common.Animations;
using PaperWorks.Workers;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

namespace PaperWorks.Drawing
{
    public class Drawer : IDisposable
    {
        protected const int MaxPointCount = 256;

        protected static readonly int ComputeBufferId = UnityEngine.Shader.PropertyToID("Points");
        protected static readonly int TextureId = UnityEngine.Shader.PropertyToID("Result");

        protected readonly Transform Transform;
        protected readonly ComputeShader Shader;
        protected readonly RenderTexture Texture;
        protected readonly WorkerHolderUI WorkerHolderUI;
        protected readonly WorkerHolder WorkerHolder;
        protected readonly GameObject HumanPrefab;
        protected readonly Transform ForeGround;
        protected readonly float HumanMoveTime;
        protected readonly List<Vector2> Points = new List<Vector2>();

        protected readonly int MainShaderKernel;
        protected readonly ComputeBuffer ComputeBuffer;


        public Drawer(
            Transform transform, ComputeShader shader, RenderTexture texture, WorkerHolderUI workerHolderUI,
            WorkerHolder workerHolder, GameObject humanPrefab, Transform foreGround, float humanMoveTime
        )
        {
            Transform = transform;
            Shader = shader;
            Texture = texture;
            WorkerHolderUI = workerHolderUI;
            WorkerHolder = workerHolder;
            HumanPrefab = humanPrefab;
            ForeGround = foreGround;
            HumanMoveTime = humanMoveTime;

            MainShaderKernel = Shader.FindKernel("Draw");

            ComputeBuffer = new ComputeBuffer(Drawer.MaxPointCount, sizeof(float) * 4);
            Shader.SetBuffer(MainShaderKernel, Drawer.ComputeBufferId, ComputeBuffer);

            Texture = new RenderTexture(16, 16, 1, GraphicsFormat.R32G32B32A32_SFloat)
            {
                enableRandomWrite = true
            };

            Shader.SetTexture(MainShaderKernel, Drawer.TextureId, Texture);
        }

        public void AddPoint(Vector2 point)
        {
            Points.Add(point);
        }

        public void EndDrawing()
        {
            //Debug.Log("End");

            // todo
            //Draw();
            CreateHuman();
            //GetComponent<RawImage>().texture = _texture;

            Points.Clear();
        }

        protected void CreateHuman()
        {
            GameObject human = Object.Instantiate(HumanPrefab, ForeGround, true);

            Action<float> tConsumer = t => TConsumers.MovePosition(
                human.transform,
                Transform.position,
                WorkerHolderUI.NewElementPosition,
                t
            );

            Interpolation.Interpolate(
                HumanMoveTime,
                tConsumer.Normalized(NormalizationFunctions.SmoothStep),
                () => WorkerHolder.Enqueue(human.transform)
            );
        }

        protected void Draw()
        {
            ComputeBuffer.SetData(Points);
            Shader.Dispatch(MainShaderKernel, 1, 1, 1);
        }

        public void Dispose() => ComputeBuffer.Dispose();
    }
}
