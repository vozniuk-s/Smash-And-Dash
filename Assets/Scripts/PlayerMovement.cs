using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum AnimStates
    {
        Idle, Run
    }

    [SerializeField] private float runOnGroundSpeed = 10f;
    [SerializeField] private float runOnAirSpeed = 3f;
    [SerializeField] private float runWithBlockSpeed = 0f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private GameObject[] ViewPoints;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool Grounded {
        get { return _animator.GetBool("Grounded"); }
        set { _animator.SetBool("Grounded", value); }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Grounded = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
            Grounded = true;
    }
    private void Update()
    {
        if (Grounded)
            _animator.SetInteger("AnimState", (int)AnimStates.Idle);
        else
            _animator.SetFloat("AirSpeedY", _rb.velocity.y);

        if (Input.GetButton("Horizontal"))
            Run();

        if (Input.GetButtonDown("Jump") && Grounded)
            Jump();
    }

    private void Run()
    {
        var dir = Input.GetAxis("Horizontal");
        float speed = 0f;

        if (_animator.GetBool("IdleBlock"))
            speed = runWithBlockSpeed;
        else if (Grounded)
            speed = runOnGroundSpeed;
        else
            speed = runOnAirSpeed;

        _animator.SetInteger("AnimState", (int)AnimStates.Run);
        _spriteRenderer.flipX = dir < 0f;
        _rb.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0, 0));

        foreach (GameObject obj in ViewPoints)
            obj.transform.Translate(new Vector3(dir * speed * Time.deltaTime, 0, 0));

    }

    private void Jump()
    {
        Grounded = false;
        _animator.SetTrigger("Jump");
        _rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
    }
}