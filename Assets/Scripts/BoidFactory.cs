using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BoidFactory
    {
        private Transform _poolTransform;
        private readonly Stack<BoidStateMachine> _stack;

        public BoidFactory(BoidStateMachine prototype, int capacity, Transform poolTransform)
        {
            _poolTransform = poolTransform;
            _stack = new Stack<BoidStateMachine>();
            for (var i = 0; i < capacity; i++)
            {
                ReleaseBoid(Object.Instantiate(prototype));
            }
        }

        public BoidStateMachine CreateBoid()
        {
            var boid = _stack.Pop();
            boid.gameObject.SetActive(true);
            return boid;
        }

        public void ReleaseBoid(BoidStateMachine boid)
        {
            boid.gameObject.SetActive(false);
            boid.transform.position = Vector3.zero;
            boid.transform.SetParent(_poolTransform);
            _stack.Push(boid);
        }

        public void ReleaseBoids(List<BoidStateMachine> boids)
        {
            foreach (var boid in boids)
            {
                ReleaseBoid(boid);
            }
        }
    }
}
