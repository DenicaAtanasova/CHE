namespace CHE.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}