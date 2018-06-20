using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgChartFinal2.Models
{
    public class CompanyCSV
    {
        public CompanyCSV()
        {
            Employees = new List<Employee>();
        }

        public List<Employee> Employees { get; set; }
        public Employee ChosenEmployee { get; set; }
        public List<Employee> ChosenMappedEmployees { get; set; }
    }
}