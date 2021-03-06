﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Resources {
    public class QuizResource {
        public Guid Id { get; set; }

        public String QuizName { get; set; }

        public String Difficulty { get; set; }

        public String ThemeColor { get; set; }

        public IEnumerable<string> Subjects { get; set; }

        public IEnumerable<QuestionResource> Questions { get; set; }
    }
}
    