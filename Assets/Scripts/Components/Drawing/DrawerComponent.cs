using PaperWorks.Components.Workers;
using PaperWorks.Drawing;
using UM.ComponentGeneration;
using UM.Assertions;
using UnityEngine;

namespace Components.PaperWorks.Drawing
{
    public class DrawerComponent : GeneratedComponent<Drawer>
    {
        [SerializeField] private ComputeShader _shader;
        [SerializeField] private RenderTexture _texture;
        [SerializeField] private WorkerHolderUIComponent _workerHolderUI;
        [SerializeField] private WorkerHolderComponent _workerHolder;
        [SerializeField] private GameObject _humanPrefab;
        [SerializeField] private Transform _foreGround;
        [SerializeField] private float _humanMoveTime;
        
        protected override Drawer Create()
        {
            this.AssertNotNull(_shader, _texture, _workerHolderUI, _workerHolder, _humanPrefab, _foreGround);
            
            return new Drawer(transform, _shader, _texture, _workerHolderUI.HeldItem, _workerHolder.HeldItem, _humanPrefab, _foreGround, _humanMoveTime);
        }
    }
}
