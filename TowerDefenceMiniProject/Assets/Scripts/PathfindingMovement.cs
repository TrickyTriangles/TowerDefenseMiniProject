using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMovement : MovementType
{

    [SerializeField] private GameObject MoveTo;
    public MapController master;

    private TileData Location;


    float orientation;
    public float Orientation
    {
        get { return orientation; }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f); 
        }
    }

    float travelSpeed = 2f;

    void Start()
    {
        Location = master.DetectTile(gameObject);

        //Clear Path
        master.ClearPath();
        //Make a Path
        //Debug.Log(master.DetectTile(gameObject).ToString() + ", " + master.DetectTile(MoveTo).ToString());
        master.FindPath(master.DetectTile(gameObject),master.DetectTile(MoveTo));
        //coroutine walk path
        Travel(master.GetPath());
    }


    // maybe in a coroutine!?
    void Update()
    {
        
    }

    /******************************/


    /* does this get reversed!? */

    List<TileData> pathToTravel;
    public void Travel(List<TileData> path)
    {
        Location = path[path.Count - 1];
        pathToTravel = path;

        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        Vector3 a, b, c = pathToTravel[0].transform.localPosition;
        transform.localPosition = c;
        yield return LookAt(pathToTravel[1].transform.localPosition);

        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            a = c;
            b = pathToTravel[i - 1].transform.localPosition;
            c = (b + pathToTravel[i].transform.localPosition) * 0.5f;
            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f; // keeps from leaning up/down hill
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            t -= 1f;
        }

        a = c;
        b = pathToTravel[pathToTravel.Count - 1].transform.localPosition;
        c = b;
        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f; // keeps from leaning up/down hill
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        transform.localPosition = Location.transform.localPosition;
        orientation = transform.localRotation.eulerAngles.y;

        //ListPool<TileData>.Add(pathToTravel);
        pathToTravel = null;
    }

    const float rotationSpeed = 180f;
    IEnumerator LookAt(Vector3 point)
    {
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);

        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;

            for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed)
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
        }
        transform.LookAt(point);
        orientation = transform.localRotation.eulerAngles.y;
    }

    /*
     * 
     * FROM HEXGAMEUI, WUT?
     * 
    void DoPathfinding()
    {
        if (currentCell && selectedUnit.IsValidDestination(currentCell))
        {
            grid.FindPath(selectedUnit.Location, currentCell, 24);
        }
        else
        {
            grid.ClearPath();
        }
    }

    */

}
