using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �v���C���[�̃p�[�c��
    [SerializeField] private GameObject parts;

    // �v���C���[��HeadGameObject
    [SerializeField] private GameObject head;

    // �v���C���[�̃f�[�^
    [SerializeField] private PlayerData data;

    // �v���C���[�̃J����
    [SerializeField] private Camera cam;

    // �v���C���[�̃}�e���A��
    [SerializeField] private Material mat;

    // �P�l�̎��_�ł̃e�N�X�`��
    [SerializeField] private Texture2D firstPersonTexture;

    // 2�A3�l�̎��_�ł̃e�N�X�`��
    [SerializeField] private Texture2D otherPersonTexture;

    // �v���C���[�̉�]�p�x
    private Vector2 rot;

    // �v���C���[�̌��ݍ��W
    private Vector3 pos;

    // �v���C���[��BoxCollider
    private BoxCollider bc;

    // �v���C���[��Rigidbody
    private Rigidbody rb;

    // �v���C���[�̌��݂̃X�s�[�h
    private float speed = 0.0f;

    // �v���C���[�̊e��X�s�[�h
    [SerializeField] private float walkingSpeed = 7.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float jumpingSpeedAspect = 0.5f;

    // �v���C���[�̃W�����v��
    [SerializeField] private float jumpForce = 5.0f;

    // �v���C���[�̒n�ʂɐڒn���Ă��邩�ǂ���
    private bool isGrounded = true;

    // �v���C���[�������Ă��邩�ǂ���
    private bool isRunning = false;

    // �����Ă���ۂ̎���p
    [SerializeField] private float diffRunningFov = 10.0f;

    // �v���C���[�̈ړ���������]
    Quaternion p180Rot = Quaternion.Euler(0, 180f, 0);
    Quaternion p90Rot = Quaternion.Euler(0, 90f, 0);
    Quaternion p45Rot = Quaternion.Euler(0, 45f, 0);
    Quaternion m90Rot = Quaternion.Euler(0, -90f, 0);
    Quaternion m45Rot = Quaternion.Euler(0, -45f, 0);

    // �A�j���[�^�[
    private Animator anim;

    public void Init()
    {
        // �e��R���|�[�l���g�̎擾
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // �v���C���[�̏����ʒu�Ɖ�]�p�x
        pos = transform.position;
        rot = parts.transform.rotation.eulerAngles;

        // �J�[�\�������b�N
        McControls.CursorLock(true);

        // �A�j���[�^�[�̏�����
        anim = parts.GetComponent<Animator>();

        // 1�l�̎��_�ł̃e�N�X�`����ݒ�
        // mat.mainTexture = firstPersonTexture;

        // Debug
        mat.mainTexture = otherPersonTexture;
    }

    public void Create()
    {

    }

    public void Load()
    {

    }

    public void ViewUpdate()
    {
        // �v���C���[�̎��_�ړ�
        Vector2 mouseAxis = McControls.GetMouseAxis();

        rot.x = Mathf.Clamp(rot.x + mouseAxis.y, -90f, 90f);
        rot.y += mouseAxis.x;

        parts.transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
        head.transform.localRotation = Quaternion.Euler(rot.x, 0.0f, 0.0f);
    }

    public void MoveUpdate()
    {
        if (McControls.IsKeyDown(Constants.CONTROL_SPRINT)) isRunning = true;

        int isFor = (McControls.IsKey(Constants.CONTROL_FOR)) ? 1 : 0;
        int isBack = (McControls.IsKey(Constants.CONTROL_BACK)) ? 1 : 0;
        int isLeft = (McControls.IsKey(Constants.CONTROL_LEFT)) ? 1 : 0;
        int isRight = (McControls.IsKey(Constants.CONTROL_RIGHT)) ? 1 : 0;

        if (isRunning) cam.fieldOfView = McVideos.Fov + diffRunningFov;
        else cam.fieldOfView = McVideos.Fov;

        // �ړ��������擾
        int vertical = isFor + isBack;
        int horizontal = isLeft + isRight;
        int diagonal = vertical + horizontal;

        // �ړ��x�N�g��
        Vector3 movement = Vector3.zero;

        // �ړ��x�N�g����Z������ݒ�
        if (diagonal != 0) // �ړ�
        {
            if (isRunning) speed = runningSpeed;
            else speed = walkingSpeed;

            if (isGrounded) movement.z = -speed;
            else movement.z = -speed * jumpingSpeedAspect;
        }
        else // ��~
        {
            isRunning = false;
        }

        // �ړ��x�N�g����ݒ�
        if (vertical == 2 || horizontal == 2) // ��~
        {
            movement = Vector3.zero;
        }
        else if (diagonal == 2) // �΂߈ړ�
        {
            if (isFor == 1)
            {
                if (isLeft == 1) movement = m45Rot * movement;
                else if (isRight == 1) movement = p45Rot * movement;
            }
            else if (isBack == 1)
            {
                movement = p180Rot * movement;
                if (isLeft == 1) movement = p45Rot * movement;
                else if (isRight == 1) movement = m45Rot * movement;
            }
        }
        else if (vertical == 1)
        {
            if (isBack == 1) movement = p180Rot * movement;
        }
        else if (horizontal == 1)
        {
            if (isLeft == 1) movement = m90Rot * movement;
            else if (isRight == 1) movement = p90Rot * movement;
        }

        Quaternion rotate = Quaternion.Euler(0, parts.transform.eulerAngles.y, 0);
        movement = rotate * movement;

        if (isGrounded) rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

    }

    public void Jump()
    {

    }

    public void Execute()
    {
        ViewUpdate();
        MoveUpdate();

        // �A�j���[�V����
        anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_WALK);
    }

    public void LoadFromJson()
    {

    }

    public void SaveToJson()
    {

    }

    public void DestoroyJson()
    {

    }
}
