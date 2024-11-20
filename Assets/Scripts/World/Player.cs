using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    // Parts��Head�ɂ���E�r�I�u�W�F�N�g
    [SerializeField] private GameObject canvasRightArm;
    [SerializeField] private GameObject canvasRightArmIdle;


    // Parts��Head�ɂ���HoldingItem�I�u�W�F�N�g
    [SerializeField] private GameObject holdingObj;
    [SerializeField] private GameObject holdingObjIdle;

    // �v���C���[�̃J����
    [SerializeField] public Camera cam;

    // �����蔻��Ǘ��N���X
    [SerializeField] private McHitBoxAdmin hitBoxAdmin;
    private int hitBoxId;

    // Vaxel�Ǘ��N���X
    [SerializeField] private BlockAdmin blockAdmin;
    [SerializeField] private ItemAdmin itemAdmin;
    
    // �v���C���[�̃��[�`
    [SerializeField] private float reach = 5f;
    public float Reach {get { return reach; }}

    // �v���C���[�̃}�e���A��
    [SerializeField] private Material mat;

    // �e�N�X�`��
    [SerializeField] private Texture2D texture;

    // �v���C���[�̉�]�p�x
    private Vector2 rot;
    public Vector2 Rot
    {
        get { return rot; }
    }

    // �v���C���[�̌��ݍ��W
    private Vector3 pos;
    public Vector3 Pos
    {
        get { return pos; }
    }

    // �v���C���[��BoxCollider
    private BoxCollider bc;
    private float collisionOffset = 0.2f;

    // �v���C���[��Rigidbody
    private Rigidbody rb;

    // �v���C���[�̌��݂̃X�s�[�h
    private float speed = 0.0f;

    // �v���C���[�̊e��X�s�[�h
    [SerializeField] private float walkingSpeed = 7.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float walkFlySpeedAspect = 0.5f;
    [SerializeField] private float runFlySpeedAspect = 1f;


    // �v���C���[�̃W�����v��
    [SerializeField] private float jumpForce = 5.0f;

    // �v���C���[�̒n�ʂɐڒn���Ă��邩�ǂ���
    private bool isGrounded = true;

    // �v���C���[�����n�������ǂ���
    private bool isLanded = false;

    // �v���C���[�������Ă��邩�ǂ���
    private bool isRunning = false;

    // �v���C���[���󒆂ɂ��邩�ǂ���
    private bool isFlying = false;

    // �W�����v�x�N�g��
    private Vector3 jumpVec = Vector3.zero;

    // �����̋���
    [SerializeField] private float inertia = 0.9f;

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
    private Animator animHolding;

    // �u���b�N�̃Z���N�^�[
    [SerializeField] private GameObject selector;
    [SerializeField] private List<GameObject> selectorParts;
    [SerializeField] private Texture2D selectorTexture;
    [SerializeField] private List<Texture2D> destroyStageTextures;

    // �t���[�����Őݒu�����u���b�N�̃f�[�^
    [HideInInspector] public bool isFrameSetBlock = false;
    [HideInInspector] public Vector4 frameSetBlocks;

    // �t���[�����Ŕj�󂵂��u���b�N�̃f�[�^
    [HideInInspector] public bool isFrameDestroyBlock = false;
    [HideInInspector] public Vector4 frameDestroyBlocks;

    // �Ō�ɐݒu�����u���b�N�ɂ���
    int lastSetFrame = 0;

    // ���ݔj�󂵂Ă���u���b�N�ɂ���
    private bool isDestroying = false;
    private float destroyProgress = 0f;
    private float blockDurability = 10f;
    private Vector4 destroyingBlock;

    // �R���e�i��
    [SerializeField] private Inventory inventory;
    [SerializeField] private HotBar hotBar;

    // ���炩�̃C���x���g�����J���Ă��邩�ǂ���
    public bool isInventoryOpen = false;

    [SerializeField] private GameObject triggerHitBoxObj;
    

    public void Init()
    {
        // �v���C���[�̏����ʒu�Ɖ�]�p�x
        pos = transform.position;
        rot = parts.transform.rotation.eulerAngles;

        triggerHitBoxObj.SetActive(true);
        triggerHitBoxObj.transform.position = pos;

        // �v���C���[��BoxCollider���擾
        bc = GetComponent<BoxCollider>();

        // �v���C���[��Rigidbody���擾
        rb = GetComponent<Rigidbody>();

        // �J�[�\�������b�N
        McControls.CursorLock(true);

        // �R���e�i�̏�����
        inventory.Init();
        hotBar.Init();

        // �A�j���[�^�[�̏�����
        animParts = parts.GetComponent<Animator>();
        animSub = partsSub.GetComponent<Animator>();
        animRightArm = canvasRightArm.GetComponent<Animator>();
        animHolding = holdingObj.GetComponent<Animator>();

        // �v���C���[��1�l�̎��_�ł̏�����
        if (viewMode == 1)
        {
            for (int i = 0; i < partsList.Count; i++)
            {
                partsList[i].SetActive(false);
            }
            partsSub.SetActive(false);
        }
        else
        {
            parts.SetActive(true);
            partsSub.SetActive(true);

            canvasRightArmIdle.SetActive(false);
            canvasRightArm.SetActive(false);

            holdingObj.SetActive(false);
            holdingObjIdle.SetActive(false);
        }

        // �e�N�X�`����ݒ�
        mat.mainTexture = texture;

        // �Z���N�^�[�̃e�N�X�`����ݒ�
        selectorParts = SupportFunc.GetChildren(selector);
        for (int i = 0; i < selectorParts.Count; i++)
        {
            selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
        }

        // �v���C���[�̓����蔻���ݒ�
        hitBoxId = hitBoxAdmin.RegisterHitBox(pos, bc.size, Vector3.zero);
    }

    public void Create()
    {

    }

    public void Load()
    {

    }

    private void ViewUpdate(ref List<Vector4> targetBlocks)
    {
        if (isInventoryOpen) return;

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
        if (isInventoryOpen) return;

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
        if (!isFlying && !isGrounded)
        {
            isFlying = true;
            jumpVec = movement;
        }
        else if (isFlying && !isLanded)
        {
            int isFor = (McControls.IsKey(Constants.CONTROL_FOR)) ? 1 : 0;
            int isBack = (McControls.IsKey(Constants.CONTROL_BACK)) ? 1 : 0;
            int isLeft = (McControls.IsKey(Constants.CONTROL_LEFT)) ? 1 : 0;
            int isRight = (McControls.IsKey(Constants.CONTROL_RIGHT)) ? 1 : 0;

            // �ړ��������擾
            int vertical = isFor + isBack;
            int horizontal = isLeft + isRight;
            int diagonal = vertical + horizontal;

            // �ړ��x�N�g��
            Vector3 newMovement = Vector3.zero;

            // �ړ��x�N�g����Z������ݒ�
            if (diagonal != 0) // �ړ�
            {
                if (isRunning && isFor == 1 && vertical == 1)
                {
                    speed = runningSpeed * runFlySpeedAspect;
                    animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_RUN);
                }
                else
                {
                    speed = walkingSpeed * walkFlySpeedAspect;
                    animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_WALK);
                }

                newMovement.z = -speed;
            }

            // �ړ��x�N�g����ݒ�
            if (diagonal == 2) // �΂߈ړ�
            {
                if (isFor == 1)
                {
                    if (isLeft == 1) newMovement = m45Rot * newMovement;
                    else if (isRight == 1) newMovement = p45Rot * newMovement;
                }
                else if (isBack == 1)
                {
                    newMovement = p180Rot * newMovement;
                    if (isLeft == 1) newMovement = p45Rot * newMovement;
                    else if (isRight == 1) newMovement = m45Rot * newMovement;
                }
            }
            else if (vertical == 1)
            {
                if (isBack == 1) newMovement = p180Rot * newMovement;
            }
            else if (horizontal == 1)
            {
                if (isLeft == 1) newMovement = m90Rot * newMovement;
                else if (isRight == 1) newMovement = p90Rot * newMovement;
            }

            Quaternion rotate = Quaternion.Euler(0, parts.transform.eulerAngles.y, 0);
            newMovement = rotate * newMovement;

            movement = Vector3.Lerp(jumpVec, newMovement, inertia);
        }
        else if (isFlying && isLanded)
        {
            isFlying = false;
            isLanded = false;
        }
    }

    private void Jump()
    {
        if (isInventoryOpen) return;

        if (McControls.IsKey(Constants.CONTROL_JUMP) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void UpdateHitBox()
    {
        // �v���C���[�̓����蔻����X�V
        hitBoxAdmin.UpdatePos(hitBoxId, pos);
    }

    private void HandAnim()
    {
        if (viewMode != 1)
        {
            rightArm.SetActive(false);
            partsSub.SetActive(true);
            animSub.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
        }
        else
        {
            if (hotBar.GetIsContain(hotBar.SelectingSlot) == 0)
            {
                canvasRightArmIdle.SetActive(false);
                canvasRightArm.SetActive(true);
                animRightArm.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }
            else
            {
                holdingObjIdle.SetActive(false);
                holdingObj.SetActive(true);
                animHolding.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_USE);
            }
        }
    }

    private void Attack(ref List<Vector4> targetBlocks)
    {
        if (isInventoryOpen) return;

        if (McControls.IsKey(Constants.CONTROL_ATTACK)) HandAnim();

        if (McControls.IsKey(Constants.CONTROL_ATTACK) && !isDestroying)
        {
            // �u���b�N�̔j��
            if 
            (
                targetBlocks[Constants.TARGET_BLOCK_SELECT].w != 0 && 
                targetBlocks[Constants.TARGET_BLOCK_SELECT].w != Constants.BLOCK_TYPE_CANT_SET
            ){
                isDestroying = true;
                destroyProgress = 0f; // �j��i�K��������
                blockDurability = 100f; // �u���b�N�̑ϋv�l���擾
                destroyingBlock = targetBlocks[Constants.TARGET_BLOCK_SELECT];
            }
        }
        else if (McControls.IsKey(Constants.CONTROL_ATTACK))
        {
            if (isDestroying && targetBlocks[Constants.TARGET_BLOCK_SELECT].Equals(destroyingBlock))
            {
                destroyProgress += 20f;

                // �j��i�K�ɉ����ăe�N�X�`����ύX
                int destroyStage = (int)((destroyProgress / blockDurability) * destroyStageTextures.Count);

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

                    isFrameDestroyBlock = true;

                    // �u���b�N�̔j��
                    blockAdmin.Break(targetBlocks[Constants.TARGET_BLOCK_SELECT], ref frameDestroyBlocks);

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

    private bool IsBlockInPlayer(Vector4 target)
    {
        Vector3 boxOrigin = new Vector3
        (
            pos.x - bc.size.x / 2, pos.y, pos.z - bc.size.z / 2
        );

        Vector3 boxOriginFloatPart = new Vector3
        (
            boxOrigin.x - (float)((int)boxOrigin.x),
            boxOrigin.y - (float)((int)boxOrigin.y),
            boxOrigin.z - (float)((int)boxOrigin.z)
        );

        Vector3Int blockOrigin = new Vector3Int
        (
            (int)boxOrigin.x, (int)boxOrigin.y, (int)boxOrigin.z
        );

        if (boxOriginFloatPart.x >= 0.5) blockOrigin.x++;
        if (boxOriginFloatPart.y >= 0.5) blockOrigin.y++;
        if (boxOriginFloatPart.z >= 0.5) blockOrigin.z++;
        if (boxOriginFloatPart.x <= -0.5) blockOrigin.x--;
        if (boxOriginFloatPart.y <= -0.5) blockOrigin.y--;
        if (boxOriginFloatPart.z <= -0.5) blockOrigin.z--;

        Vector3 boxOpposite = new Vector3
        (
            pos.x + bc.size.x / 2,
            pos.y + bc.size.y,
            pos.z + bc.size.z / 2
        );

        Vector3 boxOppositeFloatPart = new Vector3
        (
            boxOpposite.x - (float)((int)boxOpposite.x),
            boxOpposite.y - (float)((int)boxOpposite.y),
            boxOpposite.z - (float)((int)boxOpposite.z)
        );

        Vector3Int blockOpposite = new Vector3Int
        (
            (int)boxOpposite.x, (int)boxOpposite.y, (int)boxOpposite.z
        );

        if (boxOppositeFloatPart.x >= 0.5) blockOpposite.x++;
        if (boxOppositeFloatPart.y >= 0.5) blockOpposite.y++;
        if (boxOppositeFloatPart.z >= 0.5) blockOpposite.z++;
        if (boxOppositeFloatPart.x <= -0.5) blockOpposite.x--;
        if (boxOppositeFloatPart.y <= -0.5) blockOpposite.y--;
        if (boxOppositeFloatPart.z <= -0.5) blockOpposite.z--;

        blockOrigin.x += Constants.WORLD_HALF_SIZE;
        blockOrigin.z += Constants.WORLD_HALF_SIZE;

        blockOpposite.x += Constants.WORLD_HALF_SIZE;
        blockOpposite.z += Constants.WORLD_HALF_SIZE;

        for (int x = blockOrigin.x; x <= blockOpposite.x; x++)
        {
            for (int y = blockOrigin.y; y <= blockOpposite.y; y++)
            {
                for (int z = blockOrigin.z; z <= blockOpposite.z; z++)
                {
                    if (target.x == x && target.y == y && target.z == z)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void Use(ref List<Vector4> targetBlocks)
    {
        if (isInventoryOpen) return;

        if (McControls.IsKeyDown(Constants.CONTROL_USE))
        {
            if 
            (
                targetBlocks[Constants.TARGET_BLOCK_SET].w == 0 && 
                !IsBlockInPlayer(targetBlocks[Constants.TARGET_BLOCK_SET]) &&
                hotBar.GetIsContain(hotBar.SelectingSlot) != 0
            ){
                HandAnim();

                lastSetFrame = Time.frameCount;
                isFrameSetBlock = true;
                if (blockAdmin.IsUseable((int)targetBlocks[Constants.TARGET_BLOCK_SELECT].w))
                {
                    // �u���b�N�̎g�p
                    blockAdmin.Use
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SELECT], inventory
                    );
                }
                else
                {
                    // �u���b�N�̐ݒu
                    blockAdmin.Set
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SET],
                        ref frameSetBlocks, hotBar, hotBar.SelectingSlot
                    );
                }
            }
        }
        else if (McControls.IsKey(Constants.CONTROL_USE))
        {
            if 
            (
                targetBlocks[Constants.TARGET_BLOCK_SET].w == 0 && Time.frameCount - lastSetFrame > 10 &&
                !IsBlockInPlayer(targetBlocks[Constants.TARGET_BLOCK_SET]) &&
                hotBar.GetIsContain(hotBar.SelectingSlot) != 0
            ){
                HandAnim();
                
                lastSetFrame = Time.frameCount;
                isFrameSetBlock = true;

                if (blockAdmin.IsUseable((int)targetBlocks[Constants.TARGET_BLOCK_SELECT].w))
                {
                    // �u���b�N�̎g�p
                    blockAdmin.Use
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SELECT], inventory
                    );
                }
                else
                {
                    // �u���b�N�̐ݒu
                    blockAdmin.Set
                    (
                        targetBlocks[Constants.TARGET_BLOCK_SET],
                        ref frameSetBlocks, hotBar, hotBar.SelectingSlot
                    );
                }
            }
        }

    }

    public void PickUpItem(GameObject entityObj)
    {
        EntityItem entityItem = entityObj.GetComponent<EntityItem>();
        if (entityItem != null)
        {
            // �C���x���g���ɃA�C�e����ǉ�
            inventory.AddContent(entityItem.ID);
            UpdateHotBarSlot();
            Destroy(entityObj);
        }
    }

    public void ResetFrameBlocks()
    {
        isFrameSetBlock = false;
        isFrameDestroyBlock = false;
    }

    private void UpdateHotBarSlot()
    {
        if (hotBar.SelectSlot(hotBar.SelectingSlot)) // ���炩�̃A�C�e���������Ă���
        {
            canvasRightArmIdle.SetActive(false);
            holdingObjIdle.SetActive(true);
        }
        else
        {
            canvasRightArmIdle.SetActive(true);
            holdingObjIdle.SetActive(false);
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
            canvasRightArm.SetActive(false);
            canvasRightArmIdle.SetActive(true);
        }

        UpdateHotBarSlot();
    }

    public void OnHoldingAnimEnd()
    {
        holdingObj.SetActive(false);
        holdingObjIdle.SetActive(true);

        UpdateHotBarSlot();
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
        FlyUpdate();

        Attack(ref targetBlocks);

        Use(ref targetBlocks);

        // �v���C���[�̈ړ�
        UpdateHitBox();

        // �t���[���I��
        FrameFinish();
    }

    public void UpdateInfos()
    {
        // �ʒu���̍X�V
        if (!isInventoryOpen)
        {
            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }
        else
        {
            if (isFlying && !isLanded)
            {
                movement.y = rb.velocity.y;
                rb.velocity = movement;
            }
            else rb.velocity = Vector3.zero;
        }

        // EntityItem�Ƃ̓����蔻��g���K�[�̈ʒu���X�V
        triggerHitBoxObj.transform.position = transform.position;

        // �C���x���g���̍X�V
        blockAdmin.FinishedSet(frameSetBlocks);
        blockAdmin.FinishedBreak(frameDestroyBlocks);
    }

    private void HotBarSelectSlot(int slot)
    {
        hotBar.SelectSlot(slot);
        UpdateHotBarSlot();
    }

    public void FrameStart()
    {
        // �v���C���[�̌��ݍ��W���擾
        pos = transform.position;

        if (!isInventoryOpen) // �v���C���[�̃z�b�g�o�[�̑I���X���b�g���X�V
        {
            if (McControls.IsKeyDown(Constants.CONTROL_HS1)) HotBarSelectSlot(1);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS2)) HotBarSelectSlot(2);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS3)) HotBarSelectSlot(3);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS4)) HotBarSelectSlot(4);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS5)) HotBarSelectSlot(5);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS6)) HotBarSelectSlot(6);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS7)) HotBarSelectSlot(7);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS8)) HotBarSelectSlot(8);
            else if (McControls.IsKeyDown(Constants.CONTROL_HS9)) HotBarSelectSlot(9);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0)
            {
                if (hotBar.SelectingSlot == 1) HotBarSelectSlot(9);
                else HotBarSelectSlot(hotBar.SelectingSlot - 1);
            }
            else if (scroll < 0)
            {
                if (hotBar.SelectingSlot == 9) HotBarSelectSlot(1);
                else HotBarSelectSlot(hotBar.SelectingSlot + 1);
            }

            if (McControls.IsKeyDown(Constants.CONTROL_DROP_ITEM))
            {
                // �z�b�g�o�[�̃A�C�e�����h���b�v
                hotBar.RemoveContent(1, hotBar.SelectingSlot);
            }
        }
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

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (rb.velocity.y <= 0.001f && rb.velocity.y >= -0.001f)
            {
                if (contact.point.y >= pos.y - collisionOffset && contact.point.y <= pos.y + collisionOffset && !isFlying)
                {
                    isGrounded = true;
                }
                else if 
                (
                    contact.point.y >= pos.y - collisionOffset && contact.point.y <= pos.y + collisionOffset && isFlying)
                {
                    isLanded = true;
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Debug.Log("Collision Stay with " + collision.gameObject.name);
    }

    void OnCollisionExit(Collision collision)
    {
        
    }
}
