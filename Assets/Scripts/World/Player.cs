using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider bc;

    // プレイヤーの第何人称
    [SerializeField] private int viewMode = 1;

    // プレイヤーのパーツら
    [SerializeField] private GameObject parts;
    [SerializeField] private GameObject partsSub;

    private Vector3 canvasRightArmIdlePos;
    private Vector3 canvasRightArmIdleRot;
    private Vector3 canvasRightArmIdleScale;

    // PartsのGameObject
    [SerializeField] private List<GameObject> partsList;

    // PartsのHead
    [SerializeField] private GameObject head;

    // Partsの右腕
    [SerializeField] private GameObject rightArm;

    // 

    // PartsのHeadにある右腕オブジェクト
    [SerializeField] private GameObject canvasRightArm;
    [SerializeField] private GameObject canvasRightArmIdle;
    

    // プレイヤーのカメラ
    [SerializeField] public Camera cam;
    
    // プレイヤーのリーチ
    [SerializeField] private float reach = 5f;
    public float Reach {get { return reach; }}

    // プレイヤーのマテリアル
    [SerializeField] private Material mat;

    // テクスチャ
    [SerializeField] private Texture2D texture;

    // プレイヤーの回転角度
    private Vector2 rot;

    // プレイヤーの現在座標
    private Vector3 pos;
    public Vector3 Pos
    {
        get { return pos; }
    }

    // プレイヤーの現在のスピード
    private float speed = 0.0f;

    // プレイヤーの各種スピード
    [SerializeField] private float walkingSpeed = 7.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float jumpingSpeedAspect = 1.2f;

    // プレイヤーのジャンプ力
    [SerializeField] private float jumpForce = 5.0f;

    // プレイヤーの地面に接地しているかどうか
    private bool isGrounded = true;
    private int groundCount = 0;
    private int notGroundingFrame = 0;

    // プレイヤーが走っているかどうか
    private bool isRunning = false;

    // プレイヤーが空中にいるかどうか
    private bool isFlying = false;

    // プレイヤーが飛び始めた際の移動ベクトル
    private Vector3 flyDirection = Vector3.zero;

    // プレイヤーの移動ベクトル
    private Vector3 movement = Vector3.zero;

    // 走っている際の視野角
    [SerializeField] private float diffRunningFov = 30.0f;

    // プレイヤーの移動方向を回転
    Quaternion p180Rot = Quaternion.Euler(0, 180f, 0);
    Quaternion p90Rot = Quaternion.Euler(0, 90f, 0);
    Quaternion p45Rot = Quaternion.Euler(0, 45f, 0);
    Quaternion m90Rot = Quaternion.Euler(0, -90f, 0);
    Quaternion m45Rot = Quaternion.Euler(0, -45f, 0);

    // アニメーター
    private Animator animParts;
    private Animator animSub;
    private Animator animRightArm;

    // ブロックのセレクター
    [SerializeField] private GameObject selector;
    [SerializeField] private List<GameObject> selectorParts;
    [SerializeField] private Texture2D selectorTexture;
    [SerializeField] private List<Texture2D> destroyStageTextures;

    // フレーム内で設置したブロックのデータ
    [HideInInspector] public List<Vector4> frameSetBlocks;

    // フレーム内で破壊したブロックのデータ
    [HideInInspector] public List<Vector4> frameDestroyBlocks;

    // 最後に設置したブロックについて
    int lastSetFrame = 0;

    // 現在破壊しているブロックについて
    private bool isDestroying = false;
    private float destroyProgress = 0f;
    private float blockDurability = 10f;
    private Vector4 destroyingBlock;

    public void Init()
    {
        // プレイヤーの初期位置と回転角度
        pos = transform.position;
        rot = parts.transform.rotation.eulerAngles;

        // プレイヤーのBoxColliderを取得
        bc = GetComponent<BoxCollider>();

        // カーソルをロック
        McControls.CursorLock(true);

        // アニメーターの初期化
        animParts = parts.GetComponent<Animator>();
        animSub = partsSub.GetComponent<Animator>();
        animRightArm = canvasRightArm.GetComponent<Animator>();

        // プレイヤーの1人称視点での初期化
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

        // テクスチャを設定
        mat.mainTexture = texture;

        // セレクターのテクスチャを設定
        selectorParts = SupportFunc.GetChildren(selector);
        for (int i = 0; i < selectorParts.Count; i++)
        {
            selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = selectorTexture;
        }

        // フレーム内で設置、破壊したブロックのデータを初期化。Worldクラスで管理する。
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
        // プレイヤーの視点移動
        Vector2 mouseAxis = McControls.GetMouseAxis();

        rot.x = Mathf.Clamp(rot.x + mouseAxis.y, -90f, 90f);
        rot.y += mouseAxis.x;

        parts.transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);

        partsSub.transform.rotation = Quaternion.Euler(partsSub.transform.rotation.x, rot.y, partsSub.transform.rotation.z);
        head.transform.localRotation = Quaternion.Euler(rot.x, 0.0f, 0.0f);

        // セレクター位置更新
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

        // 移動方向を取得
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

        // 移動ベクトル
        movement = Vector3.zero;

        // 移動ベクトルのZ成分を設定
        if (diagonal != 0) // 移動
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
        else // 停止
        {
            isRunning = false;
            cam.fieldOfView = McVideos.Fov;

            speed = 0.0f;
            movement = Vector3.zero;
            animParts.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_BREATH);
        }

        // 移動ベクトルを設定
        if (vertical == 2 || horizontal == 2) // 停止
        {
            movement = Vector3.zero;
        }
        else if (diagonal == 2) // 斜め移動
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
        // 飛び始めた際の移動ベクトルを設定
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
        // プレイヤーの移動を行う。当たり判定もここで行う
        gameObject.transform.position += movement;
        pos += movement;
    }

    private void Attack(ref List<Vector4> targetBlocks)
    {
        if (McControls.IsKey(Constants.CONTROL_ATTACK) && !isDestroying)
        {
            // ブロックの破壊
            if (targetBlocks[Constants.TARGET_BLOCK_SELECT].w != 0)
            {
                isDestroying = true;
                destroyProgress = 0f; // 破壊段階を初期化
                blockDurability = 100f; // ブロックの耐久値を取得
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

                // 破壊段階に応じてテクスチャを変更
                int destroyStage = (int)((destroyProgress / blockDurability) * destroyStageTextures.Count);
                Debug.Log(destroyStage);

                if (destroyStage < destroyStageTextures.Count)
                {
                    for (int i = 0; i < selectorParts.Count; i++)
                    {
                        selectorParts[i].GetComponent<MeshRenderer>().material.mainTexture = destroyStageTextures[destroyStage];
                    }
                }

                // 耐久が0以下になった場合はブロックを破壊
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

            // ブロックの設置
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

            // ブロックの設置
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
        // フレーム開始
        FrameStart();

        // プレイヤーの視点更新
        ViewUpdate(ref targetBlocks);

        // プレイヤーの着地中移動更新
        if (isGrounded) MoveUpdate();

        // プレイヤーのジャンプ
        Jump();

        // プレイヤーの飛行中移動更新
        if (!isGrounded) FlyUpdate();

        Attack(ref targetBlocks);

        Use(ref targetBlocks);

        // プレイヤーの移動
        Transfer();

        // フレーム終了
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
