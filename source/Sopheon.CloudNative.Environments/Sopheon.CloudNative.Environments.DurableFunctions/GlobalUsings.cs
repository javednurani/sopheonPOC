﻿global using System;
global using System.Collections.Generic;
global using System.Data.SqlClient;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.Azure.Functions.Extensions.DependencyInjection;
global using Microsoft.Azure.Management.Fluent;
global using Microsoft.Azure.Management.ResourceManager.Fluent;
global using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
global using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
global using Microsoft.Azure.WebJobs;
global using Microsoft.Azure.WebJobs.Extensions.DurableTask;
global using Microsoft.Azure.WebJobs.Extensions.Http;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Sopheon.CloudNative.Environments.Data;
global using Sopheon.CloudNative.Environments.DurableFunctions.Configuration;
