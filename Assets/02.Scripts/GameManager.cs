using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("°è´Ü")]
    [Space(10)]
    public GameObject[] Stairs;
    public bool[] isTurn;

    private enum  State {Start, Left, Right};
    private State state;
    private Vector3 startPostion = new(0.75f, -0.1f, 0);
    private Vector3 oldPosition;
    [Header("UI")]
    [Space(10)]
    public GameObject UI_GameOver;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textCurrentScore; 
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;
    private int currentScore = 0;
    [Header("Audio")]
    [Space(10)]
    private AudioSource sound;
    public AudioClip bgmSound;
    public AudioClip dieSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        sound = GetComponent<AudioSource>();

        Init();
        InitStairs();
    }

    public void Init()
    {  
        state = State.Start;
        oldPosition = Vector3.zero;

        isTurn = new bool[Stairs.Length];
        for(int i = 0; i < Stairs.Length; i++)
        {
            Stairs[i].transform.position = Vector3.zero;
            isTurn[i] = false;
        }
        currentScore = 0; ;

        textShowScore.text = currentScore.ToString();

        UI_GameOver.SetActive(false);

        sound.clip = bgmSound;
        sound.Play();
        sound.loop = true;
        sound.volume = 0.4f;
    }
    public void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch(state) { 
                case State.Start:
                    Stairs[i].transform.position = startPostion; 
                    state = State.Right;
                    break;
                 case State.Left:
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                    isTurn[i] = true;
                    break;
                case State.Right:
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                    isTurn[i] = false;
                    break;
            }
            oldPosition = Stairs[i].transform.position;
            if(i != 0)
            {
                int ran = UnityEngine.Random.Range(0, 5);
                if (ran < 2 && i < Stairs.Length - 1) { 
                    state = state == State.Left ? State.Right : State.Left;
                }
            }
        }
        
    }

    public void SpawnStair(int cnt)
    {
        int ran = UnityEngine.Random.Range(0, 5);
        if (ran < 2)
        {
            state = state == State.Left ? State.Right : State.Left;
        }
        switch (state)
        {
            case State.Left:
                Stairs[cnt].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                isTurn[cnt] = true;
                break;
            case State.Right:
                Stairs[cnt].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                isTurn[cnt] = false;
                break;
        }
        oldPosition = Stairs[cnt].transform.position;
    }

    public void GameOver()
    {
        sound.loop = false;
        sound.Stop();
        sound.clip = dieSound;
        sound.volume = 1.0f;
        sound.Play();
        StartCoroutine(ShowGameOver()); 
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);

        UI_GameOver.SetActive(true);

        if(currentScore > maxScore)
        {
            maxScore  = currentScore;
        }

        textMaxScore.text = maxScore.ToString();
        textCurrentScore.text = currentScore.ToString();
    }

    public void AddScore()
    {
        currentScore++;
        textShowScore.text = currentScore.ToString();
    }
}
