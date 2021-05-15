using System.Collections.Generic;
using System.Linq;

namespace MRRC
{
    /// <summary>
    /// Vehicle implemented entity parser
    /// Lewis Watson 2020
    /// </summary>
    public class VehicleEntityParser : EntityParser, TypedEntityParser<Vehicle>
    {
        public VehicleEntityParser(string vehicleFile) : base(vehicleFile) { }

        protected override string GetCsvFileName()
        {
            return "fleet.csv";
        }

        protected override string GetEntityClassName()
        {
            return "Vehicle";
        }

        public List<Vehicle> LoadAll()
        {
            return Downcast(_LoadAll());
        }

        public Vehicle Downcast(Entity entity)
        {
            return (Vehicle)entity;
        }

        public List<Vehicle> Downcast(List<Entity> entities)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (Entity entity in entities)
            {
                vehicles.Add(Downcast(entity));
            }

            return vehicles;
        }

        public List<Entity> Upcast(List<Vehicle> entities)
        {
            return entities.Cast<Entity>().ToList();
        }

        public void SaveAll(List<Vehicle> entities)
        {
            _SaveAll(Upcast(entities));
        }
    }
}
