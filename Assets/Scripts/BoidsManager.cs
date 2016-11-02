using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BoidsManager : MonoBehaviour
    {
        [SerializeField]
        private float _maxUpdateTime = 1f;

        private float _updateTime;

        private List<BoidStateMachine> _objects;
        private Coroutine _updateRoutine;

        private Transform _boidsTransform;

        public void Initialize(float maxUpdateTime)
        {
            _boidsTransform = new GameObject("Boids").transform;
            _boidsTransform.SetParent(transform);

            _objects = new List<BoidStateMachine>();

            _maxUpdateTime = maxUpdateTime;
            _updateTime = _maxUpdateTime;
        }

        public void AddObject(BoidStateMachine obj)
        {
            obj.transform.SetParent(_boidsTransform);
            _objects.Add(obj);
            _maxUpdateTime = _maxUpdateTime / _objects.Count;
        }

        public void StartProcess()
        {
            _updateRoutine = StartCoroutine(UpdateLogic());
        }

        public void StopProcess()
        {
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
            }
        }

        public List<BoidStateMachine> ReturnObjects()
        {
            var objects = _objects;
            _objects = new List<BoidStateMachine>();
            return objects;
        }

        private IEnumerator UpdateLogic()
        {
            for (;;)
            {
                foreach (var obj in _objects)
                {
                    obj.UpdateLogic();

                    yield return new WaitForSeconds(_updateTime);
                }
            }
        }
    }
}
