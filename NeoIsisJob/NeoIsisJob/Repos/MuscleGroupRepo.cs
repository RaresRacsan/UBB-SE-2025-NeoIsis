﻿using NeoIsisJob.Data;
using NeoIsisJob.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoIsisJob.Repos
{
    public class MuscleGroupRepo
    {
        private readonly DatabaseHelper _databaseHelper;

        public MuscleGroupRepo()
        {
            this._databaseHelper = new DatabaseHelper();
        }

        //returns the found object is there, null otherwise
        public MuscleGroupModel GetMuscleGroupById(int mgid)
        {
            MuscleGroupModel muscleGroup = null;

            using (SqlConnection connection = this._databaseHelper.GetConnection())
            {
                connection.Open();

                String query = "SELECT * FROM MuscleGroups WHERE MGID=@mgid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@mgid", mgid);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    muscleGroup = new MuscleGroupModel(
                            Convert.ToInt32(reader["MGID"]),
                            Convert.ToString(reader["Name"])
                    );
                }
            }

            return muscleGroup;
        }

        //TODO -> add the rest of CRUD operations if necessary
    }
}
