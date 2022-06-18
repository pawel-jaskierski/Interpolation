using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math : MonoBehaviour
{
    public class  Link{
        public float value;
        public Link(float startingUpperConnection, float startingLowerConnection, 
                          Link leftConnection, Link downConnection){ 
            value = (downConnection.value-leftConnection.value)/(startingLowerConnection-startingUpperConnection);
        }
        public Link(float value){
            this.value = value; 
        }
    }
    public List<Transform> Points;
    public List<Link> Links;
    public LineRenderer lineRenderer;
    public int numberOfPoints=200;
    public List<Vector3> pointsToRender;

    [ContextMenu("Generate")]
    void Update()
    {
        Links = new List<Link>();
        pointsToRender = new List<Vector3>();
        GenertareLinks();
        GeneratePoint();
        GenerateLines();
    }

    void GenertareLinks(){
        int j = 1;
        int k = 0;
        for(int i = 0; i < Points.Count; i++){
            Links.Add(new Link(Points[i].position.y));
        }
        for(int i = 0; i < Points.Count/2f*(Points.Count-1); i++){
            Links.Add(new Link(Points[k].position.x, Points[k+j].position.x, Links[i+j-1], Links[i+j]));
            k++;
            if(k == Points.Count-j){
                j++;
                k = 0;
            }
        }

    }
    void GeneratePoint(){
        float startingPoint = Points[0].position.x;
        float endPoint = Points[Points.Count-1].position.x;
        float step = Mathf.Abs(endPoint-startingPoint)/numberOfPoints;
        float currentXValue = startingPoint;
        float partialSum = 1;
        float sum = Links[0].value;
        int current = Points.Count;
        for(int i = 0; i < numberOfPoints+1; i++){
            for(int j = 1; j <Points.Count; j++){
                for(int k = 0; k < j; k++){
                    partialSum *= currentXValue-Points[k].position.x;
                }
                partialSum *=  Links[current].value;
                current += Points.Count-j;
                sum += partialSum;
                partialSum = 1;
                
            }
            current = Points.Count;
            pointsToRender.Add(new Vector3(currentXValue,sum,0));
            sum = Links[0].value;
            currentXValue += step;
        }
    } 
    void GenerateLines(){
        lineRenderer.positionCount = numberOfPoints + 1;
        lineRenderer.SetPositions(pointsToRender.ToArray());
    }
}
