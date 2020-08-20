using Dapper.SQLGateway.Models.Base;
using System;
using System.Collections.Generic;

namespace idi.sample.data.Entities {
    public class Contact : BaseEntity<long> {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}