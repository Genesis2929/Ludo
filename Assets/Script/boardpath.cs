using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class BoardPath : MonoBehaviour
{
    [SerializeField] private GameObject yellowwaypointgameobject;
    [SerializeField] private GameObject greenwaypointgameobject;
    [SerializeField] private GameObject redwaypointgameobject;
    [SerializeField] private GameObject bluewaypointgameobject;

    [SerializeField] private GameObject repeatyellowwaypointgameobject;
    [SerializeField] private GameObject repeatgreenwaypointgameobject;
    [SerializeField] private GameObject repeatredwaypointgameobject;
    [SerializeField] private GameObject repeatbluewaypointgameobject;

    [SerializeField] private Transform[] yellowwaypoints;
    [SerializeField] private Transform[] greenwaypoints;
    [SerializeField] private Transform[] bluewaypoints;
    [SerializeField] private Transform[] redwaypoints;

    public soundscript soundscriptobj;

    public List<GameObject> nocuthome = new List<GameObject>();
    private void Start()
    {

        if(optionscript.mustcuttoenterhome)
        {
            waypointadd(true, false, false, false, false);
            nocuthomeenable();
        }
        else
        {
            waypointadd(false, true, true, true, true);
        }
    }
    void nocuthomeenable()
    {
        for(int i = 0; i <4; i++)
        {
            if(LudoDice2D.player1 == i || LudoDice2D.player2 == i || LudoDice2D.player3 == i || LudoDice2D.player4 == i)
            {
                nocuthome[i].SetActive(true);
            }
            else
            {
                nocuthome[i].SetActive(false);
            }

        }
    }
    private void Update()
    {
        if(Piece.greenhavecut)
        {
            waypointadd(false, true, false, false, false);
        }
        if (Piece.yellowhavecut)
        {
            waypointadd(false, false, true, false, false);
        }
        if (Piece.bluehavecut)
        {
            waypointadd(false, false, false, true, false);
        }
        if (Piece.redhavecut)
        {
            waypointadd(false, false, false, false, true);
        }
    }
    void waypointadd(bool changepath, bool green, bool yellow, bool blue, bool red)
    {
        GameObject g1 = null, g2 = null, g3 = null, g4 = null;
        if(changepath)
        {
            g1 = repeatgreenwaypointgameobject; 
            g2 = repeatyellowwaypointgameobject;
            g3 = repeatredwaypointgameobject;
            g4 = repeatbluewaypointgameobject;
        }
        else
        {
            if(green)
            {
                g1 = greenwaypointgameobject;
                nocuthome[0].SetActive(false);
            }
            if(yellow)
            {
                nocuthome[1].SetActive(false);
                g2 = yellowwaypointgameobject;
            }
            if(red)
            {
                g3 = redwaypointgameobject;
                nocuthome[2].SetActive(false);
            }
            if(blue)
            {
                g4 = bluewaypointgameobject;
                nocuthome[3].SetActive(false);
            }
        }
        if (g2 != null)
        {
            int i = 0;
            int childcount = g2.transform.childCount;
            yellowwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in g2.transform)
            {
                yellowwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (g1 != null)
        {
            int i = 0;
            int childcount = g1.transform.childCount;
            greenwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in g1.transform)
            {
                greenwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (g4 != null)
        {
            int i = 0;
            int childcount = g4.transform.childCount;
            bluewaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in g4.transform)
            {
                bluewaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
        if (g3 != null)
        {
            int i = 0;
            int childcount = g3.transform.childCount;
            redwaypoints = new Transform[childcount];
            //waypoints.Length = waypointgameobject.transform.childCount;
            foreach (Transform t in g3.transform)
            {
                redwaypoints[i] = t.gameObject.transform;
                i++;
            }
        }
    }
    public bool CanMove(int currentPosition, int steps, int colornum, Piece ps)
    {
        // Implement your game's specific movement rules

        if(optionscript.mustcuttoenterhome)
        {
            if(ps.colornum == 0)
            {
                if(Piece.greenhavecut == false)
                {
                    return true;
                }
            }
            else if (ps.colornum == 1)
            {
                if (Piece.yellowhavecut == false)
                {
                    return true;
                }
            }
            else if (ps.colornum == 2)
            {
                if (Piece.bluehavecut == false)
                {
                    return true;
                }
            }
            else if (ps.colornum == 3)
            {
                if (Piece.redhavecut == false)
                {
                    return true;
                }
            }
        }
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
        //if(!piece.piecehavecut())
        //{
        //    endPos = endPos % 52;
        //}

        int newcheck = 0;
        for (int i = startPos + 1; i <= endPos; i++)
        {
            soundscriptobj.piecemovesound();
            if(optionscript.mustcuttoenterhome)
            {
                if (!piece.piecehavecut())
                {
                    //endPos = endPos % 52;
                    newcheck = i % 52;

                }
                else
                    newcheck = i;

            }
            else
            {
                newcheck = i;
            }
            if (piece.colornum == 0)
            {
                yield return MoveToWaypoint(piece.transform, greenwaypoints[newcheck].position, duration);
            }
            else if(piece.colornum == 1)
            {
                yield return MoveToWaypoint(piece.transform, yellowwaypoints[newcheck].position, duration);
            }
            else if(piece.colornum == 2)
            {
                yield return MoveToWaypoint(piece.transform, bluewaypoints[newcheck].position, duration);
            }
            else
            {
                 yield return MoveToWaypoint(piece.transform, redwaypoints[newcheck].position, duration);

            }
        }
    }

    public IEnumerator movebacktohome(Piece piece,int startPos, int endPos, float duration)
    {
        Debug.Log("InsideIenumerator::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
        //duration = duration / 10;
        soundscriptobj.piecemovediesound();
        for (int i = startPos - 1; i >= endPos; i--)
        {
        Debug.Log("1InsideIenumerator::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
            piece.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            piece.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;

            soundscriptobj.piecemovesound();
            if (piece.colornum == 0)
            {
                yield return MoveToWaypoint(piece.transform, greenwaypoints[i].position, duration/startPos - endPos);
            }
            else if (piece.colornum == 1)
            {
                yield return MoveToWaypoint(piece.transform, yellowwaypoints[i].position, duration/startPos - endPos);
            }
            else if (piece.colornum == 2)
            {
                yield return MoveToWaypoint(piece.transform, bluewaypoints[i].position, duration / startPos - endPos);
            }
            else
            {
                yield return MoveToWaypoint(piece.transform, redwaypoints[i].position, duration/ startPos - endPos);

            }
        }
        piece.cutpiece(piece.colornum, piece.piecenumber, piece.gameObject);
        piece.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        piece.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;

    }

    public IEnumerator MovePieceToStart(Piece piece, float duration)
    {
        soundscriptobj.piecemovesound();
        duration = duration * 1.5f;
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

    public void movetonewpos(GameObject gm, int newpos, int colornum)
    {
        Vector3 pos = Vector3.zero;
        if(colornum == 0)
        {
            pos = greenwaypoints[newpos].position;
        }
        if (colornum == 1)
        {
            pos = yellowwaypoints[newpos].position;
        }
        if (colornum == 2)
        {
            pos = bluewaypoints[newpos].position;
        }
        if (colornum == 3)
        {
            pos = redwaypoints[newpos].position;
        }

        gm.transform.position = pos;
    }
}