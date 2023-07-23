using UnityEngine;

public class PlantCollider : MonoBehaviour
{
    [System.Serializable]
    public struct ColliderData
    {
        public Vector3 center;
        public Vector3 size;
    }

    [SerializeField] Plant plant;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] ColliderData sproutColliderValues;
    [SerializeField] ColliderData grownColliderValues;

    private void Start()
    {
        plant.OnChangeTypeReceived += OnChangedToSprout;
        plant.OnPlantFullyGrown += OnChangedToGrown;
        if(GetComponent<Plant>().PlantData.growPercentage < 1) OnChangedToSprout(Plant.PlantTypes.WaterPlant);
        else OnChangedToGrown();
    }

    private void OnDestroy()
    {
        plant.OnChangeTypeReceived -= OnChangedToSprout;
        plant.OnPlantFullyGrown -= OnChangedToGrown;
    }

    void OnChangedToSprout(Plant.PlantTypes type)
    {
        boxCollider.center = sproutColliderValues.center;
        boxCollider.size = sproutColliderValues.size;
    }

    void OnChangedToGrown()
    {
        boxCollider.center = grownColliderValues.center;
        boxCollider.size = grownColliderValues.size;
    }
}
