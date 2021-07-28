using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] BallPrefabs;
    private bool isDragged;
    private ArrayList removebleBallList = new ArrayList();
    private CommonBall firstBall = new CommonBall();


    void Start()
    {
        StartCoroutine("DropBall");
    }
    private void onDragStart()
    {
        GameObject targetObject = GetCurrentTarget();
        removebleBallList.Clear();
        if(targetObject)
        {
            firstBall = targetObject.GetComponent<CommonBall>();
            removebleBallList.Add(targetObject);
            firstBall.isAdd = true;
            
        }
    }
    private void onDragging()
    {
        GameObject targetObject = GetCurrentTarget();
        Debug.Log(targetObject.GetInstanceID());
        if(targetObject)
         {
            if(targetObject.name.IndexOf("Ball")!= -1)
            {
                
                CommonBall targetBall = targetObject.transform.GetComponent<CommonBall>();
                if(targetBall.kindOfId == firstBall.kindOfId)
                {    
                    if(targetBall.isAdd == false)
                    {
                        Debug.Log("dragging");
                        removebleBallList.Add(targetObject);
                        targetBall.isAdd = true;
                        

                    }
                }
            }
        }
    }
    private void OnDragEnd()
    {
        firstBall = new CommonBall();
        int length = removebleBallList.Count;
       
        if (length >= 3) 
        {
            foreach(GameObject ball in removebleBallList)
            {
                Destroy(ball);
                
            }
        }
        else
        {
            foreach(GameObject ball in removebleBallList)
            {
                ball.transform.GetComponent<CommonBall>().isAdd = false;
            }
        }

    }

    private IEnumerator DropBall()
    {
        for (int i = 0; i < 50;i++)
        {
            int RANDOM_INDEX = Random.Range(0, BallPrefabs.Length);
            float RANDOM_X = Random.Range(-2.0f, 2.0f);
            Vector3 BALL_INITIAL_POSITION = new Vector3(RANDOM_X, 7.0f, -0.0f);
            GameObject cloneBall = Instantiate(BallPrefabs[RANDOM_INDEX]);
           
            cloneBall.transform.position = BALL_INITIAL_POSITION;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&& isDragged == false)
        {
            isDragged = true;
            onDragStart();
        }
        else if (Input.GetMouseButton(0)&& isDragged ==true)
        {
            onDragging();
        }
        else
        {
            isDragged = false;
            OnDragEnd();
        }
    }
   private GameObject GetCurrentTarget()
   {
        GameObject atDeleteTarget = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        if (hit2d)
        {
            atDeleteTarget = hit2d.transform.gameObject;
        }

        return atDeleteTarget;
    }
}
