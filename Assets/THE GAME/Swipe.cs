using UnityEngine;
using DG.Tweening;

public class Swipe : MonoBehaviour
{
    public GameObject player;  // The player object
    public TrailRenderer trailRenderer;  // The TrailRenderer component
    public float trailTime = 0.5f;  // Duration of the trail
    public float arcAngle = 90f;  // The angle of the arc
    public float arcRadius = 5f;  // The radius of the arc
    public int resolution = 10;  // Number of points to define the arc
    public float duration = 1f;  // Duration of the swipe animation

    private Vector3[] arcPoints;

    void Start()
    {
        trailRenderer.time = trailTime;

        // Calculate arc points
        arcPoints = new Vector3[resolution + 1];
        Vector3 direction = Vector3.forward;
        Vector3 startDirection = Quaternion.Euler(0, -arcAngle * 0.5f, 0) * direction;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 pointDirection = Quaternion.Euler(0, arcAngle * t, 0) * startDirection;
            arcPoints[i] = player.transform.position + pointDirection * arcRadius;
        }
    }

    // Called to start the swipe    
    public void SwipeStart()
    {
        transform.localPosition = arcPoints[0]; 
        // Begin the swipe along the arc
        transform.DOLocalPath(arcPoints, duration, PathType.CatmullRom).SetLookAt(0.01f).OnStart(() =>
        {
            trailRenderer.enabled = true;  // Enable the trail at the start of the swipe
        }).OnComplete(() =>
        {
            trailRenderer.enabled = false;  // Disable the trail at the end of the swipe
        });
    }
}