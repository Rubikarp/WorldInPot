using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ESubstratShape
{
    Flat = 0,
    Slope = 1,
    Hill = 2,
    Valley = 3,
}
public enum ELayer
{
    Bocal = 0,
    Drainage = 1,
    Filter = 2,
    Substrat = 3,
}

public enum ETerrariumStep
{
    Bocal = 0,
    Drainage = 1,
    Filter = 2,
    SubstratAndShape = 3,   
    VegetationAndDecoration = 4,
}

public class TerrariumBuilder : SingletonMono<TerrariumBuilder> 
{
    private GameHandler gameHandler;

    [Header("Layer Settings")]
    public ETerrariumStep creationProgress = ETerrariumStep.Bocal;

    public UnityAction<ETerrariumStep> onProgressChanged;

    [Header("Layer Settings")]
    public ESubstratShape substratShape = ESubstratShape.Flat;
    public Mesh[] substratShapeMeshes;

    [Header("Terrarium Data")]
    public BocalData currentBocal = null;
    public DrainageData currentDrainage = null;
    public FilterData currentFilter = null;
    public SubstratData currentSubstrat = null;

    [Header("Terrarium Components")]
    public MeshFilter bocalMesh = null; 
    public MeshRenderer drainageMeshRenderer = null;
    public MeshRenderer currentFilterMeshRenderer = null;
    public MeshRenderer substratMeshRenderer = null;
    public MeshCollider substratMeshCollider = null;
    public MeshFilter substratMesh = null;

    [Header("Vegetation and Decoration")]
    public Transform vegetationParent;
    public List<VegetationData> currentVegetation = new List<VegetationData>();
    public List<GameObject> currentVegetationObject = new List<GameObject>();

    public Transform decorationParent;
    public List<DecorationData> currentDecoration = new List<DecorationData>();
    public List<GameObject> currentStructureObject = new List<GameObject>();    

    void Start()
    {
        gameHandler = GameHandler.Instance;
        ClearAllElements();
    }

    public void SelectBocal(BocalData bocalData)
    {
        currentBocal = bocalData;
        if (bocalMesh != null)
        {
            bocalMesh.mesh = bocalData.bocalMesh;
        }
        ProgressToNextStep();
    }
    public void SelectDrainage(DrainageData drainageData)
    {
        currentDrainage = drainageData;
        if (drainageMeshRenderer != null)
        {
            drainageMeshRenderer.material = drainageData.drainageMaterial;
            drainageMeshRenderer.enabled = true;
        }
        ProgressToNextStep();
    }
    public void SelectFilter(FilterData filterData)
    {
        currentFilter = filterData;
        if (currentFilterMeshRenderer != null)
        {
            currentFilterMeshRenderer.material = filterData.filterMaterial;
            currentFilterMeshRenderer.enabled = true;
        }
        ProgressToNextStep();
    }
    public void SelectSubstrat(SubstratData substratData)
    {
        currentSubstrat = substratData;
        if (substratMeshRenderer != null)
        {
            substratMeshRenderer.material = substratData.substratMaterial;
            substratMeshRenderer.enabled = true;
        }

        SelectSubstratShape(substratShape);
        ProgressToNextStep();
    }
    public void SelectSubstratShape(ESubstratShape shape)
    {
        substratShape = shape;
        if (substratMesh != null && substratShapeMeshes != null)
        {
            substratMesh.mesh = substratShapeMeshes[(int)shape];
            if (substratMeshCollider != null)
            {
                substratMeshCollider.sharedMesh = null;
                substratMeshCollider.sharedMesh = substratShapeMeshes[(int)shape];
                Physics.SyncTransforms();
            }
        }
    }

    private void ProgressToNextStep()
    {
        ETerrariumStep nextStep = creationProgress;

        switch (creationProgress)
        {
            case ETerrariumStep.Bocal:
                if (currentBocal != null)
                {
                    creationProgress = ETerrariumStep.Drainage;
                    onProgressChanged?.Invoke(creationProgress);
                }
                break;
            case ETerrariumStep.Drainage:
                if (currentDrainage != null)
                {
                    creationProgress = ETerrariumStep.Filter;
                    onProgressChanged?.Invoke(creationProgress);
                }
                break;
            case ETerrariumStep.Filter:
                if (currentFilter != null)
                {
                    creationProgress = ETerrariumStep.SubstratAndShape;
                    onProgressChanged?.Invoke(creationProgress);
                }
                break;
            case ETerrariumStep.SubstratAndShape:
                if (currentSubstrat != null)
                {
                    creationProgress = ETerrariumStep.VegetationAndDecoration;
                    onProgressChanged?.Invoke(creationProgress);
                }
                break;
            case ETerrariumStep.VegetationAndDecoration:
                // Final step, no progression needed
                break;
        }
    }

    public void AddVegetation(VegetationData vegetationData, Vector3 position, Quaternion rotation)
    {
        currentVegetation.Add(vegetationData);
        
        GameObject vegetationObject = Instantiate(vegetationData.vegetationPrefab, position, rotation, vegetationParent);
        currentVegetationObject.Add(vegetationObject);
    }
    public void AddDecoration(DecorationData decorationData, Vector3 position, Quaternion rotation)
    {
        currentDecoration.Add(decorationData);
        
        GameObject decorationObject = Instantiate(decorationData.decorationPrefab, position, rotation, decorationParent);
        currentStructureObject.Add(decorationObject);
    }

    public void RemoveObject(GameObject objectToRemove)
    {
        if (currentVegetationObject.Contains(objectToRemove))
        {
            RemoveVegetation(objectToRemove);
        }
        else if (currentStructureObject.Contains(objectToRemove))
        {
            RemoveDecoration(objectToRemove);
        }
        else
        {
            Debug.LogError($"Tried to remove an object that is not in the terrarium: {objectToRemove.name}");
        }
    }
    public void RemoveVegetation(GameObject vegetationObject)
    {
        if (!currentVegetationObject.Contains(vegetationObject))
        {
            Debug.LogError($"Tried to remove vegetation object that is not in the terrarium: {vegetationObject.name}");
            return;
        }

        int index = currentVegetationObject.FindIndex(x => x == vegetationObject);
        currentVegetation.RemoveAt(index);
        
        if (vegetationObject != null)
        {
            Destroy(vegetationObject);
        }
        currentVegetationObject.RemoveAt(index);
    }
    public void RemoveDecoration(GameObject decorationObject) 
    {
        if (!currentStructureObject.Contains(decorationObject))
        {
            Debug.LogError($"Tried to remove decoration object that is not in the terrarium: {decorationObject.name}");
            return;
        }

        int index = currentStructureObject.FindIndex(x => x == decorationObject);
        currentDecoration.RemoveAt(index);
        
        if (decorationObject != null)
        {
            Destroy(decorationObject);
        }
        currentStructureObject.RemoveAt(index);
    }

    [Button("Clear All Elements")]
    public void ClearAllElements()
    {
        // Clear vegetation
        for (int i = currentVegetationObject.Count - 1; i >= 0; i--)
        {
            if (currentVegetationObject[i] != null)
            {
                Destroy(currentVegetationObject[i]);
            }
        }
        currentVegetation.Clear();
        currentVegetationObject.Clear();

        // Clear decorations
        for (int i = currentStructureObject.Count - 1; i >= 0; i--)
        {
            if (currentStructureObject[i] != null)
            {
                Destroy(currentStructureObject[i]);
            }
        }
        currentDecoration.Clear();
        currentStructureObject.Clear();

        // Reset layers
        if (bocalMesh != null) bocalMesh.mesh = null;
        currentBocal = null;

        if (drainageMeshRenderer != null)
        {
            drainageMeshRenderer.material = null;
            drainageMeshRenderer.enabled = false;
        }
        currentDrainage = null;

        if (currentFilterMeshRenderer != null)
        {
            currentFilterMeshRenderer.material = null;
            currentFilterMeshRenderer.enabled = false;
        }
        currentFilter = null;

        if (substratMesh != null) substratMesh.mesh = null;
        if (substratMeshRenderer != null)
        {
            substratMeshRenderer.material = null;
            substratMeshRenderer.enabled = false;
        }
        currentSubstrat = null;

        creationProgress = ETerrariumStep.Bocal;
    }
    [Button("Start From Scratch")]
    public void StartFromScratch()
    {
        ClearAllElements();
        creationProgress = ETerrariumStep.Bocal;
        gameHandler.DeselectObject();
    }
}
