using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckXmlDto
    {
        [XmlElement("RegistrationNumber")]
        [Required]
        [StringLength(8)]
        [RegularExpression(@"^[A-Z][A-Z][0-9][0-9][0-9][0-9][A-Z][A-Z]")]
        public string RegistrationNumber { get; set; }
        [XmlElement("VinNumber")]
        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; }
        [XmlElement("TankCapacity")]
        [Required]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }
        [XmlElement("CargoCapacity")]
        [Required]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }
        [XmlElement("CategoryType")]
        [Required]
        public int CategoryType { get; set; }
        [XmlElement("MakeType")]
        [Required]
        public int MakeType { get; set; }
    }
}
