using UnityEngine;

public static class Extension_Bound
{
	public static Vector2 Corner_LeftUp(this Bounds bound) => new Vector2(-bound.extents.x, bound.extents.y);
	public static Vector2 Corner_RightUp(this Bounds bound) => new Vector2(bound.extents.x, bound.extents.y);
	public static Vector2 Corner_LeftDown(this Bounds bound) => new Vector2(-bound.extents.x, -bound.extents.y);
	public static Vector2 Corner_RightDown(this Bounds bound) => new Vector2(bound.extents.x, -bound.extents.y);

	public static bool IsOutUpBound(this Bounds bound, Vector3 pos) => (pos - bound.center).y > bound.extents.y;
	public static bool IsOutLeftBound(this Bounds bound, Vector3 pos) => (pos - bound.center).x > bound.extents.x;
	public static bool IsOutDownBound(this Bounds bound, Vector3 pos) => (pos - bound.center).y < -bound.extents.y;
	public static bool IsOutRightBound(this Bounds bound, Vector3 pos) => (pos - bound.center).x < -bound.extents.x;

	public static bool IsOutVerticalBound(this Bounds bound, Vector3 pos) => Mathf.Abs((pos - bound.center).y) > Mathf.Abs(bound.extents.y);
	public static bool IsOutHorizontalBound(this Bounds bound, Vector3 pos) => Mathf.Abs((pos - bound.center).x) > Mathf.Abs(bound.extents.x);


	public static Vector3 RandomPointInBounds(this Bounds bounds)
	{
		return bounds.center + new Vector3(
			Random.Range(bounds.min.x, bounds.max.x),
			Random.Range(bounds.min.y, bounds.max.y),
			Random.Range(bounds.min.z, bounds.max.z)
		);
	}
}
