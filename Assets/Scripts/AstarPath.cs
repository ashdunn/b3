using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AstarPath : MonoBehaviour
{
    private UnityEngine.AI.NavMeshTriangulation triNavMesh;
    public Map Map { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        triNavMesh = UnityEngine.AI.NavMesh.CalculateTriangulation();
        Map.fromNavMesh(triNavMesh);
        PrintMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PrintMesh()
    {
        Debug.Log("Areas: " + triNavMesh.areas.Length); // 156
        Debug.Log("Indices: " + triNavMesh.indices.Length); // 468: 468/3 = 156
        Debug.Log("Vertices: " + triNavMesh.vertices.Length); // 362
        foreach (int a in triNavMesh.areas) {
            Debug.Log(string.Format("a {0}\n",a));
        }
        foreach (int ind in triNavMesh.indices) {
            Debug.Log(string.Format("{0}\t", ind));
        }
        foreach (Vector3 v in triNavMesh.vertices) {
            Debug.Log(string.Format("v {0} {1} {2}\n",v.x,v.y,v.z));
        }
    }

    // Prototype for Astar; Transfer Vec3 to Node
    
    List<Node> reconstruct_path(Dictionary<Node, Node> cameFrom, Node current)
    {
        var total_path = new List<Node>();
        total_path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            total_path.Insert(0, current); // TODO: prepend in Wiki but Add in course note?
        }
        return total_path;
    }

    // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
    double h(Node node)
    {
        // TODO
        return 1;
    }

    List<Node> A_Star(Node Start, Node End)
    {
        var closeSet = new HashSet<Node>();
        var openSet = new HashSet<Node>();
        openSet.Add(Start);
         // Map of Navigated Nodes
        var cameFrom = new Dictionary<Node, Node>();

        var gScore = new Dictionary<Node, double>();
        foreach (Node v in Map.Nodes) {
            gScore.Add(v, double.PositiveInfinity);
        }
        gScore[Start] = 0;

        // For node n, fScore[n] := gScore[n] + h(n).
        var fScore = new Dictionary<Node, double>();
        foreach (Node v in Map.Nodes) {
            fScore.Add(v, double.PositiveInfinity);
        }
        fScore[Start] = h(Start);

        // openSet is not empty
        while(openSet.Count != 0 )
        {
            var minfScore = double.PositiveInfinity;
            var current = Start;
            foreach (Node node in openSet) {
                if (fScore[node] <= minfScore)
                {
                    minfScore = fScore[node];
                    current = node;
                }
            }

            if(current == End)
            {
                return reconstruct_path(cameFrom, current);
            }

            openSet.Remove(current);
            closeSet.Add(current);
            foreach (var cnn in current.Connections) {
                Node neighbor = cnn.ConnectedNode;
                var weight = cnn.Cost;
                if (closeSet.Contains(neighbor))
                {
                    continue;
                }
                
                // d is the weight of the edge from current to neighbor
                var tentative_gScore = gScore[current] + weight;
                // var tentative_gScore = gScore[current] + d(current, neighbor);
                if (tentative_gScore < gScore[neighbor])
                {
                    // This path to neighbor is better than any previous one. Record it!
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = gScore[neighbor] + h(neighbor);
                    if (! (openSet.Contains(neighbor)))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        
        }

        // FAILURE!
        // return false;
        return new List<Node>();
    }

    // For approximating straitline
    void SmoothPath()
    {
        
    }
}