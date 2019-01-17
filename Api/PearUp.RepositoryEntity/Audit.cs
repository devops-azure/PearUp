using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PearUp.RepositoryEntity
{
    public class Audit
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate
        {
            get
            {
                return this.createdDate ?? DateTime.Now;
            }

            set { this.createdDate = value; }
        }
        private DateTime? createdDate;
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}