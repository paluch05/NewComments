﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kainos.Comments.Application.Configuration
{
    public class CosmosDbConfiguration
    {
    public string CosmosDbConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ContainerName { get; set; }

    }
}