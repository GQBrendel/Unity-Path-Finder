/*
    Code adapted from @Hasan Bayat

    Thanks @Hasan Bayat!
    Original Source: https://github.com/EmpireWorld/unity-dijkstras-pathfinding.git
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstras : MonoBehaviour {

    //This list keeps track of all the nodes in the scenes
    public List<Node> nodesList = new List<Node>();

    /// <summary>
    /// Returns the shortestef Path From two given nodes
    /// </summary>
    /// <param name="start">The first node of the path</param>
    /// <param name="end">The last node of the path</param>
    /// <returns></returns>
    public Path GetShortestPath(Node start, Node end)
    {

        // The final path
        Path path = new Path();

        // If the start and end are same node, we just return the start node, since the start and the end are the same
        if (start == end)
        {
            path.nodes.Add(start);
            return path;
        }

        // Keep the unvisited nodes in a list
        List<Node> unvisited = new List<Node>();

        // Create a chain of nodes, always pointing to the previous
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        // The calculated distances, set all to Infinity at start, except the start Node
        //Keeps tracks of all distances to show then later in the sum
        Dictionary<Node, float> distances = new Dictionary<Node, float>();


        //Iterate on nodelist to add then to the unvisited list
        for (int i = 0; i < nodesList.Count; i++)
        {
            //At the start all nodes are considered unvisited
            Node node = nodesList[i];
            unvisited.Add(node);

            // And all the distances are set to Infinity
            distances.Add(node, float.MaxValue);
        }

        // Set the starting Node distance to zero
        distances[start] = 0f;

        while (unvisited.Count != 0) //Let's iterate and "visit" all the nodes
        {

            // Ordering the unvisited list by distance, smallest distance at start
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
