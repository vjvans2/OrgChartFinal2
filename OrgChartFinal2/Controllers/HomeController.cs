using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using OrgChartFinal2.Models;

namespace OrgChartFinal2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new CompanyCSV());
        }

        [HttpPost]
        public ActionResult InsertNewCSV(HttpPostedFileBase csvFile)
        {
            if (csvFile != null && csvFile.FileName.Contains(".csv"))
            {
                //get info
                CompanyCSV csv = new CompanyCSV();
                using (TextFieldParser csvParser = new TextFieldParser(csvFile.InputStream))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();
                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();
                        Employee employee = new Employee
                        {
                            EmployeeId = Convert.ToInt32(fields[0]),
                            FirstName = fields[1],
                            LastName = fields[2],
                            Title = fields[3],
                            ManagerId = GetManagerId(fields[4]),
                        };

                        csv.Employees.Add(employee);
                    }
                }

                Session["employees"] = csv.Employees;


                return View("~/Views/Home/Index.cshtml", csv);
            }
            else
            {
                return View("~/Views/Home/Index.cshtml", new CompanyCSV());
            }
        }

        public int? GetManagerId(string field)
        {
            if (field == "null")
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(field);
            }
        }

        public ActionResult ShowEmployeeInfo(int? employeeId)
        {
            if (employeeId.HasValue && employeeId > 0)
            {
                List<Employee> employees = Session["employees"] as List<Employee>;
                employees.ForEach(x => x.IsChosen = false);
                Employee chosen = employees.SingleOrDefault(x => x.EmployeeId == employeeId);
                chosen.IsChosen = true;
                CompanyCSV model = new CompanyCSV()
                {
                    Employees = employees,
                    ChosenEmployee = chosen,
                    ChosenMappedEmployees = GetChosenMappedEmployees(chosen, employees)
                };
                return View("~/Views/Home/Index.cshtml", model);
            }
            else
            {
                List<Employee> employees = Session["employees"] as List<Employee>;
                CompanyCSV model = new CompanyCSV()
                {
                    Employees = employees
                };

                return View("~/Views/Home/Index.cshtml", model);
            }
        }

        public List<Employee> GetChosenMappedEmployees(Employee chosen, List<Employee> allEmployees)
        {
            var mapped = allEmployees.Where(x => x.ManagerId == chosen.EmployeeId).ToList();
            if (mapped.Count() > 0)
            {
                mapped.Add(chosen);
            }
            return mapped;
        }
    }
}