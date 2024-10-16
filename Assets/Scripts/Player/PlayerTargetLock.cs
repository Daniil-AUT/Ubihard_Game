using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTargetLock : MonoBehaviour
{
    private PlayerController playerController;
    private Dodge dodge;
    private PlayerCrouch playerCrouch;
    
    [SerializeField] private Camera cam;
    [SerializeField] private CinemachineFreeLook freelook;
    [SerializeField] public Transform targetIcon;
    [SerializeField] private string enemyTag; 
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    public bool isTargeting;
    private float maxAngle;
    public Transform currentTarget;
    private float mouseX;
    private float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        dodge = GetComponent<Dodge>(); 
        playerCrouch = GetComponent<PlayerCrouch>();
        maxAngle = 90f;
        freelook.m_XAxis.m_InputAxisName = "";
        freelook.m_YAxis.m_InputAxisName = "";
        targetIcon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTargeting)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            NewInputTarget(currentTarget);

            if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) > maxDistance)
            {
                isTargeting = false;
                currentTarget = null;
                targetIcon.gameObject.SetActive(false); 
            }
        }

        if (targetIcon)
            targetIcon.gameObject.SetActive(isTargeting);

        freelook.m_XAxis.m_InputAxisValue = mouseX; 
        freelook.m_YAxis.m_InputAxisValue = mouseY; 

        if (Input.GetKeyDown(KeyCode.Q) && !playerController.isJumping && !playerController.sprinting && !dodge.isDodging && !playerCrouch.isCrouching)
        {
            AssignTarget();
        }
    }

    private void AssignTarget()
    {
        if (isTargeting)
        {
            isTargeting = false;
            currentTarget = null;
            return;
        }

        GameObject closestTarget = ClosestTarget();
        if (closestTarget != null)
        {
            currentTarget = closestTarget.transform;
            isTargeting = true;
        }
    }

    private void NewInputTarget(Transform target)
    {
        if (!currentTarget)
        {
            return;
        }

        Vector3 dirToTarget = currentTarget.position - transform.position;
        dirToTarget.y = 0;

        LookAtTarget(dirToTarget);

        Vector3 viewPos = cam.WorldToViewportPoint(currentTarget.position);

        if (targetIcon)
        {
            targetIcon.position = currentTarget.position + new Vector3(targetLockOffset.x, targetLockOffset.y + 2.2f, 0); 
        }

        mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 3f;
        mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 3f;
    }

    private void LookAtTarget(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private GameObject ClosestTarget()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currentAngle = maxAngle;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float currentDistance = diff.magnitude;

            if (currentDistance < distance)
            {
                Vector3 viewPosition = cam.WorldToViewportPoint(go.transform.position);
                Vector2 newPosition = new Vector2(viewPosition.x - 0.5f, viewPosition.y - 0.5f);

                if (Vector3.Angle(diff.normalized, cam.transform.forward) < maxAngle)
                {
                    closest = go;
                    currentAngle = Vector3.Angle(diff.normalized, cam.transform.forward); 
                    distance = currentDistance;
                }
            }
        }
        return closest;
    }
}
