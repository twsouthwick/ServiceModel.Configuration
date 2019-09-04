using System.IO;

namespace ServiceModel.Configuration.ConfigurationManager.Tests
{
    internal class TemporaryFileStream : FileStream
    {
        private TemporaryFileStream(string path)
            : base(path, FileMode.Open, FileAccess.Read)
        {
        }

        public static FileStream Create(string xml)
        {
            var path = Path.GetTempFileName();

            File.WriteAllText(path, xml);

            return new TemporaryFileStream(path);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                File.Delete(Name);
            }
        }
    }
}
