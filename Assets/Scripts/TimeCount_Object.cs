using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCount_Object : MonoBehaviour
{
    private TextMeshPro timerText;
    public float countdownMinutes = 10.0f;
    private float countdownSeconds;

    private void Start()
    {
        // "Time"�Ƃ������O�̃Q�[���I�u�W�F�N�g��T���ATextMeshProUGUI �R���|�[�l���g���擾
        timerText = GameObject.Find("Time_Object")?.GetComponent<TextMeshPro>();

        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI �R���|�[�l���g��������܂���ł����B");
        }

        // �J�E���g�_�E���̏����b����ݒ�
        countdownSeconds = countdownMinutes * 60;
    }

    void Update()
    {
        if (countdownSeconds > 0)
        {
            // �^�C�}�[���X�V
            countdownSeconds -= Time.deltaTime;

            // ���Ԃ� TimeSpan �ɕϊ����ĕ\��
            var span = new TimeSpan(0, 0, Mathf.CeilToInt(countdownSeconds));
            timerText.text = span.ToString(@"mm\:ss");
        }
        else
        {
            // �^�C�}�[��0�ɂȂ����ꍇ�̏���
            countdownSeconds = 0;
            timerText.text = "00:00";
            // 0�b�ɂȂ����Ƃ��̒ǉ������������ɏ���
        }
    }
}
