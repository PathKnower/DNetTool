﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_UI_Demonstration.Data
{
    public class News
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public User Editor { get; set; }
    }
}
