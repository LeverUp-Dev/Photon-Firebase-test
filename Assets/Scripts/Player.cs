using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer spriteRenderer;
    
    public float speed = 3f;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = photonView.IsMine ? Color.blue : Color.red;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        float input = InputButton.VerticalInput;

        float distance = input * speed * Time.deltaTime;
        Vector3 targetPos = transform.position + Vector3.up * distance;

        playerRigidbody.MovePosition(targetPos);
    }
}
