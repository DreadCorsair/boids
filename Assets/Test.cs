using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    private Vector3 anchor = Vector3.zero;
    [SerializeField] 
    private float _maxSpeed;
    [SerializeField]
    private Vector3 _velocity;

    [SerializeField]
    private Vector3 _delta = new Vector3(5, 5, 5);

    [SerializeField]
    private float _rotationSpeed;

    private Quaternion _rotation;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(Process());
	}

    private IEnumerator Process()
    {
        for (;;)
        {
            var q1 = Quaternion.AngleAxis(Random.Range(-_delta.x, _delta.x), transform.forward);
            var q2 = Quaternion.AngleAxis(Random.Range(-_delta.y, _delta.y), transform.right);
            var q3 = Quaternion.AngleAxis(Random.Range(-_delta.z, _delta.z), transform.up);

            _rotation *= q1 * q2 * q3;

            yield return new WaitForSeconds(1f);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 distanceToAnchor = anchor - transform.position;
        if (distanceToAnchor.sqrMagnitude > 10 * 10)
        {
            _velocity += distanceToAnchor / 10;
            //_velocity = Vector3.ClampMagnitude(boid.velocity, maxSpeed);
        }

        transform.position += _velocity * Time.deltaTime;

        transform.rotation = _rotation;//Quaternion.RotateTowards(transform.rotation, _rotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _velocity);
    }
}
