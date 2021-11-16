using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HireFest.Models;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace HireFest.Controllers
{
    public class AuthController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        void ConnectionString()
        {
            con.ConnectionString = "data source=MOHAK-GOGIA-L; database=HireFest; integrated security = SSPI;";
        }

        [HttpPost]
        public ActionResult Login(Login acc)
        {
            try
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "Select * from Login where Email='" + acc.Email + "' and Password='" + acc.Password + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {

                    return View("Homepage");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Exception found: " + ex.Message);
                return View();
            }
            finally
            {
                con.Close();
            }
            
        }

        [HttpPost]
        public ActionResult SaveInDb(Signup acc)
        {
            try
            {
                ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into Signup(Fname,Lname,PhnNum,Email) VALUES('" + acc.Fname + "','" + acc.Lname + "','" + acc.PhnNum + "','" + acc.Email + "')";
                dr = com.ExecuteReader();
                con.Close();
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into Login(Email,Password) VALUES('" + acc.Email + "','" + acc.Password + "')";
                dr = com.ExecuteReader();
                con.Close();
                return View("Homepage");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Exception found: " + ex.Message);
                return View("Signup");
            }
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

    }
}