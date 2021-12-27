#nullable enable

using UnityEngine;

namespace PaperWorks.Common.Animations
{
    public static class TConsumers
    {
        public static void MovePosition(Transform transform, in Vector3 startPosition, in Vector3 endPosition, float t)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        
        public static void MovePosition(Transform transform, in Vector2 startPosition, in Vector2 endPosition, float t)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, t);
        }
        
        public static void MoveLocalPosition(Transform transform, in Vector3 startPosition, in Vector3 endPosition, float t)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
        }
        
        public static void MoveLocalPosition(Transform transform, in Vector2 startPosition, in Vector2 endPosition, float t)
        {
            transform.localPosition = Vector2.Lerp(startPosition, endPosition, t);
        }

        public static void Move(out Vector2 position, in Vector2 startPosition, in Vector2 endPosition, float t)
        {
            position = Vector2.Lerp(startPosition, endPosition, t);
        }
    }
}
