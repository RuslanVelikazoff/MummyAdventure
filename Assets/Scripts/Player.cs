using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerControl playerInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private VolumeController volume;

    [SerializeField] private float speed;
    [SerializeField] private float sensevityLook;

    private Vector2 moveVector;
    private Vector2 lookVector;

    public Action OnLose;
    public Action OnCollected;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControl();
        MoveContol();
    }
    private void FixedUpdate()
    {
        LookControl();
    }

    private void MoveContol()
    {
        moveVector = playerInput.Player.Move.ReadValue<Vector2>();

        rb.velocity = transform.forward * speed * moveVector.y + transform.right * speed * moveVector.x;
        if(moveVector.x != 0 || moveVector.y != 0)
        {
            volume.footStepSource.enabled = true;
        }
        else
        {
            volume.footStepSource.enabled = false;
        }
    }
    
    private void LookControl()
    {
        lookVector = playerInput.Player.Look.ReadValue<Vector2>().normalized;
        Quaternion deltaRotation = Quaternion.Euler(0,lookVector.x * Time.fixedDeltaTime * sensevityLook,0);
        transform.localRotation *= deltaRotation;
    }

    private void AnimationControl()
    {
        anim.SetFloat("yPos", moveVector.y);
        anim.SetFloat("xPos", moveVector.x);
    }

    private void OnEnable()
    {
        playerInput = new PlayerControl();
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
        playerInput = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            volume.PlaySound(VolumeController.Sounds.gameover);
            OnLose?.Invoke();
        }
        if(collision.gameObject.CompareTag("Treasure"))
        {
            volume.PlaySound(VolumeController.Sounds.take);
            OnCollected?.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
