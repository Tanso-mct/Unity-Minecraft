using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider bc;

    // �v���C���[�̃p�[�c��
    [SerializeField] private GameObject parts;

    // �v���C���[��HeadGameObject
    [SerializeField] private GameObject head;

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
    public Vector3 Pos
    {
        get { return pos; }
    }

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

    // �v���C���[����юn�߂��ۂ̈ړ��x�N�g��
    private Vector3 flyDirection = Vector3.zero;

    // �v���C���[�̈ړ��x�N�g��
    private Vector3 movement = Vector3.zero;

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

        // �v���C���[��BoxCollider���擾
        bc = GetComponent<BoxCollider>();

        // �J�[�\�������b�N
        McControls.CursorLock(true);

        // �A�j���[�^�[�̏�����
        anim = parts.GetComponent<Animator>();

        // 1�l�̎��_�ł̃e�N�X�`����ݒ�
        // mat.mainTexture = firstPersonTexture;

        // 2�A3�l�̎��_�ł̃e�N�X�`����ݒ�
        mat.mainTexture = otherPersonTexture;
    }

    public void Create()
    {

    }

    public void Load()
    {

    }

    private void ViewUpdate()
    {
        // �v���C���[�̎��_�ړ�
        Vector2 mouseAxis = McControls.GetMouseAxis();

        rot.x = Mathf.Clamp(rot.x + mouseAxis.y, -90f, 90f);
        rot.y += mouseAxis.x;

        parts.transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
        head.transform.localRotation = Quaternion.Euler(rot.x, 0.0f, 0.0f);
    }

    private void MoveUpdate()
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
        movement = Vector3.zero;

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
            movement = Vector3.zero;
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
    }

    private void FlyUpdate()
    {
        // ��юn�߂��ۂ̈ړ��x�N�g����ݒ�
        if (!isFlying)
        {
            flyDirection = movement;
            isFlying = true;
        }
        
    }

    private void Jump()
    {
        if (McControls.IsKey(Constants.CONTROL_JUMP) && isGrounded)
        {
            
        }
    }

    private void Transfer()
    {
        // �v���C���[�̈ړ����s���B�����蔻��������ōs��
        gameObject.transform.position += movement;
        pos += movement;
    }

    public void Execute()
    {   
        // �t���[���J�n
        FrameStart();

        // �v���C���[�̎��_�X�V
        ViewUpdate();

        // �v���C���[�̒��n���ړ��X�V
        if (isGrounded) MoveUpdate();

        // �v���C���[�̃W�����v
        Jump();

        // �v���C���[�̔�s���ړ��X�V
        if (!isGrounded) FlyUpdate();

        // �v���C���[�̈ړ�
        Transfer();

        // �t���[���I��
        FrameFinish();
    }

    public void FrameStart()
    {
        
    }

    private void FrameFinish()
    {

    }

    public void LoadFromJson()
    {

    }

    public void SaveToJson()
    {

    }

    public void DestroyJson()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        // Debug.Log(collider.gameObject.name);
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
    }

    void OnTriggerExit(Collider collider)
    {
        // Debug.Log(collider.gameObject.name);
    }
}
