using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create new action")]
public class NewAction : ScriptableObject
{
    [SerializeField] string animationName;
    [SerializeField] float minimumHeight;
    [SerializeField] float maximumHeight;
    [SerializeField] bool lookatObstacle;
    [SerializeField] bool allowTargetMatching = true;
    [SerializeField] AvatarTarget compareBodyPart;
    [SerializeField] float compareStartTime;
    [SerializeField] float compareEndTime;
    [SerializeField] Vector3 comparePositionWeight = new Vector3(0, 1, 0);
    public Quaternion RequireRotation { get; set; }

    public Vector3 ComparePosition { get; set; }

    public bool CheckIfAvailable(ObstacleInfo hitData, Transform player)
    {
        float checkHeight = hitData.heightInfo.point.y - player.position.y;
        if (checkHeight < minimumHeight || checkHeight > maximumHeight)
            return false;
        if (lookatObstacle)
        {
            RequireRotation = Quaternion.LookRotation(-hitData.hitInfo.normal);
        }
        if (allowTargetMatching)
        {
            ComparePosition = hitData.heightInfo.point;
        }
        return true;
    }
    public string AnimationName => animationName;
    public bool LookAtObstacle => lookatObstacle;

    public bool AllowTargetMatching => allowTargetMatching;
    public AvatarTarget CompareBodyPart => compareBodyPart;
    public float CompareStartTime => compareStartTime;
    public float CompareEndTime => compareEndTime;
    public Vector3 ComparePositionWeight => comparePositionWeight;
}
