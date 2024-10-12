    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g��ݒ�
    [SerializeField] private GameObject managerObject;
    private Manager manager;

    private void Awake()
    {
        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        Param.Init();

        // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g����Manager�N���X���擾
        manager = managerObject.GetComponent<Manager>();

        // Manager�N���X���p�������N���X��Awake�֐������s
        manager.BaseAwake();
    }

    void Start()
    {
        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        Param.Init();

        // FPS��ݒ�
        SetFps();

        // �𑜓x��ݒ�
        SetResolution();

        // Manager�N���X���p�������N���X��Start�֐������s
        manager.BaseStart();
    }

    void Update()
    {
        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        Param.Init();

        // Manager�N���X���p�������N���X��Update�֐������s
        manager.BaseUpdate();
    }

    // �V�[�����؂�ւ��ۂɌĂяo�����֐�
    private void Exit()
    {
        // Manager�N���X���p�������N���X��Exit�֐������s
        manager.BaseExit();
    }

    // �e�V�[���ŕK����x���s����BFPS�̐ݒ���s���B
    public int SetFps()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constants.SPECIFIED_FPS;

        return Constants.MSG_SUCCESS;
    }

    public int SetResolution()
    {
        Screen.SetResolution(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, true);

        return Constants.MSG_SUCCESS;
    }

    // param�Ɋi�[���ꂽ�V�[���������ɁA�V�[����ύX����B
    public int ChangeScene()
    {
        return Constants.MSG_NULL;
    }
}
