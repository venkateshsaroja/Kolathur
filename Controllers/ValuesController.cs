using Kolathur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace Kolathur.Controllers
{
    public class ValuesController : ApiController
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Webapi"].ConnectionString);
        state objstate = new state();

        // GET api/values
        public List<state> Get()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_Get", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<state> list = new List<state>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        state objstate = new state();
                        objstate.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                        objstate.State = dt.Rows[i]["State"].ToString();
                        objstate.District = dt.Rows[i]["District"].ToString();
                        objstate.pincode = Convert.ToInt32(dt.Rows[i]["Pincode"]);

                        list.Add(objstate);
                    }
                }

                if (list.Count > 0)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                
                return new List<state>();
            }


        }


        public string Get(int id)
        {
            return "id";
        }

        // POST api/values
        public string Post(state objstate)
        {
            string msg = "";
            if (objstate != null)
            {
                SqlCommand cmd =new SqlCommand("InsertMasterState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", objstate.Id);
                cmd.Parameters.AddWithValue("@State", objstate.State);
                cmd.Parameters.AddWithValue("@District", objstate.District);
                cmd.Parameters.AddWithValue("@Pincode ", objstate.pincode);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    msg = "data has been inserted";
                }
                else
                {
                    msg = "somthing wrong check the code";
                }
            }
            return msg;
        }

        // PUT api/values/5
        public string put(state objstate)
        {
            string msg = "";
            if (objstate != null)
            {
                SqlCommand cmd = new SqlCommand("UpdateMasterState", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", objstate.Id);
                cmd.Parameters.AddWithValue("@State", objstate.State);
                cmd.Parameters.AddWithValue("@District", objstate.District);
                cmd.Parameters.AddWithValue("@Pincode ", objstate.pincode);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    msg = "data has been update";
                }
                else
                {
                    msg = "somthing wrong check the code";
                }
            }
            return msg;
        }

        
        public string Delete(int id) 
        {
            string msg = "";
            try
            {
                SqlCommand cmd = new SqlCommand("DeleteMasterState", con); // Assuming you have a stored procedure for deleting
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id); // To identify the record to delete

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    msg = "Data has been deleted";
                }
                else
                {
                    msg = "No records deleted or something went wrong";
                }
            }
            catch (Exception ex)
            {
                con.Close();
                msg = "Error: " + ex.Message;
            }
            return msg;
        }
    }
 
}
