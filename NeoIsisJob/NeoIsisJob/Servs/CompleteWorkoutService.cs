﻿using NeoIsisJob.Models;
using NeoIsisJob.Repos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoIsisJob.Servs
{
    public class CompleteWorkoutService
    {
        private readonly CompleteWorkoutRepo _completeWorkoutRepo;

        public CompleteWorkoutService()
        {
            this._completeWorkoutRepo = new CompleteWorkoutRepo();
        }

        public IList<CompleteWorkoutModel> GetAllCompleteWorkouts()
        {
            return this._completeWorkoutRepo.GetAllCompleteWorkouts();
        }

        public IList<CompleteWorkoutModel> GetCompleteWorkoutsByWorkoutId(int wid)
        {
            //like filter in java
            //return (IList<CompleteWorkoutModel>)this._completeWorkoutRepo.GetAllCompleteWorkouts().Where(completeWorkout => completeWorkout.WorkoutId == wid);

            IList<CompleteWorkoutModel> completeWorkouts = new List<CompleteWorkoutModel>();
            foreach (CompleteWorkoutModel completeWorkout in this._completeWorkoutRepo.GetAllCompleteWorkouts().Where(completeWorkout => completeWorkout.WorkoutId == wid))
            {
                completeWorkouts.Add(completeWorkout);
            }

            return completeWorkouts;
        }

        public void DeleteCompleteWorkoutsByWid(int wid)
        {
            this._completeWorkoutRepo.DeleteCompleteWorkoutsByWid(wid);
        }

        public void InsertCompleteWorkout(int wid, int eid, int sets, int repsPerSet)
        {
            this._completeWorkoutRepo.InsertCompleteWorkout(wid, eid, sets, repsPerSet);
        }
    }
}
