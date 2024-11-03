using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    // ����һ����Ϊcells�Ĺ������飬���ڴ洢�Ӷ���TileCell������
    public TileCell[] cells { get; private set; }


    // ����һ����ΪAwake��˽�з�������������Ϸ��ʼʱ��ʼ��
    private void Awake()
    {
        // ���Ӷ����л�ȡTileCell���͵�����
        cells = GetComponentsInChildren<TileCell>();
    }
}
