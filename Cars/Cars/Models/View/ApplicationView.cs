﻿using System;
using System.Collections.Generic;
using Cars.Models.DataModels;

namespace Cars.Models.View
{
    public class ApplicationView
    {
        public int Id { get; set; }

        public UserView Applicant { get; set; }

        public string Description { get; set; }

        public string CvFile { get; set; }

        public string ClFile { get; set; }

        public List<ProjectView> Projects { get; set; }

        public DateTime Time { get; set; }

        public bool ClauseOptAccepted { get; set; }

        public bool ClauseOpt2Accepted { get; set; }

        public CodeOverallQuality CodeOverallQuality { get; set; }

        public bool Selected { get; set; }
    }
}