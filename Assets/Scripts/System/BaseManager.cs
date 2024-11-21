using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseManager : MonoBehaviour
{
    // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g��ݒ�
    [SerializeField] private GameObject managerObject;
    private Manager manager;
    [SerializeField] private McOption option;

    private void Awake()
    {
        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        Param.Init();

        // ���[���h����ǂݍ���
        World.LoadInfoFromJson();

        // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g����Manager�N���X���擾
        manager = managerObject.GetComponent<Manager>();

        // Manager�N���X���p�������N���X��Awake�֐������s
        manager.BaseAwake();
    }

    void Start()
    {
        // �e��ݒ�̏�����
        option.Init();

        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        Param.Init();

        // FPS��ݒ�
        SetFps();

        // �𑜓x��ݒ�
        SetResolution();

        // Manager�N���X���p�������N���X��Start�֐������s
        manager.BaseStart();

        // ���b�Z�[�W����
        ProcessParam();
    }

    void Update()
    {
        // ���b�Z�[�W�����̂��߂̃p�����[�^��������a
        Param.Init();

        // Manager�N���X���p�������N���X��Update�֐������s
        manager.BaseUpdate();

        // ���b�Z�[�W����
        ProcessParam();
    }

    // �V�[�����؂�ւ��ۂɌĂяo�����֐�
    private void Exit()
    {
        // Manager�N���X���p�������N���X��Exit�֐������s
        manager.BaseExit();
    }

    public void ProcessParam()
    {
        switch (Param.msg)
        {
            case Constants.MSG_NULL:
                break;
            case Constants.MSG_CHANGE_SCENE:
                ChangeScene();
                break;
            case Constants.MSG_QUIT_GAME:
                QuitGame();
                break;
            default:
                break;
        }
    }

    public void QuitGame()
    {
        option.Save();

    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
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
        if (Param.strPar == Constants.SCENE_MENU)
        {
            Exit();
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.SCENE_MENU);
        }
        else if (Param.strPar == Constants.SCENE_PLAY)
        {
            Exit();
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.SCENE_PLAY);
        }
        else
        {
            return Constants.MSG_FAILED;
        }

        return Constants.MSG_FAILED;
    }
}
