using UnityEngine;

namespace Assets.Scripts
{
    public class Habitat : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _size;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}
