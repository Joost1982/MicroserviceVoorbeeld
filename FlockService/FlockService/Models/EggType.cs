using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Models
{
    public class EggType
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // om ervoor te zorgen dat we id kunnen inserten -> https://stackoverflow.com/questions/58841616/set-identity-insert-tablename-on-does-not-work-on-entity-framework-core-2-2
        public int Id { get; set; }
        [Required]
        public int ExternalId { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Flock> Flocks { get; set; } = new List<Flock>();

        public override string ToString()
        {
            return $"Id: {Id}, ExternalId: {ExternalId}, Description: {Description}";
        }
    }
}
