using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Data
{
    public class BaseModel
    {
        //TODO: Add application properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}