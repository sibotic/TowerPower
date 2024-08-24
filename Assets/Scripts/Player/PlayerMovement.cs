using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 12;
    public float sprintSpeedMultiplier = 2;
    public float groundDrag;

    [Header("Jump")]
    public LayerMask groundLayer;
    public float jumpForce = 25;
    public float airDrag;
    
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] float _lowJumpMultiplier;
    [SerializeField] float _fallMultiplier;
    [SerializeField] Transform _groundCheckPosition;
    bool _grounded, _canJump = true;
    float _jumpCooldown = .25f;

    Rigidbody rb;
    Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");

        _grounded = Physics.Raycast(_groundCheckPosition.position, Vector3.down, .2f, groundLayer);
        rb.drag = _grounded ? groundDrag : airDrag;

        if(Input.GetKey(_jumpKey) && _grounded && _canJump){Jump();}
        if(!_grounded){ImproveJump();}

    }

    void FixedUpdate(){
        MovePlayer();
        SpeedControl();
        
    }

    void MovePlayer(){
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jump(){
        _canJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke("ResetJump", _jumpCooldown);
    }

    void ImproveJump(){
        float multiplier = 1;
        if(rb.velocity.y < 0){
            multiplier = _fallMultiplier;
        }else if (rb.velocity.y > 0 && !Input.GetKey(_jumpKey)){
            multiplier = _lowJumpMultiplier; 
        }

        rb.velocity += Vector3.up * Physics.gravity.y * (multiplier - 1) * Time.deltaTime;
    }

    void ResetJump(){
        _canJump = true;
    }
}
