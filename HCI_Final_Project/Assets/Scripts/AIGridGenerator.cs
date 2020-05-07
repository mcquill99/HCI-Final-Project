using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;

using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//BEWARE. THIS FILE IS CURSED

[System.Serializable]
public class AIGridGenerator : MonoBehaviour
{
    [BoxGroup("Settings")] public NavMeshSurface navMesh;
    [BoxGroup("Settings")] public Transform player;

    [BoxGroup("DEBUG")] public bool showPoints;
    [BoxGroup("DEBUG")][ReadOnly] public int numberOfPoints = 0;
    [BoxGroup("DEBUG")][ReadOnly]public float pointDistance;

    private string fileName = "";

    public static List<AIGridPoint> points;
    private List<SearializableAIGridPoint> serializedPoints;
    private Bounds gridBounds;
    
    

    // public void OnBeforeSerialize() {

    // }

    // public void OnAfterDeserialize() {
    //     if(points == null) {
    //         deserializeFromList();
    //     }
    // }

    private string getFileName() {
        string newFileName = "Assets/AI/" + NavMesh.GetSettingsNameFromID(navMesh.agentTypeID).Replace(" ", "") + "_AIGridPoints.dat";
        if(!newFileName.Equals(fileName)) {
            fileName = newFileName;
        }
        return fileName;
    }

    //TODO: Make sure this works for builds
    public void serializeToList() {
        if(serializedPoints == null) serializedPoints = new List<SearializableAIGridPoint>();
        serializedPoints.Clear();
        foreach(AIGridPoint p in points) {
            serializedPoints.Add(new SearializableAIGridPoint() {
                pointX = p.point.x,
                pointY = p.point.y,
                pointZ = p.point.z
            });
        }
        
        FileStream fs = new FileStream(getFileName(), FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, serializedPoints);
        fs.Close();
    }

    [Button("Load Grid")]
    public void deserializeFromList() {
        string path = getFileName();

        if(!File.Exists(path)) {
            print("Cannot find file at: " + path);
            return;
        }
        
        FileStream fs = new FileStream(fileName, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        serializedPoints = (List<SearializableAIGridPoint>)bf.Deserialize(fs);
        fs.Close();

        if(points == null) points = new List<AIGridPoint>();
        points.Clear();
        foreach(SearializableAIGridPoint p in serializedPoints) {
            points.Add(new AIGridPoint(new Vector3(p.pointX, p.pointY, p.pointZ)));
        }
        numberOfPoints = points.Count;
        print("Read in " + points.Count + " points from: " + path);

    }

    void OnDrawGizmosSelected() {
        if(points == null || points.Count == 0) {
            deserializeFromList();
        }

        if(showPoints && points != null) {
            AIGridPoint point;
            float colorVal;
            if(Application.isPlaying) {
                for(int i = 0; i < points.Count; i++) {
                    // if(Random.Range(0, 9) % 10 != 0 || points[i].sqrProximityToPlayer > 1000f) continue;
                    colorVal = points[i].sqrProximityToPlayer / 1000f;
                    colorVal = Mathf.Round(colorVal * 3) / 3f;
                    colorVal = points[i].isVisible ? 1 : 0;
                    Gizmos.color = new Color(colorVal, 1 - colorVal, 0);
                    point = points[i];
                    Gizmos.DrawLine(point.getPoint() + Vector3.forward * pointDistance / 3f, point.getPoint() - Vector3.forward * pointDistance / 3f);
                    Gizmos.DrawLine(point.getPoint() + Vector3.right * pointDistance / 3f, point.getPoint() - Vector3.right * pointDistance / 3f);
                }
            } else {
                for(int i = 0; i < points.Count; i++) {
                    // print(points[i].point);
                    int index = i;
                    colorVal = points[index].sqrProximityToPlayer / 1000f;
                    Gizmos.color = new Color(colorVal, 1 - colorVal, 0);
                    point = points[index];
                    Gizmos.DrawLine(point.getPoint() + Vector3.forward * pointDistance / 3f, point.getPoint() - Vector3.forward * pointDistance / 3f);
                    Gizmos.DrawLine(point.getPoint() + Vector3.right * pointDistance / 3f, point.getPoint() - Vector3.right * pointDistance / 3f);
                }
                
            }
            // section = (section + 1) % NUM_SECTIONS;
        }
    } 

    [Button("Generate Grid")]
    public void generateGrid() {
        points = new List<AIGridPoint>();
        numberOfPoints = 0;
        pointDistance = NavMesh.GetSettingsByID(navMesh.agentTypeID).agentRadius * 3;
        gridBounds = new Bounds(navMesh.navMeshData.sourceBounds.center, navMesh.navMeshData.sourceBounds.size);
        print("Generating NavMesh with bounds: " + gridBounds.center + " | " + gridBounds.size);
        int numPointsX = Mathf.RoundToInt(gridBounds.size.x / pointDistance);
        int numPointsY = Mathf.RoundToInt(gridBounds.size.y / pointDistance);
        int numPointsZ = Mathf.RoundToInt(gridBounds.size.z / pointDistance);
        float halfDistX = gridBounds.size.x / 2f;
        float halfDistY = gridBounds.size.y / 2f;
        float halfDistZ = gridBounds.size.z / 2f;
        NavMeshQueryFilter filter = new NavMeshQueryFilter();
        filter.agentTypeID = navMesh.agentTypeID;
        filter.areaMask = ~0;
        NavMeshHit hit;
        Vector3 point;

        for(int x = 0; x < numPointsX; x++) {
            for(int y = 0; y < numPointsY; y++) {
                for(int z = 0; z < numPointsZ; z++) {
                    point = new Vector3((x * pointDistance) - halfDistX, (y * pointDistance) - halfDistY, (z * pointDistance) - halfDistZ);
                    //print(point);
                    if(NavMesh.SamplePosition(point, out hit, pointDistance, filter)) {
                        
                        if(hit.position.y < point.y)
                            points.Add(new AIGridPoint(hit.position));
                        
                        // print("Found point: " + point);
                    }
                }
            }
        }

        numberOfPoints = points.Count;
        //Undo.RecordObject(points, "Generated AI Points");
        serializeToList();

        print("Found " + points.Count + " points");
    }


    void Start() {
        //generateGrid();
        deserializeFromList();
    }

    void Update() {
        // if(points != null) {
        //     Vector3 playerPosition = player.position + Vector3.up;
        //     int layerMask = 1 | 1<<9 | 1<<10;
        //     float height = NavMesh.GetSettingsByID(navMesh.agentTypeID).agentHeight;
        //     // foreach(AIGridPoint p in points) {
        //     //     p.sqrProximityToPlayer = Vector3.SqrMagnitude(p.getPoint() - playerPosition);
        //     // }
        //     Parallel.ForEach(points, (p, state) => {
        //         p.sqrProximityToPlayer = Vector3.SqrMagnitude(p.getPoint() - playerPosition);
        //     });
        //     for(int i = 0; i < points.Count; i++) {
        //         if(Random.Range(0, 9) % 10 != 0 || points[(int)i].sqrProximityToPlayer > 1000f) continue;
        //         points[i].isVisible = Physics.Linecast(points[i].point + Vector3.up * height, playerPosition, layerMask);
        //     }
        // }
        
    }
}


