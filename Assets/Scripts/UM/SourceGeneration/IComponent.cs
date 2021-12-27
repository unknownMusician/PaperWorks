using UnityEngine;

namespace UM.SourceGeneration
{
    public interface IComponent<out T>
    {
        T HeldType { get; }
    }

    public interface ITriggerEnterHandler
    {
        void OnTriggerEnter(Collider other);
    }
}

//        - [GenerateMonoBehaviour] Mover                  !Mover.IsInterface && !Mover.IsAbstract && !Mover : MonoBehaviour && Mover.ConstructorCount <= 1
//
// 0 0 00 - FromThis Inject isNotComponentItself(UseAbstractComponent)
// 
// 0 0 0  - IMover                                         !IMover.IsInterface && IMover : Component
// 0 0 11 - IComponent<IMover>.HeldType                    !true && !IMover : Component
// 0 0 10 - IMoverComponent.HeldType                       !IMover.IsInterface && [Generate] IMover
//
// 0 1 0  - Mover                                          !Mover.IsInterface && Mover : Component
// 0 1 11 - IComponent<Mover>.HeldType                     !true && !IMover : Component
// 0 1 10 - MoverComponent.HeldType                        !MoverComponent.IsInterface (&& MoverComponent : Component)
//
// 1 0 0  - GetComponent<IMover>()                         IMover : Component
// 1 0 11 - GetComponent<IComponent<IMover>>.HeldType      !IMover : Component
// 1 0 10 - GetComponent<IMoverComponent>.HeldType         [Generate] IMover
// 
// 1 1 0  - GetComponent<Mover>()                          Mover : Component
// 1 1 11 - GetComponent<IComponent<Mover>>.HeldType       !IMover : Component
// 1 1 10 - GetComponent<MoverComponent>.HeldType          (MoverComponent : Component)

