/*
    Code from @Hasan Bayat

    Thanks @Hasan Bayat!
    Original Source: https://github.com/EmpireWorld/unity-dijkstras-pathfinding.git
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Path.
/// </summary>
public class Path
{

	/// <summary>
	/// The nodes.
	/// </summary>
	protected List<Node> m_Nodes = new List<Node> ();
	
	/// <summary>
	/// The length of the path.
	/// </summary>
	protected float m_Length = 0f;

	/// <summary>
	/// Gets the nodes.
	/// </summary>
	/// <value>The nodes.</value>
	public virtual List<Node> nodes
	{
		get
		{
			return m_Nodes;
		}
	}

	/// <summary>
	/// Gets the length of the path.
	/// </summary>
	/// <value>The length.</value>
	public virtual string length
	{
		get
		{
            return m_Length.ToString("f2");
		}
	}

	/// <summary>
	/// Bake the path.
	/// Making the path ready for usage, Such as caculating the length.
	/// </summary>
	public virtual void Bake ()
	{
		List<Node> calculated = new List<Node> ();
		m_Length = 0f;
		for ( int i = 0; i < m_Nodes.Count; i++ )
		{
			Node node = m_Nodes [ i ];
			for ( int j = 0; j < node.connections.Count; j++ )
			{
				Node connection = node.connections [ j ];
				
				// Don't calcualte calculated nodes
				if ( m_Nodes.Contains ( connection ) && !calculated.Contains ( connection ) )
				{
					// Calculating the distance between a node and connection when they are both available in path nodes list
			        m_Length += euclidianDistance(node.transform.position, connection.transform.position);
				}
			}
            node.turnBlue();
			calculated.Add ( node );
		}
        ToString();
	}

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString ()
	{
        string s;

        s = string.Format(
            "Nodes: {0}\nLength: {1}",
            string.Join(
                ", ",
                nodes.Select(node => node.name).ToArray()),
            length);
        Debug.Log(s);
        return s;
    }
    //Returns the euclidian distance beetween two points
    private float euclidianDistance(Vector3 p1, Vector3 p2)
    {
        float x0 = p1.x;
        float y0 = p1.z;

        float x1 = p2.x;
        float y1 = p2.z;

        float dX = x1 - x0;
        float dY = y1 - y0;
        double distance = System.Math.Sqrt(dX * dX + dY * dY);

        return (float)distance;
    }

}
