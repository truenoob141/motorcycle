using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace Motorcycle
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private SpriteShapeController shape;
        [SerializeField] private VehicleBehaviour vehicle;

        private const float safeArea = 3;
        private const float maxHeight = 4;
        private const float size = 1000;
        private readonly Vector2 distanceBtwPoints = new Vector2(3, 7);

        private void Start()
        {
            CreateMap();
        }

        private void OnEnable()
        {
            vehicle.onRestart += OnRestart;
            CreateMap();
        }

        private void OnDisable()
        {
            vehicle.onRestart -= OnRestart;
        }

        private void OnRestart()
        {
            CreateMap();
        }

        private void CreateMap()
        {
            var spline = shape.spline;
            spline.Clear();

            int count = 0;
            // Begin
            spline.InsertPointAt(count++, new Vector3(-5, -1));
            spline.InsertPointAt(count++, new Vector3(-5, 50));
            spline.InsertPointAt(count++, new Vector3(-4, 0));
            spline.InsertPointAt(count++, new Vector3(safeArea, 0));
            
            // Middle
            float previousHeight = 0;
            float distance = safeArea + distanceBtwPoints.y;
            while (distance < size)
            {
                float height = Random.Range(0, maxHeight);
                if (Mathf.Abs(height - previousHeight) > 1)
                    height = previousHeight + (previousHeight > height ? -1 : 1);
                
                var pos = new Vector3(distance, height);
                spline.InsertPointAt(count, pos);
                spline.SetTangentMode(count, ShapeTangentMode.Continuous);
                spline.SetLeftTangent(count, new Vector3(-2, 0, 0));
                spline.SetRightTangent(count, new Vector3(2, 0, 0));

                previousHeight = pos.y;
                distance += Random.Range(distanceBtwPoints.x, distanceBtwPoints.y);
                ++count;
            }

            // End
            spline.InsertPointAt(count++, new Vector3(size - 1, 0));
            spline.InsertPointAt(count++, new Vector3(size, 50));
            spline.InsertPointAt(count, new Vector3(size, -1));
        }
    }
}