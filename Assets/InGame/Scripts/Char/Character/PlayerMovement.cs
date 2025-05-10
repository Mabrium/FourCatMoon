using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isMoving = false;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isMoving) // �̵� ���� �ƴ� ���� �Է��� ����
        {
            float moveX = Input.GetAxisRaw("Horizontal"); // �¿� �̵�
            float moveY = Input.GetAxisRaw("Vertical");   // ���� �̵�

            // �밢�� �Է� ���� (�� ���� �� ���⸸ �̵�)
            if (moveX != 0) moveY = 0;

            moveInput = new Vector2(moveX, moveY);

            if (moveInput != Vector2.zero) // ���� �Է��� ���� ���� �̵�
            {
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        isMoving = true;
        Vector3 targetPosition = transform.position + (Vector3)moveInput; // 1ĭ �̵�

        // �̵� ���� ���� Ȯ��
        if (CanMove(moveInput))
        {
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;

            while (elapsedTime < 1f / moveSpeed)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * moveSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition; // �̵� �Ϸ� �� ��ġ ����
        }

        isMoving = false;
    }

    private bool CanMove(Vector2 direction)
    {
        if (direction == Vector2.zero) return false;

        // Raycast�� ���� ���� �ִ��� Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Wall"));
        return hit.collider == null; // ���� ������ �̵� ����
    }
}
