using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HabitatResident : MonoBehaviour
    {
        [SerializeField]
        private int _type;

        public abstract int Type { get; protected set; }
        public abstract float Authority { get; protected set; }

        protected virtual void Start()
        {
            Type = _type;
            Authority = GetVolume();
        }

        public bool TrySetType(int type, float authority)
        {
            if (authority <= Authority) return false;
            Authority = authority;
            Type = type;
            return true;
        }

        private float GetVolume()
        {
            return 1f / 3f * Mathf.PI * Mathf.Sqrt(transform.localScale.x / 2f) * transform.localScale.z;
        }
    }
}
