using UnityEngine;

public interface IPlacableObject
{
    GameObject Prefab { get; }
    Mesh PreviewMesh { get; }
    string Name { get; }
} 