using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataBusiness;
namespace WebApiTutorial.Controllers
{
    public class EmployeeController : ApiController
    {

        //public IEnumerable<tblEmployee> LoadALlEmployees()
        //{
        //    using (SampleDBEntities db = new SampleDBEntities())
        //    {
        //        return db.tblEmployees.ToList();
        //    }
        //}
        [HttpGet]
        public HttpResponseMessage Get(string gender="all")
        {
            using(SampleDBEntities entities = new SampleDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.tblEmployees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.tblEmployees.Where(x => x.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.tblEmployees.Where(x => x.Gender.ToLower() == "female").ToList());

                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for gender must be all, male or female" + gender + "is invalid");
                }
            }
        }
        [HttpPost]
        public HttpResponseMessage Post([FromBody]tblEmployee employee)
        {
            try
            {
                using (SampleDBEntities entities = new SampleDBEntities())
                {
                    entities.tblEmployees.Add(employee);
                    entities.SaveChanges();


                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using(SampleDBEntities db = new SampleDBEntities())
            {
                var employee = db.tblEmployees.SingleOrDefault(x => x.Id == id);

                if(employee != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employee);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Employee with" + employee.Id.ToString() + "not found");
                }
            }
        }



        [HttpDelete]

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (SampleDBEntities entity = new SampleDBEntities())
                {
                    var employee = entity.tblEmployees.SingleOrDefault(x => x.Id == id);
                    if (employee == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "employee with ID" + employee.Id.ToString() + "is invalid");
                    }
                    else
                    {
                        entity.tblEmployees.Remove(employee);
                        entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }


                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]int id, [FromUri]tblEmployee employee)
        {
            try
            {
                using (SampleDBEntities entities = new SampleDBEntities())
                {
                    var entity = entities.tblEmployees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.Name = employee.Name;
                        entity.Gender = employee.Gender;
                        entity.City = employee.City;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
