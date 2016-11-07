using UnityEngine;

public class ModuleConnector : MonoBehaviour
{
	public string[] Tags;
	public bool IsDefault;

	void OnDrawGizmos()
	{
		var scale = 1.0f;

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * scale);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position - transform.right * scale);
		Gizmos.DrawLine(transform.position, transform.position + transform.right * scale);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * scale);

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.125f);
	}

	public bool CheckForChuck()
	{
		var origin = transform.position;
		var direction = transform.forward;
		Debug.Log (direction.x + " " + direction.y + " " + direction.z);
		var maxDistance = 2;
		RaycastHit hit;
		return !Physics.Raycast (origin, direction, out hit, maxDistance);
	}
}
