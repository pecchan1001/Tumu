using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    // Ball生成
    [SerializeField] GameObject ballPrefab = default;

    //画像設定
    [SerializeField] Sprite[] ballSprite = default;

    [SerializeField] Sprite bombSprite = default;



    

    private void Start()
    {
        //StartCoroutine(Spawns(40));

    }


    public IEnumerator Spawns(int count)
    {
        for (int i=0; i< count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-0.2f, 0.2f),8f);
            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);
            int ballID = Random.Range(0, ballSprite.Length);
            
            if(Random.Range(0,100) < 5)
            {
                ballID = -1;
                ball.GetComponent<SpriteRenderer>().sprite = bombSprite;
            }
            else
            {
                ball.GetComponent<SpriteRenderer>().sprite = ballSprite[ballID];
            }

            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.04f);
        }
    }
   
}
