using System.Data;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }
    //���������������͵ı������ֱ��ʾ�����е��к͵�Ԫ��

    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;
    //�ֱ��ȡ����ķ�����������������

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
        //��ȡ��������������Ӷ���ǰ�洴������Ҳ��Ϊ�����Ŀ��
    }

    private void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }
    //�� Start �׶Σ�����������������ÿ����Ԫ������꣬ȷ�����������е�Ԫ��֪���Լ���λ�ã�Ҳ���Ǹ�ÿ����Ԫ��λ

    #region ���庯������

    /// <summary>
    /// �� TileGrid ���ҵ�һ�������δ��ռ�õĵ�Ԫ���������������ֿ顣������е�Ԫ���ѱ�ռ�ã��򷵻� null����ʾ�޷��������µĿ顣
    /// </summary>
    /// <returns></returns>
    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingindex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            // all cells are occupied
            if (index == startingindex)
            { return null; }
        }

        return cells[index];
    //�����������ѡȡһ����Ԫ�񣬲�������Ƿ�ռ��,�����ռ�ã�����������һ����Ԫ��ѭ������������������������񶼱�ռ�ã����� null������ҵ�δ��ռ�õĵ�Ԫ�񣬷��ظõ�Ԫ��
    }


    /// <summary>
    /// GetCell ��������ҪĿ���Ǹ��ݸ��������� (x, y) ���ض�Ӧ�� TileCell ����
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TileCell GetCell(int x, int y) 
    //һ��Ҫע�⣬����������ص��� TileCell ���󣬶����� Tile ����Ҳ����TileCell�����ռ�ã�occupied������
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return (rows[y].cells[x]);
            //������λ�ڵ� y �С��� x �е� TileCell ����
        }
        else
        { return null; }
    }

    /// <summary>
    /// ������GetCell��������Vector2Int�������ò�����ʾ��Ԫ������꣬Ȼ�����֮ǰ��GetCell����
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }


    /// <summary>
    /// ���ݸ����� TileCell �����һ���������������ظõ�Ԫ����ָ�������ϵ����ڵ�Ԫ��
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    #endregion

}
