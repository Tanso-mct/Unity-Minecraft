using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider bc;

    // プレイヤーのパーツら
    [SerializeField] private GameObject parts;

    // プレイヤーのHeadGameObject
    [SerializeField] private GameObject head;

    // プレイヤーのカメラ
    [SerializeField] private Camera cam;

    // プレイヤーのマテリアル
    [SerializeField] private Material mat;

    // １人称視点でのテクスチャ
    [SerializeField] private Texture2D firstPersonTexture;

    // 2、3人称視点でのテクスチャ
    [SerializeField] private Texture2D otherPersonTexture;

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
    private Animator anim;

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
        anim = parts.GetComponent<Animator>();

        // 1人称視点でのテクスチャを設定
        // mat.mainTexture = firstPersonTexture;

        // 2、3人称視点でのテクスチャを設定
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
        // プレイヤーの視点移動
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
                anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_RUN);
            }
            else
            {
                speed = walkingSpeed;
                anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_WALK);
            }

            movement.z = -speed;
        }
        else // 停止
        {
            isRunning = false;
            cam.fieldOfView = McVideos.Fov;

            speed = 0.0f;
            movement = Vector3.zero;
            anim.SetInteger(Constants.ANIM_TYPE, Constants.ANIM_PLAYER_BREATH);
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

    public void Execute()
    {   
        // フレーム開始
        FrameStart();

        // プレイヤーの視点更新
        ViewUpdate();

        // プレイヤーの着地中移動更新
        if (isGrounded) MoveUpdate();

        // プレイヤーのジャンプ
        Jump();

        // プレイヤーの飛行中移動更新
        if (!isGrounded) FlyUpdate();

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
