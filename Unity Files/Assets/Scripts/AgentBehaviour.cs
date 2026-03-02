using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class AgentBehaviour : Agent
{
    [Header("Setup")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material pass;
    [SerializeField] private Material fail;
    [SerializeField] private MeshRenderer floorRenderer;

    [Header("Procedural Movement")]
    [SerializeField] private Transform leftUpLeg;
    [SerializeField] private Transform rightUpLeg;
    [SerializeField] private Transform leftUpArm;
    [SerializeField] private Transform rightUpArm;
    [SerializeField] private float legSpeed = 5f;
    [SerializeField] private float legSwing = 30f;
    [SerializeField] private float armSpeed = 5f;
    [SerializeField] private float armSwing = 25f;

    private Rigidbody rb;
    private float moveSpeed = 2f;
    private float previousDistance;
    private float walkTime;
    private Quaternion leftUpLegStartRot;
    private Quaternion rightUpLegStartRot;
    private Quaternion leftUpArmStartRot;
    private Quaternion rightUpArmStartRot;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();

        // Store initial rotations for proper relative movement
        if (leftUpLeg != null) leftUpLegStartRot = leftUpLeg.localRotation;
        if (rightUpLeg != null) rightUpLegStartRot = rightUpLeg.localRotation;
        if (leftUpArm != null) leftUpArmStartRot = leftUpArm.localRotation;
        if (rightUpArm != null) rightUpArmStartRot = rightUpArm.localRotation;
    }

    public override void OnEpisodeBegin()
    {
        // Reset position
        transform.localPosition = new Vector3(Random.Range(-5.6f, 5.6f), 0, Random.Range(-5.5f, 5.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-5.6f, 5.6f), 0.5f, Random.Range(-5.5f, 5.5f));

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        previousDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(transform.localPosition, targetTransform.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        Vector3 movement = new Vector3(moveX, 0, moveZ);

        // Use Rigidbody for realistic movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Face direction of movement
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(movement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
        }

        // Move limbs visually
        UpdateLegs(movement.magnitude);
        UpdateArms(movement.magnitude);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.rightArrowKey.isPressed) horizontal = -1f;
        if (Keyboard.current.leftArrowKey.isPressed) horizontal = 1f;
        if (Keyboard.current.upArrowKey.isPressed) vertical = -1f;
        if (Keyboard.current.downArrowKey.isPressed) vertical = 1f;

        continuousActions[0] = horizontal;
        continuousActions[1] = vertical;
    }

    private void FixedUpdate()
    {
        float currentDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);

        if (currentDistance < previousDistance)
            AddReward(0.01f);
        else
            AddReward(-0.02f);

        previousDistance = currentDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Apple>(out Apple goal))
        {
            SetReward(+10f);
            floorRenderer.material = pass;
            EndEpisode();
        }
        else if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-5f);
            floorRenderer.material = fail;
            EndEpisode();
        }
    }

    private void UpdateLegs(float moveAmount)
    {
        if (leftUpLeg == null || rightUpLeg == null)
            return;

        if (moveAmount <= 0.1f)
        {
            leftUpLeg.localRotation = Quaternion.Slerp(leftUpLeg.localRotation, leftUpLegStartRot, Time.deltaTime * 5f);
            rightUpLeg.localRotation = Quaternion.Slerp(rightUpLeg.localRotation, rightUpLegStartRot, Time.deltaTime * 5f);
            return;
        }

        walkTime += Time.deltaTime * legSpeed;
        float angle = Mathf.Sin(walkTime) * legSwing;

        Vector3 swingAxis = Vector3.up;
        Quaternion swingRotation = Quaternion.AngleAxis(angle, swingAxis);

        leftUpLeg.localRotation = leftUpLegStartRot * swingRotation;
        rightUpLeg.localRotation = rightUpLegStartRot * Quaternion.Inverse(swingRotation);
    }

    private void UpdateArms(float moveAmount)
    {
        if (leftUpArm == null || rightUpArm == null)
            return;

        if (moveAmount <= 0.1f)
        {
            leftUpArm.localRotation = Quaternion.Slerp(leftUpArm.localRotation, leftUpArmStartRot, Time.deltaTime * 5f);
            rightUpArm.localRotation = Quaternion.Slerp(rightUpArm.localRotation, rightUpArmStartRot, Time.deltaTime * 5f);
            return;
        }

        float angle = Mathf.Sin(Time.time * armSpeed) * armSwing;

        Vector3 swingAxis = Vector3.up;

        Quaternion swingRotation = Quaternion.AngleAxis(-angle, swingAxis);

        leftUpArm.localRotation = leftUpArmStartRot * swingRotation;
        rightUpArm.localRotation = rightUpArmStartRot * Quaternion.Inverse(swingRotation);
    }
}