﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.ComBoost
{
    public interface IDomainRpcClientRequestHandler
    {
        Task HandleAsync(IDomainRpcRequest request, IDomainContext context);
    }
}
