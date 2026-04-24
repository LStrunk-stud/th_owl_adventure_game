using UnityEngine;

/// Controls player animations based on movement state.
/// Attach to the Player GameObject alongside PlayerMovement.
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator        _animator;
    private PlayerMovement  _movement;
    private SpriteRenderer  _spriteRenderer;

    // Animator parameter names
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    void Awake()
    {
        _animator       = GetComponent<Animator>();
        _movement       = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool moving = _movement != null && _movement.IsMoving();

        // Set animator parameter
        _animator.SetBool(IsMoving, moving);

        // Flip sprite based on movement direction
        if (moving)
            FlipTowardsTarget();
    }

    private void FlipTowardsTarget()
    {
        // Access target position via PlayerMovement
        // We use velocity direction from Rigidbody2D
        var rb = GetComponent<Rigidbody2D>();
        if (rb == null || _spriteRenderer == null) return;

        if (rb.linearVelocity.x > 0.01f)
            _spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.01f)
            _spriteRenderer.flipX = true;
    }
}