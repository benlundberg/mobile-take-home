using System.IO;

namespace GuestlogixTestXF.Core
{
    public interface IFileHelper
    {
        Stream ReadResourceFileAsStream(string name);
    }
}
