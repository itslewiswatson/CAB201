using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MRRC
{
    /// <summary>
    /// Common functionality for an entity parser
    /// Lewis Watson 2020
    /// </summary>
    abstract public class EntityParser
    {
        private string dataFile { get; }

        public EntityParser(string dataFile)
        {
            this.dataFile = dataFile;
        }

        private const char Delimiter = ',';
        private const int Column_Ignore_Count = 2;

        /// <summary>
        /// Locates the data directory in a cross-platform friendly manner
        /// </summary>
        /// <returns>A string of the relative data directory</returns>
        private string GetDataDirectory()
        {
            return string.Format("..{0}..{0}..{0}Data{0}", Path.DirectorySeparatorChar.ToString());
        }

        abstract protected string GetEntityClassName();
        abstract protected string GetCsvFileName();

        /// <summary>
        /// Data file decided based on user input
        /// </summary>
        /// <returns></returns>
        private string GetDataFile()
        {
            if (dataFile == null)
            {
                return GetDataDirectory() + GetCsvFileName();
            }
            return dataFile;
        }

        /// <summary>
        /// Gets the correctly namespaced Type for an entity
        /// </summary>
        /// <param name="className">String representing the class name of any entity (eg: Customer)</param>
        /// <returns>Type of the class within the MRRC namespace</returns>
        public Type GetEntityType(string className)
        {
            Assembly asm = typeof(Entity).Assembly;
            return asm.GetType("MRRC." + className, true);
        }

        /// <summary>
        /// Converts an array of arguments into a fully-fledged entity
        /// </summary>
        /// <param name="args">Array of correct length for entity of its constructor arguments</param>
        /// <returns>Returns a new in-memory instance of the specified entity with the passed parameters</returns>
        public Entity CreateEntity(object[] args)
        {
            dynamic processor = CreateProcessor();
            dynamic DTO = CreateDTO(args);

            return processor.ConvertToEntity(DTO);
        }

        /// <summary>
        /// Dynamically create an entity processor for the given entity without the need for type-hinting generics
        /// </summary>
        /// <returns>A DTOProcessor for any given entity</returns>
        public dynamic CreateProcessor()
        {
            var processorType = GetEntityType(GetEntityClassName() + "DTOProcessor");
            dynamic loadedDTOProcessor = Activator.CreateInstance(processorType);

            return loadedDTOProcessor;
        }

        /// <summary>
        /// Creates a data-transfer-object representation of the specified entity
        /// </summary>
        /// <param name="args">An array of serialisable arguments</param>
        /// <returns>DTO specific to the given entity class</returns>
        public dynamic CreateDTO(object[] args)
        {
            var dtoType = GetEntityType(GetEntityClassName() + "DTO");
            dynamic convertedDTO = Activator.CreateInstance(dtoType, args);

            return convertedDTO;
        }

        /// <summary>
        /// Generic function to load all entities contained within the specified CSV file.
        /// These entities will need to be manually down-casted in their respective concrete entity parser.
        /// </summary>
        /// <returns>A list of generic entities that correspond to the given entity class</returns>
        protected List<Entity> _LoadAll()
        {
            try
            {
                if (!File.Exists(GetDataFile()))
                {
                    throw new FileNotFoundException(string.Format("'{0}' does not exist. Double check the directory you provided.", GetDataFile()));
                }

                List<Entity> entities = new List<Entity>();

                StreamReader streamReader = new StreamReader(GetDataFile());
                streamReader.ReadLine();

                // Read the data in the files
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    object[] args = line.Split(Delimiter);

                    // Ignore lines 
                    if (args.Length < Column_Ignore_Count)
                    {
                        continue;
                    }

                    Entity loadedEntity = CreateEntity(args);
                    entities.Add(loadedEntity);
                }

                streamReader.Close();

                return entities;
            }
            catch (Exception e)
            {
                // Do not overwrite custom exception
                if (e is FileNotFoundException)
                {
                    throw e;
                }
                else
                {
                    throw new Exception(string.Format("Data directory '{0}' given to entity parser is not incorrect", GetDataFile()));
                }
            }
        }

        public List<string> GetHeader()
        {
            StreamReader streamReader = new StreamReader(GetDataFile());
            string headerLine = streamReader.ReadLine();
            streamReader.Close();

            List<string> header = headerLine.Split(Delimiter).ToList();
            return header;
        }

        /// <summary>
        /// Rewrite the current data file with the specified list of entities
        /// </summary>
        /// <param name="entities">A list of generic entities to write to disk</param>
        protected void _SaveAll(List<Entity> entities)
        {
            // Read current file to extract header line
            StreamReader streamReader = new StreamReader(GetDataFile());
            string headerLine = streamReader.ReadLine();
            streamReader.Close();

            // Open file handle
            StreamWriter streamWriter = new StreamWriter(GetDataFile());
            streamWriter.WriteLine(headerLine);

            // Write each entity to file
            foreach (Entity entity in entities)
            {
                streamWriter.WriteLine(entity.ToString());
            }

            streamWriter.Close();
        }
    }
}
