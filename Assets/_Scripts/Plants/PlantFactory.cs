using System.Collections.Generic;

public class PlantFactory
{
    Plant _context;
    Dictionary<Plant.PlantTypes, BasePlant> _plantsTypes = new Dictionary<Plant.PlantTypes, BasePlant>();

    public PlantFactory(Plant currentContext)
    {
        _context = currentContext;

        _plantsTypes.Add(Plant.PlantTypes.WaterPlant, new WaterPlant(_context, this));
        _plantsTypes.Add(Plant.PlantTypes.EnergyPlant, new EnergyPlant(_context, this));
        _plantsTypes.Add(Plant.PlantTypes.OxygenPlant, new OxygenPlant(_context, this));
    }

    public BasePlant GetConcretePlant(Plant.PlantTypes plantType)
    {
        return _plantsTypes[plantType];
    }
}
