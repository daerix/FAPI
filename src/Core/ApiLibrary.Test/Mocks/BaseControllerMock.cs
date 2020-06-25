using System;
using System.Collections.Generic;
using System.Text;
using ApiLibrary.Core.Controllers;
using ApiLibrary.Test.Mocks.Models;

namespace ApiLibrary.Test.Mocks
{
    class BaseControllerMock : BaseController<ModelTest,int,BaseDbContextMock>
    {
        public BaseControllerMock(BaseDbContextMock db) : base(db)
        {

        }
    }
}
