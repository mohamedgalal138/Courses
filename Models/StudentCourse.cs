﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class StudentCourse
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }  
        public virtual Course Course { get; set; }
    }
}
