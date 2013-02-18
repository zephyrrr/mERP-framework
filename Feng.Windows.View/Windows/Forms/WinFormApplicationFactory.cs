using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    public class WinFormApplicationFactory : IApplicationFactory
    {
        public IApplication CreateApplication()
        {
            return new TabbedMdiForm();
        }
    }

    public class WinFormApplicationFactory2 : IApplicationFactory
    {
        public IApplication CreateApplication()
        {
            return new TabbedMdiForm2();
        }
    }
}
