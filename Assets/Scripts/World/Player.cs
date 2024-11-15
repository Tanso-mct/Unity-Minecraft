using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider bc;

    // �v���C���[�̑扽�l��
    [SerializeField] private int viewMode = 1;

    // �v���C���[�̃p�[�c��
    [SerializeField] private GameObject parts;
    [SerializeField] private GameObject partsSub;

    private Vector3 canvasRightArmIdlePos;
    private Vector3 canvasRightArmIdleRot;
    private Vector3 canvasRightArmIdleScale;

    // Parts��GameObject
    [SerializeField] private List<GameObject> partsList;

    // Parts��Head
    [SerializeField] private GameObject head;

    // Parts�̉E�r
    [SerializeField] private GameObject rightArm;

    // 

    // Parts��Head�ɂ���E�r�I�u�W�F�N�g
    [SerializeField] private GameObject canvasRightArm;
    [SerializeField] private GameObject canvasRightArmIdle;
    

    // �v���C���[�̃J����
    [SerializeField] public Camera cam;
    
    // �v���C���[�̃��[�`
    [SerializeField] private float reach = 5f;
    public float Reach {get { return reach; }}

    // �v���C���[�̃}�e���A��
    [SerializeField] private Material mat;

    // �e�N�X�`��
    [SerializeField] private Texture2D texture;

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
    private Animator animParts;
    private Animator animSub;
    private Animator animRightArm;

    // �u���b�N�̃Z���N�^�[
    [SerializeField] private GameObject selector;
    [SerializeField] private List<GameObject> selectorParts;
    [SerializeField] private Texture2D selectorTexture;
    [SerializeField] private List<Texture2D> destroyStageTextures;

    // �t���[�����Őݒu�����u���b�N�̃f�[�^
    [HideInInspector] public List<Vector4> frameSetBlocks;

    // �t���[�����Ŕj�󂵂��u���b�N�̃f�[�^
    [HideInInspector] public List<Vector4> frameDestroyBlocks;

    // �Ō�ɐݒu�����u���b�N�ɂ���
    int lastSetFrame = 0;

    // ���ݔj�󂵂Ă���u���b�N�ɂ���
    private bool isDestroying = false;
    private float destroyProgress = 0f;
    private float blockDurability = 10f;
    private Vector4 destroyingBlock;

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
        animParts = parts.GetComponent<Animator>();
        animSub = partsSub.GetComponent<Animator>();
        animRightArm = canvasRightArm.GetComponent<Animator>();

        // �v���C���[��1�l�̎��_�ł̏�����
        if (viewMode == 1)
        {
            for (int i = 0; i < partsList.Count; i++)
            {
                partsList[i].SetActive(false);
            }
            partsSub.SetActive(false);

            canvasRightArmIdle.SetActive(true);
        }
        else
        {
            parts.SetActive(true);
            partsSub.SetActive(true);

            canvasRightArmIdle.SetActive(false);
            canvasRightArm.SetActive(false);
        }

        // �e�N�X�`����ݒ�
        mat.mainTexture = texture;

        // �Z���N�^�[�̃e�N�X�`����ݒ�
        selectorParts = SupportFunc.GetChildren(selector);
        for (int i = 0; i < selectorParts.Count; i++)
        {
            selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
        }

        // �t���[�����Őݒu�A�j�󂵂��u���b�N�̃f�[�^���������BWorld�N���X�ŊǗ�����B
        frameSetBlocks = new List<Vector4>();
        frameDestroyBlocks = new List<Vector4>();
    }

    public void Create()
    {

    }

    public void Load()
    {

    }

    private void ViewUpdate(ref List<Vector4> targetBlocks)
    {
        // �v���C���[�̎��_�ړ�
        Vector2 mouseAxis = McControls.GetMouseAxis();

        rot.x = Mathf.Clamp(rot.x + mouseAxis.y, -90f, 90f);
        rot.y += mouseAxis.x;

        parts.transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);

        partsSub.transform.rotation = Quaternion.Euler(partsSub.transform.rotation.x, rot.y, partsSub.transform.rotation.z);
        head.transform.localRotation = Quaternion.Euler(rot.x, 0.0f, 0.0f);

        // �Z���N�^�[�ʒu�X�V
        if (targetBlocks[Constants.TARGET_BLOCK_SELECT].w != 0)
        {
            selector.SetActive(true);
            selector.transform.position = new Vector3
            (
                targetBlocks[Constants.TARGET_BLOCK_SELECT].x - Constants.WORLD_HALF_SIZE, 
                targetBlocks[Constants.TARGET_BLOCK_SELECT].y, 
                targetBlocks[Constants.TARGET_BLOCK_SELECT].z - Constants.WORLD_HALF_SIZE
            );
        }
        else
        {
            selector.SetActive(false);
        }
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
                animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_RUN);
            }
            else
            {
                speed = walkingSpeed;
                animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_WALK);
            }

            movement.z = -speed;
        }
        else // ��~
        {
            isRunning = false;
            cam.fieldOfView = McVideos.Fov;

            speed = 0.0f;
            movement = Vector3.zero;
            animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_BREATH);
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

    private void Attack(ref List<Vector4> targetBlocks)
    {
        if (McControls.IsKey(Constants.CONTROL_ATTACK) && !isDestroying)
        {
            // �u���b�N�̔j��
            if (targetBlocks[Constants.TARGET_BLOCK_SELECT].w != 0)
            {
                isDestroying = true;
                destroyProgress = 0f; // �j��i�K��������
                blockDurability = 100f; // �u���b�N�̑ϋv�l���擾
                destroyingBlock = targetBlocks[Constants.TARGET_BLOCK_SELECT];
            }
        }
        else if (McControls.IsKey(Constants.CONTROL_ATTACK))
        {
            if (viewMode != 1)
            {
                rightArm.SetActive(false);
                partsSub.SetActive(true);
                animSub.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }
            else
            {
                canvasRightArmIdle.SetActive(false);
                canvasRightArm.SetActive(true);
                animRightArm.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }

            if (isDestroying && targetBlocks[Constants.TARGET_BLOCK_SELECT].Equals(destroyingBlock))
            {
                destroyProgress += 1f;

                // �j��i�K�ɉ����ăe�N�X�`����ύX
                int destroyStage = (int)((destroyProgress / blockDurability) * destroyStageTextures.Count);
                Debug.Log(destroyStage);

                if (destroyStage < destroyStageTextures.Count)
                {
                    for (int i = 0; i < selectorParts.Count; i++)
                    {
                        selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = destroyStageTextures[destroyStage];
                    }
                }

                // �ϋv��0�ȉ��ɂȂ����ꍇ�̓u���b�N��j��
                if (destroyProgress >= blockDurability)
                {
                    isDestroying = false;
                    destroyProgress = 0f;
                    frameDestroyBlocks.Add(targetBlocks[Constants.TARGET_BLOCK_SELECT]);
                    for (int i = 0; i < selectorParts.Count; i++)
                    {
                        selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
                    }
                }
            }
            else if (isDestroying && !targetBlocks[Constants.TARGET_BLOCK_SELECT].Equals(destroyingBlock))
            {
                isDestroying = false;
                destroyProgress = 0f;
                for (int i = 0; i < selectorParts.Count; i++)
                {
                    selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
                }
            }
        }
        else if (McControls.IsKeyUp(Constants.CONTROL_ATTACK))
        {
            isDestroying = false;
            destroyProgress = 0f;
            for (int i = 0; i < selectorParts.Count; i++)
            {
                selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
            }
        }
    }

    private void Use(ref List<Vector4> targetBlocks)
    {
        if (McControls.IsKeyDown(Constants.CONTROL_USE))
        {
            if (viewMode != 1)
            {
                rightArm.SetActive(false);
                partsSub.SetActive(true);
                animSub.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }
            else
            {
                canvasRightArmIdle.SetActive(false);
                canvasRightArm.SetActive(true);
                animRightArm.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }

            // �u���b�N�̐ݒu
            if (targetBlocks[Constants.TARGET_BLOCK_SET].w == 0)
            {
                lastSetFrame = Time.frameCount;
                frameSetBlocks.Add
                (
                    new Vector4
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SET].x,
                        targetBlocks[Constants.TARGET_BLOCK_SET].y,
                        targetBlocks[Constants.TARGET_BLOCK_SET].z,
                        (float)Constants.BLOCK_TYPE.DIRT
                    )
                );
            }
        }
        else if (McControls.IsKey(Constants.CONTROL_USE))
        {
            if (viewMode != 1)
            {
                rightArm.SetActive(false);
                partsSub.SetActive(true);
                animSub.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }
            else
            {
                canvasRightArmIdle.SetActive(false);
                canvasRightArm.SetActive(true);
                animRightArm.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }

            // �u���b�N�̐ݒu
            if (targetBlocks[Constants.TARGET_BLOCK_SET].w == 0 && Time.frameCount - lastSetFrame > 10)
            {
                lastSetFrame = Time.frameCount;
                frameSetBlocks.Add
                (
                    new Vector4
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SET].x,
                        targetBlocks[Constants.TARGET_BLOCK_SET].y,
                        targetBlocks[Constants.TARGET_BLOCK_SET].z,
                        (float)Constants.BLOCK_TYPE.DIRT
                    )
                );
            }
        }

    }

    public void OnArmAnimEnd()
    {
        if (viewMode != 1)
        {
            rightArm.SetActive(true);
            partsSub.SetActive(false);
        }
        else
        {
            canvasRightArmIdle.SetActive(true);
            canvasRightArm.SetActive(false);
        }
    }

    public void Execute(List<Vector4> targetBlocks)
    {   
        // �t���[���J�n
        FrameStart();

        // �v���C���[�̎��_�X�V
        ViewUpdate(ref targetBlocks);

        // �v���C���[�̒��n���ړ��X�V
        if (isGrounded) MoveUpdate();

        // �v���C���[�̃W�����v
        Jump();

        // �v���C���[�̔�s���ړ��X�V
        if (!isGrounded) FlyUpdate();

        Attack(ref targetBlocks);

        Use(ref targetBlocks);

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
