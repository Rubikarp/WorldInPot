#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public static class Extension_Handles
{
	public static void DrawWireSquare(Vector3 center, Vector2 size)
	{
		Vector3 halfSize = ((Vector3)size) / 2;

		Handles.DrawLines(new Vector3[]{
			center + Vector3.Scale(halfSize, new Vector2(-1,1)),
			center + halfSize,
			center + halfSize,
			center + Vector3.Scale(halfSize, new Vector2(1,-1)),
			center + Vector3.Scale(halfSize, new Vector2(1,-1)),
			center - halfSize,
			center - halfSize,
			center + Vector3.Scale(halfSize, new Vector2(-1,1)),
		});
	}

}
#endif
