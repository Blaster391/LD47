using UnityEngine;
using System.Collections;

public class SlidingAverageBuffer
{
    public SlidingAverageBuffer(uint bSize)
    {
        m_data = new Vector3[bSize];
        m_bSize = bSize;
    }

    public void PushValue(Vector3 i_val)
    {
        m_data[m_ind] = i_val;

        //cringe hack
        if (m_firstFillCount < m_bSize)
        {
            ++m_firstFillCount;
        }

        ++m_ind;
        if (m_ind == m_bSize)
        {
            m_ind = 0;
        }
    }

    public Vector3 GetAverage()
    {
        Vector3 total = Vector3.zero;
        for(uint i = 0; i < m_firstFillCount; ++i)
        {
            total += m_data[i];
        }

        return total / (float)m_firstFillCount;
    }

    uint m_firstFillCount = 0;
    Vector3[] m_data;
    uint m_bSize;
    uint m_ind = 0;
}
