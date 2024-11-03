using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;
    //���ڴ洢 Tile �����Ԥ����� Tile ״̬����

    public GameManager gameManager;
    private TileGrid grid;
    private List<Tile> tiles;

    private bool waiting;

    //���������������� Tile����CreateTile()�����Ĳ�������
    private List<TileCell> lastGeneratedCells = new List<TileCell>();
    private const int maxLastGeneratedCells = 3;
    private const float probabilityOfTwo = 0.9f;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
        //���ڻ�ȡ TileGrid �����������һ���յ� Tile �����б������ 16 �� TileGrid �е�Ԫ�������
    }


    private void Update()
    {
        if (!waiting)
        //Ŀ���Ƿ�ֹ����� Tile �ƶ��������֮ǰ�ٴΰ����ƶ���
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }


    #region ���庯������

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                changed |= MoveTile(cell.tile, direction);
                //MoveTile�ȱ����ã���� Tile ����ɹ��ƶ����򷵻� true�����򷵻� false
                //��˼�ǵ�����һ����������ƶ��ˣ�changed �ͻᱻ����Ϊ true���ͻ���ú�����Э��
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    //���ز���ֵ����ʾ Tile �����Ƿ�ɹ��ƶ�
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        //����Ϊ�˰�ȫ�ԵĿ��ǣ���� adjacent Ϊ null����ֱ������ѭ��
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        //���Ӧ��ѭ��������ִ�У���� newCell ��Ϊ null�����ʾ�ҵ���һ���յ�Ԫ�񣬿����ƶ� Tile ����
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ����������� Tile ����
    /// </summary>
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        // ʹ���������������2����4
        int value = UnityEngine.Random.value < probabilityOfTwo ? 2 : 4;
        tile.SetState(tileStates[value == 2 ? 0 : 1], value);

        // ��ȡ����յ�Ԫ��
        TileCell emptyCell = GetRandomEmptyCellExcludingLast();

        // ���û�пյ�Ԫ��ֱ�ӷ���
        if (emptyCell == null)
        {
            Destroy(tile.gameObject);
            return;
        }

        // ����tile
        tile.Spawn(emptyCell);

        // ����������ɵ�λ��
        lastGeneratedCells.Add(emptyCell);
        if (lastGeneratedCells.Count > maxLastGeneratedCells)
        {
            lastGeneratedCells.RemoveAt(0);
        }

        tiles.Add(tile);
    }
    private TileCell GetRandomEmptyCellExcludingLast()
    {
        TileCell emptyCell;
        int attempts = 0;
        int maxAttempts = grid.size; // ���� grid.size ���ܵĵ�Ԫ������

        do
        {
            emptyCell = grid.GetRandomEmptyCell();
            attempts++;

            // ������Դ����������ֵ�����������ɵĵ�Ԫ���б�
            if (attempts > maxAttempts)
            {
                lastGeneratedCells.Clear();
                return emptyCell;
            }
        }
        while (emptyCell != null && lastGeneratedCells.Contains(emptyCell));

        return emptyCell;
    }
    ///// <summary>
    ///// �� TileGrid �������һ���յ�Ԫ���д���һ���µ� Tile ����
    ///// </summary>
    //public void CreateTile()
    //{
    //    Tile tile = Instantiate(tilePrefab, grid.transform);
    //    tile.SetState(tileStates[0], 2);
    //    tile.Spawn(grid.GetRandomEmptyCell());
    //    //����ʵ����һ�� Tile ���󣬲�������ӵ� TileGrid �������һ���յ�Ԫ����, ��������״̬Ϊ tileStates �����еĵ�һ��Ԫ��(����������ɫ���ı���ɫ), �Լ�����2

    //    tiles.Add(tile);
    //    //Ŀ���ǽ��´����� Tile ������ӵ� tiles �б��У��Ա��ܹ�����ع������ٺͲ�����Щ Tile ����
    //}


    /// <summary>
    /// �ж����� Tile �����Ƿ���Ժϲ���������ֹһ�ºϲ����Tile��������
    /// </summary>
    /// <param name="tile1"></param>
    /// <param name="tile2"></param>
    /// <returns></returns>
    private bool CanMerge(Tile tile1, Tile tile2)
    {
        return tile1.state == tile2.state && !tile2.locked;
        //���� Tile �����״̬��ͬ�� tile2 ����û�б�����ʱ�ſ��Ժϲ�,��Ȼ�����һ���Ӻϲ���� Tile ��������

        //return tile1.state == tile2.state && tile1.number < 2048 && !tile2.locked;
        //�������Ӳ���룬��С��2048�� Tile ����ſ��Ժϲ�
    }


    /// <summary>
    /// �ϲ����� Tile ���󣬲���Ҫ����Tile����� Merge ����
    /// </summary>
    /// <param name="tile1"></param>
    /// <param name="tile2"></param>
    private void Merge(Tile tile1, Tile tile2)
    {
        tiles.Remove(tile1);
        tile1.Merge(tile2.cell);

        int index = Mathf.Clamp(IndexOf(tile2.state) + 1, 0, tileStates.Length - 1);
        int number = tile2.number * 2;

        tile2.SetState(tileStates[index], number);

        gameManager.IncreaseScore(number);//����������Ϸ�÷�
    }

    /// <summary>
    /// ���� TileState ö�����ͷ��ض�Ӧ������ֵ�����ڲ�ѯ������ Tile �����״̬
    /// </summary>
    /// <param name="state"></param>
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tileStates[i] == state)
            {
                return i;
            }
        }

        return -1;
        //����-1����ʾû���ҵ���Ӧ�� TileState ö�����ͣ�������Ϊ�˷�ֹ����δ������쳣��һ���ò���
    }


    /// <summary>
    /// ���Э�����ڵȴ�һ��ʱ�䣬��ֹ�� Tile �ƶ��������֮ǰ�����ƶ�������б���ƶ���ͬʱ�����ж���Ϸ�Ƿ�������Ƿ���Ҫ�����µ� Tile
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;

        if(tiles.Count != grid.size)
        {
            CreateTile();
        }

        foreach (var tile in tiles)
        {
            tile.locked = false;
            //���ڽ��� Tile ����ʹ����Ժϲ�
        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    /// <summary>
    /// �����Ϸ�Ƿ������������е�Ԫ�񶼱�����������û���������ڵĵ�Ԫ�������ͬ��״̬������Ϸ����
    /// </summary>
    /// <returns></returns>
    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)){
                return false;
            }


            if (down != null && CanMerge(tile, down.tile)){
                return false;
            }


            if (left != null && CanMerge(tile, left.tile)){
                return false;
            }


            if (right != null && CanMerge(tile, right.tile)){
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// �������������� tiles ���󣬲���� tiles �б� ��������������Cell����
    /// </summary>
    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
        //��� Tile ����ļ���
    }


    #endregion
}