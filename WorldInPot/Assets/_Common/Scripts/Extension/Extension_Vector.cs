using UnityEngine;


public static class Extension_Vector
{
	public static Vector3 FlatXY(this Vector3 vector) => new Vector3(vector.x, vector.y, 0);
	public static Vector3 FlatXZ(this Vector3 vector) => new Vector3(vector.x, 0, vector.z);

	public static Vector2 XY(this Vector3 vector) => new Vector2(vector.x, vector.y);
	public static Vector2 XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);
	/*
    public static Vector2 Corner_RightUp(this Bounds bound) => (Vector2)bound.center + new Vector2(bound.extents.x, bound.extents.y);
    public static Vector2 Corner_LeftDown(this Bounds bound) => (Vector2)bound.center + new Vector2(-bound.extents.x, -bound.extents.y);
    public static Vector2 Corner_RightDown(this Bounds bound) => (Vector2)bound.center + new Vector2(bound.extents.x, -bound.extents.y);
    */
}
