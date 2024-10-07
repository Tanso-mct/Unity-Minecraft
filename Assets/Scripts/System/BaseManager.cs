    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g��ݒ�
    [SerializeField] private GameObject managerObject;
    private Manager manager;

    /* 
     * ����̃v���g�^�C�v�łł͈ȉ��̊֐��ł̂�Unity��Awake�AStart�AUpdate�֐����g�p����
     * ���̃X�N���v�g�ł������g�����Ƃ͐����͂��Ȃ����A��Ǝ��ԏ㋖�e����ꍇ������
     */
    private void Awake()
    {
        // FPS��ݒ�
        SetFps();

        // ���b�Z�[�W�����̂��߂̃p�����[�^��������
        param.Init();

        // �V���A���ʐM�X�N���v�g�����Q�[���I�u�W�F�N�g�𐶐�
        sensorDevice.Create();

        // Manager�N���X���p�������N���X�����Q�[���I�u�W�F�N�g����Manager�N���X���擾
        manager = managerObject.GetComponent<Manager>();

        // Manager�N���X���p�������N���X��Awake�֐������s
        manager.BaseAwake(ref param);
    }

    void Start()
    {
        // Manager�N���X���p�������N���X��Start�֐������s
        manager.BaseStart(ref param);
    }

    void Update()
    {
        // Manager�N���X���p�������N���X��Update�֐������s
        manager.BaseUpdate(ref param);
    }

    // �V�[�����؂�ւ��ۂɌĂяo�����֐�
    private void Exit()
    {
        // Manager�N���X���p�������N���X��Exit�֐������s
        manager.BaseExit(ref param);
    }

    // �e�V�[���ŕK����x���s����BFPS�̐ݒ���s���B
    public int SetFps()
    {
        return Constants.MSG_NULL;
    }

    // param�Ɋi�[���ꂽ�V�[���������ɁA�V�[����ύX����B
    public int ChangeScene()
    {
        return Constants.MSG_NULL;
    }
}
