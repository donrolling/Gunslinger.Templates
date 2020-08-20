using Dapper.SQLGateway.Models.Base;
using System;
using System.Collections.Generic;

namespace idi.sample.data.Entities {
    public class Product : BaseEntity<long> {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}