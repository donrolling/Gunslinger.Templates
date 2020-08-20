using Dapper.SQLGateway.Models.Base;
using System;
using System.Collections.Generic;

namespace idi.sample.data.Entities {
    public class User : BaseEntity<long> {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}