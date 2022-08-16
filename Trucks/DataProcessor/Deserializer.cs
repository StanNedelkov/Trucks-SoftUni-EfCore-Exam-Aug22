namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DespatcherXmlDto[]), xmlRoot);

            using StringReader reader = new StringReader(xmlString);
            DespatcherXmlDto[] dDtos = (DespatcherXmlDto[])xmlSerializer.Deserialize(reader);

            ICollection<Despatcher> despatchers = new List<Despatcher>();

            foreach (var dDto in dDtos)
            {
                if (!IsValid(dDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = dDto.Name,
                    Position = dDto.Position
                };
                ICollection<Truck> trucks = new List<Truck>();

                foreach (var tDto in dDto.Trucks)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                   /* if (!Enum.IsDefined(typeof(CategoryType),tDto.CategoryType))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!Enum.IsDefined(typeof(MakeType), tDto.MakeType))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }*/

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = tDto.RegistrationNumber,
                        VinNumber = tDto.VinNumber,
                        TankCapacity = tDto.TankCapacity,
                        CargoCapacity = tDto.CargoCapacity,
                        CategoryType = (CategoryType)tDto.CategoryType,
                        MakeType = (MakeType)tDto.MakeType
                        
                    };
                    trucks.Add(truck);
                }
                despatcher.Trucks = trucks;
                despatchers.Add(despatcher);
                sb.AppendLine($"Successfully imported despatcher - {despatcher.Name} with {despatcher.Trucks.Count} trucks.");
            }
            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();
            return sb.ToString().Trim();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var cDtos = JsonConvert.DeserializeObject<ClientsJsonDto[]>(jsonString);

            List<Client> clients = new List<Client>();

            foreach (var cDto in cDtos)
            {
                if (!IsValid(cDto) || cDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality,
                    Type = cDto.Type
                };

                foreach (var currentTruck in cDto.Trucks.Distinct())
                {
                    Truck t = context.Trucks.Find(currentTruck);
                    if (t == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    client.ClientsTrucks.Add(new ClientTruck() { TruckId = currentTruck });
                }
                clients.Add(client);
                sb.AppendLine($"Successfully imported client - {client.Name} with {client.ClientsTrucks.Count} trucks.");
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
