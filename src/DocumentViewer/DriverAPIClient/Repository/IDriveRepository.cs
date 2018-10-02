using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverAPIClient.Models;

namespace DriverAPIClient.Repository
{
    public interface IDriveRepository
    {
        DriveModel GetDirectoryFileList(string directoryName);

    }
}
