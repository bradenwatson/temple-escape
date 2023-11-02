using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Swing : MonoBehaviour
{
    [Header("Swing")]
    public Transform startSwingHand;
    public float maxDistance = 35;
    public LayerMask swingableLayer;

    public float pullStrength = 5000;
    public Rigidbody playerRigidbody;

    public LineRenderer lineRenderer;

    private SpringJoint joint;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    [Header("Input Action")]
    public InputActionProperty swingAction;
    public InputActionProperty pullAction;



    [Header("Joint")]
    public float jointSpring = 4.5f;
    public float jointDamper = 7;
    public float jointMassScale = 4.5f;

    private Vector3 swingPoint;
    private Vector3 currentGrapplePosition;
    private bool hasHit;
    private bool hasSphereHit;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetSwingPoint();

        if (swingAction.action.WasPressedThisFrame())
        {
            StartSwing();
        }
        else if (swingAction.action.WasReleasedThisFrame())
        {
            StopSwing();
        }
        PullRope();
        DrawRope();
    }


    public void PullRope()
    {
        if (!joint)
        {
            return;
        }
        
        if(pullAction.action.IsPressed())
        {
            
            Vector3 direction = (swingPoint - startSwingHand.position).normalized;
            playerRigidbody.AddForce(direction * pullStrength * Time.deltaTime);

            float distance = Vector3.Distance(playerRigidbody.position, swingPoint);
            joint.maxDistance = distance;
        }
    }

    public void StartSwing()
    {
        currentGrapplePosition = startSwingHand.position;
        if (hasHit)
        {
            joint = playerRigidbody.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distance = Vector3.Distance(playerRigidbody.position, swingPoint);
            joint.maxDistance = distance;

            joint.spring = jointSpring;
            joint.damper = jointDamper;
            joint.massScale = jointMassScale;
        }
    }

    public void StopSwing()
    {
        Destroy(joint);
        currentGrapplePosition = startSwingHand.position;
    }

    public void GetSwingPoint()
    {
        if(joint)
        {
            predictionPoint.gameObject.SetActive(false);
            return;
        }
        
        RaycastHit sphereCastHit;
        hasSphereHit = Physics.SphereCast(startSwingHand.position, predictionSphereCastRadius, startSwingHand.forward, out sphereCastHit, maxDistance, swingableLayer);

        RaycastHit raycastHit;
        hasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out raycastHit, maxDistance, swingableLayer);

        

        if (hasHit)
        {
            swingPoint = raycastHit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else if (hasSphereHit)
        {
            swingPoint = sphereCastHit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false) ;
        }
    }

    public void DrawRope()
    {

        
        if (!joint)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;

            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

            lineRenderer.SetPosition(0, startSwingHand.position);
            lineRenderer.SetPosition(1, currentGrapplePosition);
        }
    }
}
