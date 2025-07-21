using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FACULTY_PORTAL.Models
{
    public class CreateChallanModel
    {
        public int StdRollNoID { get; set; }
        public string ChallanTypeID { get; set; }  
        public decimal Amount { get; set; }
        public string IssueDate { get; set; }
        public string DueDate { get; set; }
    }

}
