using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenSize = 10f;     
    [HideInInspector] public Transform m_Target;


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;              


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindPosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindPosition()
    {
        Vector3 targetPos = new Vector3();
        if (m_Target)
            targetPos = m_Target.position;

        targetPos.y = transform.position.y;

        m_DesiredPosition = targetPos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        return m_ScreenSize;
    }


    public void SetStartPositionAndSize()
    {
        FindPosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}