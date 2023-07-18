using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ProjectUtils.Helpers
{
    public static class Helpers 
    {
        private static Camera _camera;
        public static Camera Camera
        {
            get
            {
                if(_camera == null) _camera = Camera.main;
                return _camera;
            }
        }
    
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;

        public static bool PointerIsOverUi()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }

        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>Returns the angle in degrees between this position and the mouse position</summary>
        public static float GetAngleToPointer(this Vector3 position)
        {
            Vector3 attackDirection = Input.mousePosition;
            attackDirection = Camera.ScreenToWorldPoint(attackDirection);
            attackDirection.z = position.z;
            attackDirection = (attackDirection-position).normalized;

            float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            while (angle<0) angle += 360;

            return angle;
        }
        /// <summary><para>Returns a vector with the 4 corners of the screen</para>
        /// <param name="targetObjectScale"> The scale of the object we want to be at the point</param>
        /// <param name="borderModification"> Multiplier for the borders to be bigger or smaller</param>
        /// <param name="targetObjectDistance"> (ONLY USE IN PERSPECTIVE MODE) The distance of the object we want to be at the point</param>
        /// </summary>
        public static Vector3[] GetBounds(this Camera cam, float targetObjectScale = 1, float borderModification = 1, float targetObjectDistance = 0)
        {
            Vector3 dist = Camera.WorldToScreenPoint(Camera.transform.forward * (Camera.nearClipPlane + targetObjectScale + targetObjectDistance));
            dist = Camera.ScreenToWorldPoint(dist);
        
            if (Camera.orthographic) 
            {
                var rightTopBounds = Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)) + dist;
                rightTopBounds *= borderModification;
                var leftTopBounds = Camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)) + dist;
                leftTopBounds *= borderModification;
                var rightBotBounds = Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)) + dist;
                rightBotBounds *= borderModification;
                var leftBotBounds = Camera.ScreenToWorldPoint(new Vector3(0, 0, 0)) + dist;
                leftBotBounds *= borderModification;
                return new Vector3[] { leftBotBounds, rightBotBounds, rightTopBounds, leftTopBounds };
            }
        
            Vector3[] res = new Vector3[4];
            Ray ray = Camera.ScreenPointToRay(new Vector3(0, 0, 0)+new Vector3(-Screen.width, -Screen.height, 0)* (borderModification-1));
            Plane plane = new Plane(Camera.transform.forward, dist);
            plane.Raycast(ray, out var distance);
            res[0] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(Screen.width, 0, 0)+new Vector3(Screen.width, -Screen.height, 0)* (borderModification-1));
            plane.Raycast(ray, out distance);
            res[1] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0)* borderModification);
            plane.Raycast(ray, out distance);
            res[2] = ray.GetPoint(distance);
                    
            ray = Camera.ScreenPointToRay(new Vector3(0, Screen.height, 0)+new Vector3(-Screen.width, Screen.height, 0)* (borderModification-1));
            plane.Raycast(ray, out distance);
            res[3] = ray.GetPoint(distance);
            return res;
        }
        
        private struct MyVector
        {
            public Vector3 startPoint;
            public Vector3 direction;
            public float distance;
        }
        private static readonly MyVector[] _vectors = new MyVector[4];
        /// <summary><para>Returns a random point in the bounds of the screen</para>
        /// <param name="targetObjectScale"> The scale of the object we want to be at the point</param>
        /// <param name="borderModification"> Multiplier for the borders to be bigger or smaller</param>
        /// <param name="distanceToTarget"> (ONLY USE IN PERSPECTIVE MODE) The distance of the object we want to be at the point</param>
        /// </summary>
        public static Vector3 GetRandomPointInBounds(this Camera cam, float targetObjectScale = 1, float borderModification = 1, float distanceToTarget = 0)
        {
            Vector3[] bounds = cam.GetBounds(targetObjectScale, borderModification, distanceToTarget);
            for (var i = 0; i < bounds.Length; i++)
            {
                _vectors[i] = new MyVector
                {
                    startPoint = bounds[i],
                    direction = (bounds[(i+1)%bounds.Length] - bounds[i]).normalized,
                    distance = Vector3.Distance(bounds[i], bounds[(i+1)%bounds.Length])
                };
            }
            MyVector vector = _vectors[Random.Range(0, 4)];
            return vector.startPoint + vector.direction * (vector.distance * Random.value);
        }
    }
}
