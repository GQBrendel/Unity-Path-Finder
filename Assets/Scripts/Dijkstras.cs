/*
    Code adapted from @Hasan Bayat

    Thanks @Hasan Bayat!
    Original Source: https://github.com/EmpireWorld/unity-dijkstras-pathfinding.git
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstras : MonoBehaviour {

    public List<Node> nodesList = new List<Node>();

    void Start () {
		
	}
	
	public Path GetShortestPath(Node start, Node end)
    {

        // The final path
        Path path = new Path();

        // If the start and end are same node, we can return the start node
        if (start == end)
        {
            path.nodes.Add(start);
            return path;
        }

        // The list of unvisited nodes
        List<Node> unvisited = new List<Node>();

        // Previous nodes in optimal path from source
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        // The calculated distances, set all to Infinity at start, except the start Node
        Dictionary<Node, float> distances = new Dictionary<Node, float>();

        for (int i = 0; i < nodesList.Count; i++)
        {
            Node node = nodesList[i];
            unvisited.Add(node);

            // Setting the node distance to Infinity
            distances.Add(node, float.MaxValue);
        }

        // Set the starting Node distance to zero
        distances[start] = 0f;

        while (unvisited.Count != 0)
        {

            // Ordering the unvisited list by distance, smallest distance at start and largest at end
            unvisited = unvisited.OrderBy(node => distances[node]).ToList();

            // Getting the Node with smallest distance
            Node currentNode = unvisited[0];

            // Remove the current node from unvisisted list
            unvisited.Remove(currentNode);

            // When the current node is equal to the end node, then we can break and return the path
            if (currentNode == end)
            {
                // Construct the shortest path
                while (previous.ContainsKey(currentNode))
                {

                    // Insert the node onto the final result
                    path.nodes.Insert(0, currentNode);

                    // Traverse from start to end
                    currentNode = previous[currentNode];
                }

                // Insert the source onto the final result
                path.nodes.Insert(0, currentNode);
                break;
            }

            // Looping through the Node connections (neighbors) and where the connection (neighbor)
            //is available at unvisited list
            for (int i = 0; i < currentNode.connections.Count; i++)
            {
                Node neighborNode = currentNode.connections[i];

                // Getting the distance between the current node and the connection (neighbor)
                float length = Vector3.Distance(currentNode.transform.position, neighborNode.transform.position);

                // The distance from start node to this connection (neighbor) of current node
                float alternativeDistance = distances[currentNode] + length;

                // A shorter path to the connection (neighbor) has been found
                if (alternativeDistance < distances[neighborNode])
                {
                    distances[neighborNode] = alternativeDistance;
                    previous[neighborNode] = currentNode;
                }
            }
        }

        path.Bake();
        return path;

    }
}
