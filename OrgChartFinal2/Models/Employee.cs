using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgChartFinal2.Models
{
    public class Employee
    {
        public Employee()
        {
            IsChosen = false;
        }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int? ManagerId { get; set; }
        public bool IsChosen { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}