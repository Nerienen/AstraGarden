using UnityEngine;

public class PlantCollider : MonoBehaviour
{
    [System.Serializable]
    public struct ColliderData
    {
        public Vector3 center;
        public float radius;
        public float height;
    }

    [SerializeField] Plant plant;
    [SerializeField] CapsuleCollider capsuleCollider;
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
        capsuleCollider.center = sproutColliderValues.center;
        capsuleCollider.radius = sproutColliderValues.radius;
        capsuleCollider.height = sproutColliderValues.height;
    }

    void OnChangedToGrown()
    {
        capsuleCollider.center = grownColliderValues.center;
        capsuleCollider.radius = grownColliderValues.radius;
        capsuleCollider.height = grownColliderValues.height;
    }
}
