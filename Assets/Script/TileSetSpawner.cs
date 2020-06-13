﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSetSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_TileSetArr = null;//인스펙터에서 사용하기 위한 2차원 배열

    [SerializeField]
    private int[] m_TileLimit = null;

    [SerializeField]
    private Text[] m_TileLimitText = null;

    [SerializeField]
    private RectTransform m_ButtonPanel = null;

    [SerializeField]
    private Button[] m_Buttons = null;

    [SerializeField]
    private Button m_RotateButton_RCW = null;

    [SerializeField]
    private Button m_RotateButton_CW = null;

    [SerializeField]
    private Button m_SelectButton = null;

    private GameObject m_NowTileSet = null;

    private int m_LatestTileSetNum = -1;

    private void Awake()
    {
        for(int i = 0; i < m_TileLimit.Length;++i)
        {
            m_TileLimitText[i].text = m_TileLimit[i].ToString();
            if(m_TileLimit[i] <= 0)
            {
                m_Buttons[i].interactable = false;
                m_Buttons[i].gameObject.GetComponent<Image>().color = new Color(100,100,100,255);
            }
        }
    }

    public void SpawnTileSet(int num)
    {
        if(m_NowTileSet == null)
        {
            m_RotateButton_RCW.onClick.RemoveAllListeners();
            m_RotateButton_CW.onClick.RemoveAllListeners();
            m_SelectButton.onClick.RemoveAllListeners();

            m_NowTileSet = Instantiate(m_TileSetArr[num],new Vector3(-9.5f,-4,0),Quaternion.Euler(0,0,0));
            m_ButtonPanel.gameObject.SetActive(true);

            m_RotateButton_RCW.onClick.AddListener(m_NowTileSet.GetComponent<TileSet>().CancelTile);
            m_RotateButton_CW.onClick.AddListener(m_NowTileSet.GetComponent<TileSet>().RotateTileSet_CW);
            m_SelectButton.onClick.AddListener(m_NowTileSet.GetComponent<TileSet>().InstallTile);
            m_SelectButton.onClick.AddListener(ReduceTileLimit);
            m_LatestTileSetNum = num;

        }
    }

    private void LateUpdate()
    {
        if(m_NowTileSet != null)
        {
            Debug.Log(Camera.main.WorldToScreenPoint(m_NowTileSet.transform.position));
            m_ButtonPanel.position = Camera.main.WorldToScreenPoint(m_NowTileSet.transform.position);
        }
        else
        {
            m_ButtonPanel.gameObject.SetActive(false);
        }
    }

    private void ReduceTileLimit()
    {
        m_TileLimitText[m_LatestTileSetNum].text = (--m_TileLimit[m_LatestTileSetNum]).ToString();
        if (m_TileLimit[m_LatestTileSetNum] <= 0)
        {
            m_Buttons[m_LatestTileSetNum].interactable = false;
            m_Buttons[m_LatestTileSetNum].gameObject.GetComponent<Image>().color = new Color(100, 100, 100, 255);
        }
    }

    public int[] GetTileLimits()
    {
        return m_TileLimit;
    }
}
