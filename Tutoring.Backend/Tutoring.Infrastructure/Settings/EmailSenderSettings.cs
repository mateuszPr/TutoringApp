﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tutoring.Infrastructure.Settings
{
    public class EmailSenderSettings
    {
        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public string UsernameEmail { get; set; }

        public string UsernamePassword { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

    }
}
