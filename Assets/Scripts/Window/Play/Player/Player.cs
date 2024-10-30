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

    // �v���C���[�̌��݂̃X�s�[�h
    private float speed = 0.0f;

    // �v���C���[�̊e��X�s�[�h
    [SerializeField] private float walkingSpeed = 7.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float jumpingSpeedAspect = 1.2f;

    // �v���C���[�̃W�����v��
    [SerializeField] private float jumpForce = 5.0f;

    // �v���C���[�̒n�ʂɐڒn���Ă��邩�ǂ���
    private bool isGrounded = true;
    private int groundCount = 0;
    private int notGroundingFrame = 0;

    // �v���C���[�������Ă��邩�ǂ���
    private bool isRunning = false;

    // �v���C���[���󒆂ɂ��邩�ǂ���
    private bool isFlying = false;

    // �n�ʂ𗣂ꂽ�ۂ̈ړ��x�N�g��
    private Vector3 flyMovement = Vector3.zero;

    // �����Ă���ۂ̎���p
    [SerializeField] private float diffRunningFov = 30.0f;

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
        if (McControls.IsKey(Constants.CONTROL_SPRINT)) isRunning = true;

        int isFor = (McControls.IsKey(Constants.CONTROL_FOR)) ? 1 : 0;
        int isBack = (McControls.IsKey(Constants.CONTROL_BACK)) ? 1 : 0;
        int isLeft = (McControls.IsKey(Constants.CONTROL_LEFT)) ? 1 : 0;
        int isRight = (McControls.IsKey(Constants.CONTROL_RIGHT)) ? 1 : 0;

        // �ړ��������擾
        int vertical = isFor + isBack;
        int horizontal = isLeft + isRight;
        int diagonal = vertical + horizontal;

        if (isRunning && isFor == 1 && vertical == 1)
        {
            cam.fieldOfView = McVideos.Fov + diffRunningFov;
        }
        else
        {
            isRunning = false;
            cam.fieldOfView = McVideos.Fov;
        }

        // �ړ��x�N�g��
        Vector3 movement = Vector3.zero;

        // �ړ��x�N�g����Z������ݒ�
        if (diagonal != 0) // �ړ�
        {
            if (isRunning && isFor == 1 && vertical == 1)
            {
                speed = runningSpeed;
                anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_RUN);
            }
            else
            {
                speed = walkingSpeed;
                anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_WALK);
            }

            movement.z = -speed;
        }
        else // ��~
        {
            isRunning = false;
            cam.fieldOfView = McVideos.Fov;

            speed = 0.0f;
            rb.velocity = new Vector3(0, 0, 0);
            anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_BREATH);
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

        rb.velocity = new Vector3(movement.x, 0, movement.z);

    }

    public void FlyUpdate()
    {
        // ��юn�߂��ۂ̈ړ��x�N�g����ݒ�
        if (!isFlying)
        {
            flyMovement = rb.velocity;
            isFlying = true;
        }
        
    }

    public void Jump()
    {
        if (McControls.IsKey(Constants.CONTROL_JUMP) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isFlying = true;
        }
    }

    public void Execute()
    {   
        // �t���[���J�n
        FrameStart();

        // �v���C���[�̎��_�X�V
        ViewUpdate();

        Debug.Log("isGrounded: " + isGrounded + ", isFlying: " + isFlying);

        // �v���C���[�̒��n���ړ��X�V
        MoveUpdate();

        // �v���C���[�̃W�����v
        Jump();

        // �v���C���[�̔�s���ړ��X�V
        // if (!isGrounded) FlyUpdate();

        // �t���[���I��
        FrameFinish();
    }

    public void FrameStart()
    {
        if (groundCount != 0)
        {
            isGrounded = true;
            isFlying = false;
            notGroundingFrame = 0;
        }
        else
        {
            notGroundingFrame++;
            if (notGroundingFrame > 1)
            {
                isGrounded = false;
                isFlying = true;
            }
        }
    }

    public void FrameFinish()
    {
        groundCount = 0;
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

    void OnCollisionEnter(Collision collision)
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(Constants.TAG_BLOCK_TOP))
        {
            groundCount++;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        
    }

    
}
