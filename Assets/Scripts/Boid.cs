using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Boid : HabitatResident, IBehavioralObject
    {
        [SerializeField]
        private float _calculateStepTime = 0.2f;
        [SerializeField]
        private float _cohesionRadius = 10f;
        [SerializeField]
        private float _separationRadius = 5f;
        [SerializeField]
        private float _separationCoef = 1;
        [SerializeField]
        private float _maxSpeed = 1;
        [SerializeField]
        private float _rotationSpeedFactor;

        private Vector3 _velocity;
        private Collider _thisBoid;
        private Rigidbody _rigidbody;

        private Vector3 _cohesion;
        private Vector3 _separation;
        private Vector3 _alignment;

        public override float Authority { get; protected set; }
        public override int Type { get; protected set; }

        [SerializeField] private int _t;

        protected override void Start()
        {
            base.Start();

            _thisBoid = gameObject.GetComponent<Collider>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();

            StartCoroutine(CalculateVelocity());
        }

        private void Update()
        {
            var position = _rigidbody.position + _velocity * Time.deltaTime;
            _rigidbody.MovePosition(position);

            var direction = Quaternion.LookRotation(_velocity);
            var rotation = Quaternion.RotateTowards(_rigidbody.rotation, direction, _rotationSpeedFactor * Time.deltaTime);
            _rigidbody.MoveRotation(rotation);

            _t = Type;
        }

        private IEnumerator CalculateVelocity()
        {
            for (;;)
            {
                _velocity = Vector3.zero;
                var cohesion = Vector3.zero;
                var separation = Vector3.zero;
                var alignment = Vector3.zero;

                var cohesionCount = 0;
                var separationCount = 0;
                var position = _rigidbody.position;

                var boids = Physics.OverlapSphere(transform.position, _cohesionRadius);
                foreach (var boid in boids)
                {
                    if (boid.GetComponent<HabitatResident>().Type != Type) continue;

                    cohesion += boid.transform.position;
                    cohesionCount++;

                    var direction = position - boid.transform.position;
                    var distance = direction.magnitude;
                    if (boid.Equals(_thisBoid) || distance > _separationRadius) continue;
                    separation += direction / distance;
                    separationCount++;

                    var behaviorComponent = boid.GetComponent<Boid>();
                    if (behaviorComponent)
                    {
                        alignment += behaviorComponent._velocity;
                    }
                }

                if (cohesionCount > 0)
                    cohesion = cohesion / cohesionCount - position;
                if (separationCount > 0)
                {
                    separation = (separation / separationCount) * _separationCoef;
                    alignment /= separationCount;
                }
                    
                _velocity = Vector3.ClampMagnitude(cohesion + separation + alignment, _maxSpeed);

                _cohesion = cohesion;
                _separation = separation;
                _alignment = alignment;

                yield return new WaitForSeconds(_calculateStepTime);
            }
        }

        #region Debug

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _cohesionRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _separationRadius);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, _cohesion);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, _separation);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _alignment);
        }

        #endregion

        public void UpdateBehavior()
        {
            _velocity = Vector3.zero;
            var cohesion = Vector3.zero;
            var separation = Vector3.zero;
            var alignment = Vector3.zero;

            var cohesionCount = 0;
            var separationCount = 0;
            var position = _rigidbody.position;

            var boids = Physics.OverlapSphere(transform.position, _cohesionRadius);
            foreach (var boid in boids)
            {
                if (boid.GetComponent<HabitatResident>().Type != Type) continue;

                cohesion += boid.transform.position;
                cohesionCount++;

                var direction = position - boid.transform.position;
                var distance = direction.magnitude;
                if (boid.Equals(_thisBoid) || distance > _separationRadius) continue;
                separation += direction / distance;
                separationCount++;

                var behaviorComponent = boid.GetComponent<Boid>();
                if (behaviorComponent)
                {
                    alignment += behaviorComponent._velocity;
                }
            }

            if (cohesionCount > 0)
                cohesion = cohesion / cohesionCount - position;
            if (separationCount > 0)
            {
                separation = (separation / separationCount) * _separationCoef;
                alignment /= separationCount;
            }

            _velocity = Vector3.ClampMagnitude(cohesion + separation + alignment, _maxSpeed);

            _cohesion = cohesion;
            _separation = separation;
            _alignment = alignment;
        }
    }
}
