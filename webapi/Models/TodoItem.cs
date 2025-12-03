using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("items")]
    public class TodoItem
    {
        //[Required]
        //public string ID { get; set; }

        //[Required]
        //public string? Name { get; set; }

        //[Required]
        //public string Notes { get; set; }

        //public bool Done { get; set; }

        
        
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("id")]
        //public long Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; }

        //[Column("complete")]       
        //public bool IsComplete { get; set; }
    }
}
