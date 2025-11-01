using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSystem : MonoBehaviour
{
    [SerializeField] BallGenerator ballGenerator = default;
    bool isDragging;
    [SerializeField] List<Ball> removeBalls = new List <Ball>();
    Ball currentDraggingBall;
    int score;
    [SerializeField] Text scoreText = default;
    [SerializeField] GameObject pointEffectPrefab = default;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Main);
        UpdateScore(0);
        StartCoroutine(ballGenerator.Spawns(70));
    }

    void UpdateScore(int point)
    {
        score = point;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //右クリック押し込んだ時
            OnDragBegin();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            //右クリックを離した時
            OnDragEnd();

        }
        else if (isDragging)
        {
            OnDrangging();
        }

    }

    void OnDragBegin()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Ball ball = hit.collider.GetComponent<Ball>();
            if(ball.IsBomb())
            {   
                Explosion(ball);
            }
            else
            {
                AddRemoveBall(ball);
                isDragging = true;
            }
        }
    }

    void OnDrangging()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Debug.Log("ヒット中！！");
            Ball ball = hit.collider.GetComponent<Ball>();
            //removeBalls.Add(ball);
            if(ball.id == currentDraggingBall.id)
            {
                float distance = Vector2.Distance(ball.transform.position,currentDraggingBall.transform.position);
                if(distance < 1.5)
                {
                    AddRemoveBall(ball);
                }
            }
        }
    }

    void OnDragEnd()
    {
        int removeCount = removeBalls.Count;
        if(removeCount >= 3)
        {
            for ( int i = 0; i < removeCount; i++)
            {
                removeBalls[i].Explosion();
            }
            StartCoroutine(ballGenerator.Spawns(removeCount));
            score += removeCount * 100;
            UpdateScore(score);
            SpawnPointEffect(removeBalls[removeBalls.Count-1].transform.position, score);
            SoundManager.instance.PlaySE(SoundManager.SE.Destroy);

            

        }

        for (int i=0; i < removeCount;i++)
        {
            removeBalls[i].transform.localScale = Vector3.one;
            removeBalls[i].GetComponent<SpriteRenderer>().color = Color.white;

        }
        
        removeBalls.Clear();
        Debug.Log("ドラッグ終了！！");
        isDragging = false;

    }

    void AddRemoveBall(Ball ball)
    {
        currentDraggingBall = ball;
        if (removeBalls.Contains(ball) == false)
        {
            ball.transform.localScale = Vector3.one * 1.4f;
            ball.GetComponent<SpriteRenderer>().color = Color.yellow;
            removeBalls.Add(ball);
            SoundManager.instance.PlaySE(SoundManager.SE.Touch);


        }
    }

    void Explosion(Ball bomb)
    {
        List<Ball> explosionList = new List<Ball>();
        //ボムを中心に爆破するBallを集める
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(bomb.transform.position, 1.5f);

        for(int i=0; i < hitObj.Length; i++)
        {
            //Ballなら爆破リストに追加
            Ball ball = hitObj[i].GetComponent<Ball>();
            if(ball)
            {
                explosionList.Add(ball);
            }
        }
        int removeCount = explosionList.Count;
        for ( int i = 0; i < removeCount; i++)
        {
            explosionList[i].Explosion();
        }
        StartCoroutine(ballGenerator.Spawns(removeCount));
        score += removeCount * 1500;
        UpdateScore(score);
        SpawnPointEffect(bomb.transform.position, score);
        SoundManager.instance.PlaySE(SoundManager.SE.Destroy);
    }

    void SpawnPointEffect(Vector2 position,int score)
    {
        Instantiate(pointEffectPrefab, position, Quaternion.identity);
    }


    
}
