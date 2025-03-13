using UnityEngine;
using System.Collections;

public class BoardPath : MonoBehaviour
{
    [SerializeField] private GameObject yellowwaypointgameobject;
    [SerializeField] private GameObject greenwaypointgameobject;
    [SerializeField] private GameObject redwaypointgameobject;
    [SerializeField] private GameObject bluewaypointgameobject;


    [SerializeField] private Transform[] yellowwaypoints;
    [SerializeField] private Transform[] greenwaypoints;
    [SerializeField] private Transform[] bluewaypoints;
    [SerializeField] private Transform[] redwaypoints;

    private void Start()
    {
        waypointadd();
    }

    void waypointadd()
    {
        if (yellowwaypointgameobject != null)
        {
            int i = 0;
            int childcount = yellowwaypointgameobject.transform.childCount;
            yellowwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in yellowwaypointgameobject.transform)
            {
                yellowwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (greenwaypointgameobject != null)
        {
            int i = 0;
            int childcount = greenwaypointgameobject.transform.childCount;
            greenwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in greenwaypointgameobject.transform)
            {
                greenwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (bluewaypointgameobject != null)
        {
            int i = 0;
            int childcount = bluewaypointgameobject.transform.childCount;
            bluewaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in bluewaypointgameobject.transform)
            {
                bluewaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (redwaypointgameobject != null)
        {
            int i = 0;
            int childcount = redwaypointgameobject.transform.childCount;
            redwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in redwaypointgameobject.transform)
            {
                redwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
    }
    public bool CanMove(int currentPosition, int steps, int colornum)
    {
        // Implement your game's specific movement rules


        if (colornum == 0)
        {
            return (currentPosition + steps) < greenwaypoints.Length;
        }
        else if (colornum == 1)
        {
            return (currentPosition + steps) < yellowwaypoints.Length;
        }
        else if (colornum == 2)
        {
            return (currentPosition + steps) < bluewaypoints.Length;
        }
        else
        {
            return (currentPosition + steps) < redwaypoints.Length;

        }
    }

    public IEnumerator MovePieceAlongPath(Piece piece, int startPos, int endPos, float duration)
    {
        for (int i = startPos + 1; i <= endPos; i++)
        {
            if(piece.colornum == 0)
            {
                yield return MoveToWaypoint(piece.transform, greenwaypoints[i].position, duration / (endPos - startPos));
            }
            else if(piece.colornum == 1)
            {
                yield return MoveToWaypoint(piece.transform, yellowwaypoints[i].position, duration / (endPos - startPos));
            }
            else if(piece.colornum == 2)
            {
                yield return MoveToWaypoint(piece.transform, bluewaypoints[i].position, duration / (endPos - startPos));
            }
            else
            {
                 yield return MoveToWaypoint(piece.transform, redwaypoints[i].position, duration / (endPos - startPos));

            }
        }
    }

    public IEnumerator MovePieceToStart(Piece piece, float duration)
    {
        if (piece.colornum == 0)
        {
            yield return MoveToWaypoint(piece.transform, greenwaypoints[0].position, duration);
        }
        else if (piece.colornum == 1)
        {
            yield return MoveToWaypoint(piece.transform, yellowwaypoints[0].position, duration);
        }
        else if (piece.colornum == 2)
        {
             yield return MoveToWaypoint(piece.transform, bluewaypoints[0].position, duration);
        }
        else
        {
            yield return MoveToWaypoint(piece.transform, redwaypoints[0].position, duration);

        }
      
    }

    IEnumerator MoveToWaypoint(Transform pieceTransform, Vector3 target, float moveTime)
    {
        Vector3 startPos = pieceTransform.position;
        float elapsed = 0;

        while (elapsed < moveTime)
        {
            pieceTransform.position = Vector3.Lerp(startPos, target, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pieceTransform.position = target;
    }
}