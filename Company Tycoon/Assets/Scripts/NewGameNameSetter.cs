using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameNameSetter : MonoBehaviour
{
    [SerializeField] private InputField m_InputField;

    public void SetName()
    {
        Player.Instance.SetStart(m_InputField.text);
        Destroy(gameObject);
    }
}