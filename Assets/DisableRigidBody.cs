using UnityEngine;
public class DisableRigidBody : MonoBehaviour
{
    private Rigidbody rb;
    private bool isRigidBodyEnabled = true;
    public float timer = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (!IsInFrustum())
        {

            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (IsInFrustum())
        {

            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        if (rb.velocity == new Vector3(0f, 0f, 0f)) //Ceases physics calculations. 
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer <= 0 && rb.isKinematic != true)
            {
                rb.isKinematic = true;
                timer = 5;
            }
        }

        Collision();
    }

    private bool IsInFrustum()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, GetComponent<Renderer>().bounds);
    }

    public void Collision()
    {
        // Raycast to detect collisions with moving objects
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.deltaTime))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

    }
}
