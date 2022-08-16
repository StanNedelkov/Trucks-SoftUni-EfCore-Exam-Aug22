namespace Trucks.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        

        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DespatcherXmlDto[]), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            DespatcherXmlDto[] despatchers = context.Despatchers
                .ToArray()
                .Where(d => d.Trucks.Count > 0)
                .Select(d => new DespatcherXmlDto() 
                {
                    TrucksCount = d.Trucks.Count,
                    DespatcherName = d.Name,
                    Trucks = d.Trucks.Select(t => new TrucksXmlDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                    
                })
                .OrderByDescending(d => d.Trucks.Count())
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, despatchers, namespaces);

            return sb.ToString().Trim();

            
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientTrucks = context.Clients
                .ToArray()
               .Where(x => x.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
               .Select(x => new
               {
                   Name = x.Name,
                   Trucks = x.ClientsTrucks.Where(t => t.Truck.TankCapacity >= capacity).Select(t => new
                   {
                       TruckRegistrationNumber = t.Truck.RegistrationNumber,
                       VinNumber = t.Truck.VinNumber,
                       TankCapacity = t.Truck.TankCapacity,
                       CargoCapacity = t.Truck.CargoCapacity,
                       CategoryType = t.Truck.CategoryType.ToString(),
                       MakeType = t.Truck.MakeType.ToString()
                   }).ToArray().OrderBy(t => t.MakeType).ThenByDescending(t => t.CargoCapacity)
               }).OrderByDescending(x => x.Trucks.Count()).ThenBy(x => x.Name).Take(10)
               .ToArray();

            string json = JsonConvert.SerializeObject(clientTrucks,Formatting.Indented);
            return json.Trim();
        }
    }
}
