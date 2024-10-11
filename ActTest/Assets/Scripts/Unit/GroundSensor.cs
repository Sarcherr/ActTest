using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ش�����
/// </summary>
public class GroundSensor : MonoBehaviour
{
    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    /// <summary>
    /// ���ؽӴ�����״̬(trueΪ����)
    /// </summary>
    /// <returns></returns>
    public bool State()
    {
        if (m_DisableTimer > 0)
        {
            return false;
        }
        return m_ColCount > 0;
    }

    /// <summary>
    /// ��ʱ���õ��洫����(����Ϊ����ʱ��)
    /// </summary>
    /// <param name="duration"></param>
    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_ColCount++;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_ColCount--;
        }
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }
}
