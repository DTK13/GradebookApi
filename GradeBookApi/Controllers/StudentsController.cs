using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using StudentDataAccess;

namespace GradeBookApi.Controllers
{

    /// <summary>
    /// Welcome to GradeBook API
    /// </summary>
    /// 
    [EnableCors("http://localhost:4200", "Origin, Content-Type, X-Auth-Token, content-type", "GET,POST,PATCH,DELETE,PUT")]
    public class StudentsController : ApiController
    {
        /// <summary>
        /// Gets all Students
        /// </summary>
        /// <returns></returns>

        // GET: api/Students

        [Route("api/students")]
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            using (GradebookEntities entity = new GradebookEntities())
            {
                return entity.Students.ToList();
            }

        }

        [Route("api/students/{unique_id}")]
        [HttpGet]
        public Student Get(int unique_id)
        {
            using (GradebookEntities entity = new GradebookEntities())
            {
                return entity.Students.Where(x => x.Unique_Id == unique_id).First();
            }

        }

        /// <summary>
        /// Gets all Students by gradelevel
        /// </summary>
        /// <returns></returns>

        // GET: api/Students/GradeLevel

        [Route("api/students/gradelevel/{gradelevel}")]
        [HttpGet]
        public IEnumerable<Student> GetByGradeLevel(int gradeLevel)
        {
            using (GradebookEntities entity = new GradebookEntities())
            {
                return entity.Students.Where(x => x.GradeLevel == gradeLevel).ToList();
            }

        }

        /// <summary>
        /// Gets all Students by State
        /// </summary>
        /// <returns></returns>

        // GET: api/Students/GradeLevel
        [Route("api/students/state/{state}")]
        [HttpGet]
        public IEnumerable<Student> GetByGradeState(string state)
        {

            using (GradebookEntities entity = new GradebookEntities())
            {
                return entity.Students.Where(x => x.State == state).ToList();
            }


        }


        /// <summary>
        /// Adds one Student, do not include unique_Id, it will be assigned automatically.
        /// </summary>
        /// <returns></returns>


        // POST: api/AddStudent

        [Route("api/students/addstudent")]
        [HttpPost]

        public HttpResponseMessage AddStudent(HttpRequestMessage request, Student value)
        {

            HttpResponseMessage response = null;
            string message = string.Empty;

            if (value == null)
            {
                message = "No student entered!";
                return response = request.CreateResponse(HttpStatusCode.NoContent, message);
            }

            try
            {
                using (GradebookEntities entity = new GradebookEntities())
                {
                    if (value.Unique_Id == 0)
                    {
                        var lastStudentValue = entity.Students
                       .OrderByDescending(s => s.Unique_Id)
                       .FirstOrDefault();
                        int assignedID;
                        assignedID = lastStudentValue.Unique_Id + 1;
                        value.Unique_Id = assignedID;
                        entity.Students.Add(value);
                        entity.SaveChanges();
                        message = "Student Successfully Added";

                    }
                    else if (value.Unique_Id != 0)
                    {
                        message = "The unique_ID is a field that cannot be set manually. Please remove it and try again.";
                        return response = request.CreateResponse<string>(HttpStatusCode.BadRequest, message);
                    }

                }


            }
            catch (Exception e)
            {
                message = e.Message + "Please check to see if you are sending the student object correctly! Learn more here: http://localhost:50559/Help";
                return response = request.CreateResponse<string>(HttpStatusCode.BadRequest, message);
            }

            response = Request.CreateResponse<string>(HttpStatusCode.OK, message);
            return response;

        }

        /// <summary>
        /// Add as many students as you want. do not include unique_Id, it will be assigned automatically.
        /// </summary>
        /// <returns></returns>


        // POST: api/AddStudent
        [Route("api/students/addstudents/list")]
        [HttpPost]
        public HttpResponseMessage AddStudentList(HttpRequestMessage request, List<Student> values)
        {
            HttpResponseMessage response = null;
            string message = string.Empty;

            try
            {
                using (GradebookEntities entity = new GradebookEntities())
                {
                    foreach (var value in values)
                    {
                        if (value.Unique_Id == 0)
                        {
                            var lastStudentValue = entity.Students
                           .OrderByDescending(s => s.Unique_Id)
                           .FirstOrDefault();
                            int assignedID;
                            assignedID = lastStudentValue.Unique_Id + 1;
                            value.Unique_Id = assignedID;
                            entity.Students.Add(value);
                            entity.SaveChanges();
                            message = "Student Successfully Added";

                        }
                        else if (value.Unique_Id != 0)
                        {
                            message = "The unique_ID is a field that cannot be set manually. Please remove it and try again.";
                            return response = request.CreateResponse<string>(HttpStatusCode.BadRequest, message);
                        }
                    }

                }

                message = "Students Successfully Added";
            }
            catch (Exception e)
            {
                message = e.Message + "Please check to see if you are sending the student object correctly! Learn more here: http://localhost:50559/Help";
            }

            response = Request.CreateResponse<string>(HttpStatusCode.OK, message);
            return response;

        }




        /// <summary>
        /// Updates a Students information. Can send only one parameter or all of them. It's up to you.
        /// </summary>
        /// <returns></returns>
        [Route("api/students/updatestudent/{id}")]
        [HttpPatch]
        public HttpResponseMessage UpdateStudent(HttpRequestMessage request, int id, Student value)
        {
            HttpResponseMessage response = null;
            string message = string.Empty;

            try
            {
                using (GradebookEntities entity = new GradebookEntities())
                {
                    var student = (from s in entity.Students
                                   where s.Unique_Id == id
                                   select s).FirstOrDefault<Student>();

                    if (value.FirstName != null)
                    {
                        student.FirstName = value.FirstName;
                    }
                    else
                    {
                        value.FirstName = student.FirstName;
                    }
                    if (value.MiddleName != null)
                    {
                        student.MiddleName = value.MiddleName;
                    }
                    else
                    {
                        value.MiddleName = student.MiddleName;
                    }
                    if (value.LastName != null)
                    {
                        student.LastName = value.LastName;
                    }
                    else
                    {
                        value.LastName = student.LastName;
                    }

                    if (value.Street_Address != null)
                    {
                        student.Street_Address = value.Street_Address;
                    }
                    else
                    {
                        value.Street_Address = student.Street_Address;
                    }
                    if (value.Address_2 != null)
                    {
                        student.Address_2 = value.Address_2;
                    }
                    else
                    {
                        value.Address_2 = student.Address_2;
                    }
                    if (value.City != null)
                    {
                        student.City = value.City;
                    }
                    else
                    {
                        value.City = student.City;
                    }
                    if (value.State != null)
                    {
                        student.State = value.State;
                    }
                    else
                    {
                        value.State = student.State;
                    }
                    if (value.Age != null)
                    {
                        student.Age = value.Age;
                    }
                    else
                    {
                        value.Age = student.Age;
                    }
                    if (value.DOB != null)
                    {
                        student.DOB = value.DOB;
                    }
                    else
                    {
                        value.DOB = student.DOB;
                    }

                    if (value.GradeLevel != null)
                    {
                        student.GradeLevel = value.GradeLevel;
                    }
                    else
                    {
                        value.GradeLevel = student.GradeLevel;
                    }

                    entity.SaveChanges();
                }



                message = "Student Successfully Updated!";
            }
            catch (Exception e)
            {
                message = e.Message + "Please check to see if you are sending the student object correctly! Learn more here: http://localhost:50559/Help";
            }

            response = Request.CreateResponse<string>(HttpStatusCode.OK, message);
            return response;

        }


        // DELETE: api/DeleteStudent/5

        [Route("api/students/deletestudent/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteStudent(int id)
        {
            HttpResponseMessage response = null;
            string message = string.Empty;

            try
            {
                using (GradebookEntities entity = new GradebookEntities())
                {
                    var student = (from s in entity.Students
                                   where s.Unique_Id == id
                                   select s).FirstOrDefault<Student>();

                    entity.Students.Remove(student);
                    entity.SaveChanges();

                }

                message = "Student Successfully Removed!";
            }
            catch (Exception e)
            {
                message = e.Message + ".Please check to see if the Id is correct in the URL, the student may not exist!";
                response = Request.CreateResponse<string>(HttpStatusCode.BadRequest, message);
                return response;
            }

            response = Request.CreateResponse<string>(HttpStatusCode.OK, message);
            return response;
        }

    }
}
