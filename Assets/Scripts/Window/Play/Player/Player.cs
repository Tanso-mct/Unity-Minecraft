using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // プレイヤーのパーツら
    [SerializeField] private GameObject parts;

    // プレイヤーのHeadGameObject
    [SerializeField] private GameObject head;

    // プレイヤーのデータ
    [SerializeField] private PlayerData data;

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

    // プレイヤーのBoxCollider
    private BoxCollider bc;

    // プレイヤーのRigidbody
    private Rigidbody rb;

    // プレイヤーの現在のスピード
    private float speed = 0.0f;

    // プレイヤーの各種スピード
    [SerializeField] private float walkingSpeed = 7.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float jumpingSpeedAspect = 0.5f;

    // プレイヤーのジャンプ力
    [SerializeField] private float jumpForce = 5.0f;

    // プレイヤーの地面に接地しているかどうか
    private bool isGrounded = true;

    // プレイヤーが走っているかどうか
    private bool isRunning = false;

    // 走っている際の視野角
    [SerializeField] private float diffRunningFov = 10.0f;

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
        // 各種コンポーネントの取得
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // プレイヤーの初期位置と回転角度
        pos = transform.position;
        rot = parts.transform.rotation.eulerAngles;

        // カーソルをロック
        McControls.CursorLock(true);

        // アニメーターの初期化
        anim = parts.GetComponent<Animator>();

        // 1人称視点でのテクスチャを設定
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
        // プレイヤーの視点移動
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

        // 移動方向を取得
        int vertical = isFor + isBack;
        int horizontal = isLeft + isRight;
        int diagonal = vertical + horizontal;

        // 移動ベクトル
        Vector3 movement = Vector3.zero;

        // 移動ベクトルのZ成分を設定
        if (diagonal != 0) // 移動
        {
            if (isRunning) speed = runningSpeed;
            else speed = walkingSpeed;

            if (isGrounded) movement.z = -speed;
            else movement.z = -speed * jumpingSpeedAspect;
        }
        else // 停止
        {
            isRunning = false;
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

        if (isGrounded) rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

    }

    public void Jump()
    {

    }

    public void Execute()
    {
        ViewUpdate();
        MoveUpdate();

        // アニメーション
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
