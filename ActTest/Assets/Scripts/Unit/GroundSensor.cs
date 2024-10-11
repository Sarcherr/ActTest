using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触地传感器
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
    /// 返回接触地面状态(true为触地)
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
    /// 暂时禁用地面传感器(参数为禁用时长)
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
