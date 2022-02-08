using UnityEngine;

public class CameraTrackBounds : MonoBehaviour
{
    [SerializeField]
    private CellGridGOL_CPU _TrackTarget;

    void LateUpdate()
    {
        BoundsInt targetBounds = _TrackTarget.simulationBounds;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = (float)targetBounds.size.x / (float)targetBounds.size.y;

        if (screenRatio > targetRatio)
        {
            Camera.main.orthographicSize = targetBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
        }

        transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1);
    }
}
