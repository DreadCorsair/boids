using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Leader : HabitatResident, IBehavioralObject
    {
        [SerializeField]
        private float _authorityRadius = 10;
        [SerializeField]
        private float _moveSpeedFactor;
        [SerializeField]
        private float _rotateSpeedFactor;
        [SerializeField]
        private float _avoidingCollisionSpeedFactor = 30;
        [SerializeField]
        private float _changeDirectionTime;
        [SerializeField]
        private Vector3 _delta = new Vector3(5, 5, 5);

        private Quaternion _direction;

        private bool _avoidingCollision;


        public override float Authority { get; protected set; }
        public override int Type { get; protected set; }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(ChangeDirection());
            StartCoroutine(Scan());
        }

        private void Update()
        {
            Vector3 position = transform.position + transform.forward * _moveSpeedFactor * Time.deltaTime;
            if (position.x < -9 || position.x > 9 ||
                position.y < -9 || position.y > 9 ||
                position.z < -9 || position.z > 9)
            {
                _avoidingCollision = true;
            }
            else
            {
                _avoidingCollision = false;
            }

            transform.position = position;

            var direction = IsCloseToBorder(transform.position) ? Quaternion.LookRotation(-transform.position) : _direction;
            var speed = IsCloseToBorder(transform.position) ? _avoidingCollisionSpeedFactor : _rotateSpeedFactor;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, speed * Time.deltaTime);
        }

        private bool IsCloseToBorder(Vector3 position)
        {
            return
                position.x > 10 || position.x < -10 ||
                position.y > 10 || position.y < -10 ||
                position.z > 10 || position.z < -10;
        }

        private IEnumerator ChangeDirection()
        {
            for (;;)
            {
                var euler = transform.rotation.eulerAngles;
                euler.x += Random.Range(-_delta.x, _delta.x);
                euler.y += Random.Range(-_delta.y, _delta.y);
                euler.z += Random.Range(-_delta.z, _delta.z);
                _direction = Quaternion.Euler(euler);

                yield return new WaitForSeconds(_changeDirectionTime);
            }
        }

        private IEnumerator Scan()
        {
            for (;;)
            {
                var boids = Physics.OverlapSphere(transform.position, _authorityRadius);
                foreach (var boid in boids)
                {
                    var behaviorComponent = boid.GetComponent<HabitatResident>();
                    if (behaviorComponent)
                    {
                        behaviorComponent.TrySetType(Type, Authority);
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        #region Debug

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _authorityRadius);
        }

        #endregion

        public void UpdateBehavior()
        {
            var boids = Physics.OverlapSphere(transform.position, _authorityRadius);
            foreach (var boid in boids)
            {
                var behaviorComponent = boid.GetComponent<HabitatResident>();
                if (behaviorComponent)
                {
                    behaviorComponent.TrySetType(Type, Authority);
                }
            }
        }
    }
}
