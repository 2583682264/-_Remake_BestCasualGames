using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }
    //ʹ��Vector2Int��������ʾ��ά�ռ��е�λ�á�
    public Tile tile { get; set; }
    //����һ��Tile���͵�����tile�����ڴ洢��Ԫ���е�Tile����

    //ʹ�ò���ֵ����ʾ��Ԫ���״̬�����Ƿ�Ϊ�ջ�ռ�á�
    //���tileΪnull�����ʾ��Ԫ��Ϊ�ա�
    //���tile��Ϊnull�����ʾ��Ԫ��ռ�á�
    public bool empty => tile == null;
    public bool occupied => tile != null;
}
    