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

    // プレイヤーのジャンプ力
    [SerializeField] private float jumpForce = 5.0f;

    // プレイヤーの地面に接地しているかどうか
    private bool isGrounded = true;

    // 走っている際の視野角
    [SerializeField] private float diffRunningFov = 10.0f;

    public void Init()
    {
        // 各種コンポーネントの取得
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // プレイヤーの初期位置と回転角度
        pos = transform.position;
        rot = parts.transform.rotation.eulerAngles;

        McControls.CursorLock(true);
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
        head.transform.rotation = Quaternion.Euler(-rot.x, parts.transform.rotation.y, parts.transform.rotation.z);
    }

    public void MoveUpdate()
    {

    }

    public void Jump()
    {

    }

    public void Execute()
    {
        ViewUpdate();
        MoveUpdate();
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
