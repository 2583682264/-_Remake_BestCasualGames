using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; private set; }
    public bool locked { get; set; }
    //�����������locked�����ڱ�ʾ Tile �����Ƿ���������ֹ����Ϸ�����б������ϲ�

    private Image background;
    private TextMeshProUGUI text;


    private void Awake()
    { 
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    #region ���庯��

    /// <summary>
    /// Ŀ����Ϊ�����û���ת�� Tile ����ı�����ɫ���ı���ɫ����ʾ������
    /// </summary>
    /// <param name="state"></param>
    /// <param name="number"></param>
    public void SetState(TileState state, int number)
    //����������������TileState �����˵�Ԫ��ı�����ɫ���ı���ɫ���� number ������ʾ�ڵ�Ԫ���ϵ�����
    {
        this.state = state;
        this.number = number;
        //������Ĳ�����ֵ��Tile����(Ҳ����this����state��number����

        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
        //ע�����ToString()������Ҫ������ת��Ϊ�ַ������Ա���UI����ʾ
    }


    public void Spawn(TileCell cell)
    //Spawn ��������Ҫ�����Ǵ��� Tile ������ TileCell ����֮��Ĺ�����λ��ͬ����������ɵĹ����������µĹ��������� Tile ��λ�ø���Ϊ���µ� TileCell ��ͬ
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        //��鵱ǰ Tile �Ƿ��Ѿ���������ĳ�� TileCell;����Ѵ��ڹ������� cell ��Ϊ null��,����ù���
        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }


    /// <summary>
    /// ��Tile�����ƶ���ָ����TileCell������
    /// </summary>
    /// <param name="cell"></param>
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        //transform.position = cell.transform.position;��ν����ڵ��ԣ�ʵ����Ϸ����Ҫ����Ч��������Ҳ����ʵ���ƶ��߼�

        StartCoroutine(Animate(cell.transform.position));
    //����ƶ����߼���Spawn����һģһ�������ǻ��һ������Ч����ԭ���Ǹ���cell��λ������λ
    }


    /// <summary>
    /// ƽ�������ƶ�������MoveTo������Merge��������
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    private IEnumerator Animate(Vector3 to, bool merging = false)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;
        //��¼ Tile ����ʼλ��

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
            //yield return null ��Э�̵ȴ�����һ֡�ټ���ִ��
            //ʹ�����ܹ��ֲ��ڶ��֡�ϣ�����ƽ�����Ӿ�Ч������Ȼ��������һ֡�����
        }

        transform.position = to;
    //ͨ���ڶ�ʱ���ڣ�0.1���ڣ����С�����ƶ� Tile�������ƽ���ƶ����Ӿ�Ч��
    //ʹ�� Vector3.Lerp ȷ���ƶ��ٶ��ȿ����������һ����Ȼ�ĸо�

        if (merging)
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// ���� Tile ����ĺϲ��������һᴥ������Ч��
    /// </summary>
    /// <param name="cell"></param>
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        //�� Tile ����� cell ��������Ϊ null
        cell.tile.locked = true;
        //����Ŀ�� TileCell ����� tile ��������Ϊ locked���Է�ֹ�����ٴκϲ�

        StartCoroutine(Animate(cell.transform.position, true));
    }

    #endregion
}
