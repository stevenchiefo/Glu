using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private float m_FramesPerSecond;
    [SerializeField] private List<Sprite> m_Sprites;
    private SpriteRenderer m_SpriteRenderer;
    private int m_Index;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / m_FramesPerSecond);
            int futureindex = m_Index + 1;
            if (futureindex >= m_Sprites.Count)
            {
                m_Index = 0;
            }
            else
            {
                m_Index = futureindex;
            }
            m_SpriteRenderer.sprite = m_Sprites[m_Index];
        }
    }
}