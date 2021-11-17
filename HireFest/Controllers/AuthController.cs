using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HireFest.Models;
using System.Data.SqlClient;
using System.Web.Mvc;
using HireFest.Services;


namespace HireFest.Controllers
{
    public class AuthController : Controller
    {
        AuthService auth = AuthService.GetInstance;
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login acc)
        {
            try
            {
                SqlConnection con = auth.ConnectionString();
                con.Open();
                com.Connection = con;
                HashingService hashObj = new HashingService();
                string hashedPassword = hashObj.PasswordHashing(acc.Password);
                com.CommandText = "Select * from Login where Email='" + acc.Email + "' and Password='" + hashedPassword + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    con.Close();
                    return View("Homepage");
                }
                else
                {
                    con.Close();
                    ViewBag.ErrorMessage = "Invalid Credentials";
                    return View();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception found: " + ex.Message);
                return View("Error");
            } 
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SaveInDb(Signup acc)
        {
            try
            {
                SqlConnection con = auth.ConnectionString();
                con.Open();
                com.Connection = con;
                com.CommandText = "Select * from Login where Email='" + acc.Email + "'";
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    con.Close();
                    ViewBag.ErrorMessage = "Invalid Details";
                    return View("Signup");
                }
                con.Close();
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into Signup(Fname,Lname,PhnNum,Email) VALUES('" + acc.Fname + "','" + acc.Lname + "','" + acc.PhnNum + "','" + acc.Email + "')";
                dr = com.ExecuteReader();
                con.Close();
                HashingService hashObj = new HashingService();
                string hashedPassword = hashObj.PasswordHashing(acc.Password);
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into Login(Email,Password) VALUES('" + acc.Email + "','" + hashedPassword + "')";
                dr = com.ExecuteReader();
                con.Close();
                return View("Homepage");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception found: " + ex.Message);
                return View("Error");
            }
        }
    }
}