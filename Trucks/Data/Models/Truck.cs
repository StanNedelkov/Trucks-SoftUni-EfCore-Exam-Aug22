using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(8)]
        [RegularExpression(@"^[A-Z][A-Z][0-9][0-9][0-9][0-9][A-Z][A-Z]")]
        public string RegistrationNumber { get; set; }
        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; }
        [Required]
        [Range(950,1420)]
        public int TankCapacity { get; set; }
        [Required]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }
        [Required]
        public CategoryType CategoryType { get; set; }
        [Required]
        public MakeType MakeType { get; set; }
        [Required]
        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public Despatcher Despatcher { get; set; }

        public ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
