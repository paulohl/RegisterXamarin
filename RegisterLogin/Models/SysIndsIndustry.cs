using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterLogin.Models
{
    public partial class SysIndsIndustry
    {
        public long IndsId { get; set; }
        public string IndsDesc { get; set; }
        public DateTime IndsCreateDtm { get; set; }

        public string INDS_ID { get; set; }
        public string INDS_DESC { get; set; }
        public string INDS_CAT { get; set; }
        public DateTime INDS_CREATE_DTM { get; set; }
    }
}
