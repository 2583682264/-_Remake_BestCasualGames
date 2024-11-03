using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScore;

    private int score = 0;

    private void Start()
    {
        NewGame();
    }

    /// <summary>
    /// ��ʼ����Ϸ
    /// </summary>
    public void NewGame()
    {
        SetScore(0);
        highScore.text = LoadHighScore().ToString();

        gameOver.alpha = 0;
        gameOver.interactable = false;

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1, 1f));
    }   

    /// <summary>
    /// ��Ϸ�����������������Բ�ֵ��lerping�������𽥸ı�CanvasGroup��alphaֵ��ʵ�ֵ��뵭��Ч��
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="to"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        // �ڿ�ʼִ�е��뵭��Ч��֮ǰ�ȴ��ض�ʱ��
        yield return new WaitForSeconds(delay);

        // ��ʼ������
        float elapsed = 0f;          // ��¼�Ѿ���ȥ��ʱ��
        float duration = 0.5f;       // �������ܳ���ʱ�䣬Ϊ0.5��
        float from = canvasGroup.alpha; // ������ʼʱCanvasGroup�ĳ�ʼalphaֵ

        // ʹ��whileѭ���𽥸ı�alphaֵ
        while (elapsed < duration)
        {
            // ʹ�����Բ�ֵ��lerping���������㵱ǰ��alphaֵ
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);

            // �����Ѿ���ȥ��ʱ�䣨��������֡����֡���ӣ�
            elapsed += Time.deltaTime;

            // ��ͣЭ�̵�ִ�У��ȴ���һ֡����
            yield return null;
        }

        // ȷ����ѭ��������alphaֵ׼ȷ�趨ΪĿ��ֵ
        canvasGroup.alpha = to;
    }


    /// <summary>
    /// ͨ������������������ӷ���
    /// </summary>
    /// <param name="points"></param>
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }


    /// <summary>
    /// ͨ���������������������ʾ��������������߷�
    /// </summary>
    /// <param name="score"></param>
    private void SetScore(int score)
    {
        this.score = score;

        scoreText.text = score.ToString();

        SaveHighScore();
    }


    /// <summary>
    /// ������߷�
    /// </summary>
    private void SaveHighScore()
    {
        int highScore = LoadHighScore();

        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }


    /// <summary>
    /// ������߷�
    /// </summary>
    /// <returns></returns>
    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }

}
