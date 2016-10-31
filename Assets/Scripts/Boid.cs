using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Boid : MonoBehaviour
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

        [SerializeField]
        private Transform _leader;

        [SerializeField]
        private int _type = 0;

        public void SetType(int type)
        {
            _type = type;
        }

        private void Start()
        {
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
        }

        private IEnumerator CalculateVelocity()
        {
            for (;;)
            {
                _velocity = Vector3.zero;
                var cohesion = Vector3.zero;
                var separation = Vector3.zero;
                var alignment = Vector3.zero;

                var separationCount = 0;
                var position = _rigidbody.position;

                var boids = Physics.OverlapSphere(transform.position, _cohesionRadius);
                foreach (var boid in boids)
                {
                    cohesion += boid.transform.position;

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

                cohesion = cohesion / boids.Length - position;
                separation = (separation / separationCount) * _separationCoef;
                alignment /= separationCount;
                _velocity = Vector3.ClampMagnitude(cohesion + separation + alignment, _maxSpeed);

                yield return new WaitForSeconds(_calculateStepTime);
            }
        }
    }
}
