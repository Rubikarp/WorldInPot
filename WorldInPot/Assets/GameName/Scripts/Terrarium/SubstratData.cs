using UnityEngine;

[CreateAssetMenu(fileName = "SubstratData", menuName = "Terrarium/SubstratData")]
public class SubstratData : ScriptableObject
{
    [field:SerializeField]
    public string substratName { get; private set; }

    public Material substratMaterial;
    public float layerHeight;

    public Sprite substratIcon;

}
