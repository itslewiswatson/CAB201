using System.Collections.Generic;
using System.Linq;

namespace MRRC
{
    /// <summary>
    /// Rental implemented entity parser
    /// Lewis Watson 2020
    /// </summary>
    public class RentalEntityParser : EntityParser, TypedEntityParser<Rental>
    {
        public RentalEntityParser(string rentalFile) : base(rentalFile) { }

        protected override string GetCsvFileName()
        {
            return "rentals.csv";
        }

        protected override string GetEntityClassName()
        {
            return "Rental";
        }

        public Rental Downcast(Entity entity)
        {
            return (Rental)entity;
        }

        public List<Rental> Downcast(List<Entity> entities)
        {
            List<Rental> rentals = new List<Rental>();

            foreach (Entity entity in entities)
            {
                rentals.Add(Downcast(entity));
            }

            return rentals;
        }

        public List<Rental> LoadAll()
        {
            return Downcast(_LoadAll());
        }

        public void SaveAll(List<Rental> entities)
        {
            _SaveAll(Upcast(entities));
        }

        public List<Entity> Upcast(List<Rental> entities)
        {
            return entities.Cast<Entity>().ToList();
        }
    }
}
