using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private const float dampTime = 0.2f;
    [SerializeField] private const float screenEdgeBuffer = 4f;
    [SerializeField] private const float minSize = 6.5f;                  
    [HideInInspector] public Transform[] targets; 

    private new Camera camera = null;                        
    private float zoomSpeed = 0;
    private Vector3 moveVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 desiredPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].gameObject.activeSelf)
            {
                averagePos += targets[i].position;
                numTargets++;
            }   
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        desiredPosition = averagePos;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / camera.aspect);
            }
        }
        
        size += screenEdgeBuffer;

        size = Mathf.Max(size, minSize);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = desiredPosition;

        camera.orthographicSize = FindRequiredSize();
    }
}