using UnityEngine;

[CreateAssetMenu(fileName = "BocalData", menuName = "Terrarium/BocalData")]
public class BocalData : ScriptableObject
{
    [field: SerializeField]
    public string bocalName { get; private set; }

    public Mesh bocalMesh;
    public Sprite bocalSprite;
}
