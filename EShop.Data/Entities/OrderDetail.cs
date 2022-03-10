using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    [Table("OderDetails")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        public int OrderID { set; get; }

        [Required]
        public int ProductID { set; get; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { set; get; }

        public int Quantitty { set; get; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { set; get; }

        [ForeignKey("OrderID")]
        public virtual Order Order { set; get; }

        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }
    }



}
