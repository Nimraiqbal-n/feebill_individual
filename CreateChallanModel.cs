using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FACULTY_PORTAL.Models
{
    public class CreateChallanModel
    {
        public string StdRollNoID { get; set; }
        public string ChallanTypeID { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string SemesterSession { get; set; }   
    }


}
