using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairShipChest : MonoBehaviour
{
    [SerializeField] private GameObject m_UI;
    [SerializeField] private Button m_Button;
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        m_UI.transform.LookAt(m_Camera.transform);
    }

    public void OnRepair()
    {
        bool _booleanDur = PlayerShip.Instance.Durrability != DataBase.Instance.GetData().ShipData.MaxDurrabilty;
        if (_booleanDur == false || (Player.Instance.GetPlayerStats().Gold >= 20) == false || Player.Instance.IsOnShip())
        {
            return;
        }
        Player.Instance.RemoveGold(20);
        PlayerShip.Instance.Durrability += 20;
        PlayerShipUI.Instance.UpdateUI();
        PlayerInterfaceUI.Instance.UpdateUI();
    }
}