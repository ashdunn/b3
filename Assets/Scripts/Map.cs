using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

// Map Class from https://www.codeproject.com/Articles/1221034/Pathfinding-Algorithms-in-Csharp

namespace Common
{
    public class Map
    {

        public static Map fromNavMesh(UnityEngine.AI.NavMeshTriangulation triNavMesh)
        {
            var map = new Map();

            // Add Vertices
            for (int i = 0; i < triNavMesh.vertices.Length; i++)
            {
                var vec = triNavMesh.vertices[i];
                var newNode = Node.New(vec, i, i.ToString());
                map.Nodes.Add(newNode);
            }
            // Add Connections among triangles
            for (int i = 0; i < triNavMesh.indices.Length; i = i + 3)
            {
                var triangle = new List<Node>();
                var A = map.Nodes[triNavMesh.indices[i]];
                var B = map.Nodes[triNavMesh.indices[i+1]];
                var C = map.Nodes[triNavMesh.indices[i+2]];
                triangle.Add(A);
                triangle.Add(B);
                triangle.Add(C);

                int areaI = (int)Math.Floor(i/3.0);
                var area_indicator = triNavMesh.areas[areaI];
                // Cost from Unity
                float cost = UnityEngine.AI.NavMesh.GetAreaCost(area_indicator);
                A.Connect(triangle, cost);
                B.Connect(triangle, cost);
                C.Connect(triangle, cost);

            }

            // Debug Line
            foreach (var node in map.Nodes)
            {
                UnityEngine.Debug.Log($"{node}");
                foreach (var cnn in node.Connections)
                {
                    UnityEngine.Debug.Log($"{cnn}");
                }
            }

            return map;

        }

        public List<Node> Nodes { get; set; } = new List<Node>();

        // public Node StartNode { get; set; }

        // public Node EndNode { get; set; }

        // public List<Node> ShortestPath { get; set; } = new List<Node>();
    }

    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Point { get; set; }
        public List<Edge> Connections { get; set; } = new List<Edge>();

        // public double? MinCostToStart { get; set; }
        // public Node NearestToStart { get; set; }
        // public bool Visited { get; set; }
        // public double StraightLineDistanceToEnd { get; set; }

        internal static Node New(Vector3 v, int indice, string name)
        {
            return new Node
            {
                Point = new Point
                {
                    X = v.x,
                    Y = v.y,
                    Z = v.z
                },
                Id = indice,
                Name = name
            };
        }

        internal void Connect(List<Node> triangle, float weight)
        {
            var connections = new List<Edge>();
            foreach (var node in triangle)
            {
                if (node.Id == this.Id)
                    continue;

                var dist = Math.Sqrt(Math.Pow(Point.X - node.Point.X, 2) + Math.Pow(Point.Y - node.Point.Y, 2) + Math.Pow(Point.Z - node.Point.Z, 2));
                connections.Add(new Edge
                {
                    ConnectedNode = node,
                    Length = dist,
                    Cost = weight,
                });
            }
            connections = connections.OrderBy(x => x.Length).ToList();
            var count = 0;
            foreach (var cnn in connections)
            {
                //Connect three closes nodes that are not connected.
                if (!Connections.Any(c => c.ConnectedNode == cnn.ConnectedNode))
                    Connections.Add(cnn);
                count++;

                //Make it a two way connection if not already connected
                if (!cnn.ConnectedNode.Connections.Any(cc => cc.ConnectedNode == this))
                {
                    var backConnection = new Edge { ConnectedNode = this, Length = cnn.Length };
                    cnn.ConnectedNode.Connections.Add(backConnection);
                }
            }
        }


        public double StraightLineDistanceTo(Node end)
        {
            return Math.Sqrt(Math.Pow(Point.X - end.Point.X, 2) + Math.Pow(Point.Y - end.Point.Y, 2) + Math.Pow(Point.Z - end.Point.Z, 2));
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Edge
    {
        public double Length { get; set; }
        public double Cost { get; set; }
        public Node ConnectedNode { get; set; }

        public override string ToString()
        {
            return "-> " + ConnectedNode.ToString() + " (cost: " + Cost.ToString() + ")";
        }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
