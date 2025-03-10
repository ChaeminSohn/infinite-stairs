using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPostion;
    private Vector3 oldPosition;
    private bool isTurn = false;
    private int moveCnt = 0;
    private int turnCnt = 0;
    private int spawnCnt = 0;
    private bool isDie = false;

    private AudioSource sound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        startPostion = transform.position;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(1))
        {
            CharTurn();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            CharMove();
        }*/
    }

    private void Init()
    {
        anim.SetBool("isDie", false);
        transform.position = startPostion;
        oldPosition = startPostion;
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = false; 
        isDie = false;
    }
    public void CharTurn()
    {
        isTurn = isTurn == true ? false : true;
        spriteRenderer.flipX = isTurn; ;
    }

    public void CharMove()
    {
       if(isDie == true) 
        {
            return;
        }
        sound.Play();
        MoveDirection();
        moveCnt++;

        if (isFailTurn())
        {
            CharDie();
            return;
        }

        if(moveCnt > 5)
        {
            RespawnStair();
        }

        GameManager.instance.AddScore();
    }

    private void MoveDirection()
    {
        if (isTurn)
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }
        transform.position = oldPosition;
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool result = false;
        if (GameManager.instance.isTurn[turnCnt] != isTurn) 
        {
            result = true;
        }
        turnCnt++;

        if(turnCnt > GameManager.instance.Stairs.Length - 1)
        {
            turnCnt = 0;
        }
        return result;
    }

    private void RespawnStair()
    {
        GameManager.instance.SpawnStair(spawnCnt);
        spawnCnt++;
        if(spawnCnt > GameManager.instance.Stairs.Length - 1) 
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.instance.GameOver();
        anim.SetBool("isDie", true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.instance.Init(); ;
        GameManager.instance.InitStairs(); 
    }
}
