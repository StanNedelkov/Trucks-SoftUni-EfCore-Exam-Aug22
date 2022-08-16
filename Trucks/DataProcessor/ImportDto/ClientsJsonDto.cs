using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Trucks.DataProcessor.ImportDto
{
    public class ClientsJsonDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        [JsonProperty(nameof(Nationality))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]

        public string Nationality { get; set; }
        [JsonProperty(nameof(Type))]
        [Required]
        public string Type { get; set; }
        [JsonProperty(nameof(Trucks))]
        public int[] Trucks { get; set; }
    }
}
