using UnityEngine;

namespace Assets.Scripts
{
    public class Habitat : MonoBehaviour
    {
        public Vector3 Size { get; private set; }

        private float _extentX;
        private float _extentY;
        private float _extentZ;

        public void Initialize(Vector3 size)
        {
            Size = size;
            _extentX = size.x / 2f;
            _extentY = size.y / 2f;
            _extentZ = size.z / 2f;
        }

        public Vector3 GetRandomPosition()
        {
            var x = Random.Range(-_extentX, _extentX);
            var y = Random.Range(-_extentY, _extentY);
            var z = Random.Range(-_extentZ, _extentZ);

            return new Vector3(x, y, z);
        }

        public float GetArea()
        {
            return Size.x * Size.y * Size.z;
        }

        public Vector3 GetExtents()
        {
            return new Vector3(_extentX, _extentY, _extentZ);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Size);
        }
    }
}
