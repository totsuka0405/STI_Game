using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public Camera playerCamera;

    private Rigidbody rb;
    private float verticalLookRotation;

    private bool isGameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*
        if (GameManager.instance.IsGameStarted())
        {
            if (!isGameStarted)
            {
                isGameStarted = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }*/
        View();
        Moves();
    }

    void Moves()
    {
        // �v���C���[�̈ړ�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * currentSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

        // ���͂��s���Ă��Ȃ��ꍇ�A��]�̗͂�0�ɂ���
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void View()
    {
        // �v���C���[�̎��_����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // X����Z���̉�]���Œ�
        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localEulerAngles = new Vector3(-verticalLookRotation, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        // �}�E�X�J�[�\���̃��b�N/����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}
